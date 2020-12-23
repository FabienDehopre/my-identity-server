namespace MyIdentityServer4
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.Claims;
    using System.Security.Cryptography;
    using IdentityModel;
    using IdentityServer4;
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Mappers;
    using IdentityServer4.Services;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.IdentityModel.Tokens;
    using MyIdentityServer4.Data;
    using MyIdentityServer4.Infrastructure;
    using MyIdentityServer4.Models;
    using MyIdentityServer4.Settings;
    using Serilog;

    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            this.Environment = environment ?? throw new ArgumentNullException(nameof(environment));
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<UserStoreDbContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("UserStoreConnection")));
            services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<UserStoreDbContext>().AddDefaultTokenProviders();
            services.AddTransient<IEventSink, SentryEventSink>();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            var builder = services
                .AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    // see https://identityserver4.readthedocs.io/en/latest/topics/resources.html
                    options.EmitStaticAudienceClaim = true;
                })
#pragma warning disable IDE0053 // Use expression body for lambda expressions
                .AddConfigurationStore(options =>
#pragma warning restore IDE0053 // Use expression body for lambda expressions
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(this.Configuration.GetConnectionString("ConfigurationStoreConnection"), sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(this.Configuration.GetConnectionString("OperationalStoreConnection"), sql => sql.MigrationsAssembly(migrationsAssembly));
                    options.EnableTokenCleanup = true;
                })
                .AddAspNetIdentity<User>();

            if (this.Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                builder
                    .AddSigningCredential(this.LoadECDsaSecurityKey("3594ab85ad5d4784a5c862fc28e53a6b"), IdentityServerConstants.ECDsaSigningAlgorithm.ES256)
                    .AddSigningCredential(this.LoadRsaSecurityKey("d327491fa24349389fcf87ea53d2b1d2"), IdentityServerConstants.RsaSigningAlgorithm.PS256)
                    .AddSigningCredential(this.LoadRsaSecurityKey("e3626be31f7f48eca843ded4abc4cbf1"), IdentityServerConstants.RsaSigningAlgorithm.RS256);
            }

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    var settings = this.Configuration.GetSection("Authentication:Google").Get<ExternalAuthentication>();
                    options.ClientId = settings.ClientId;
                    options.ClientSecret = settings.ClientSecret;
                })
                .AddMicrosoftAccount(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    var settings = this.Configuration.GetSection("Authentication:Microsoft").Get<ExternalAuthentication>();
                    options.ClientId = settings.ClientId;
                    options.ClientSecret = settings.ClientSecret;
                });

            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            this.InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
        }

        private ECDsaSecurityKey LoadECDsaSecurityKey(string keyId)
        {
            var path = Path.Combine("certs", "ec", $"{keyId}.pem");
            var ecdsa = ECDsa.Create();
            ecdsa.ImportECPrivateKey(this.LoadPrivateKey(path), out _);
            return new ECDsaSecurityKey(ecdsa) { KeyId = keyId };
        }

        private RsaSecurityKey LoadRsaSecurityKey(string keyId)
        {
            var path = Path.Combine("certs", "rsa", $"{keyId}.pem");
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(this.LoadPrivateKey(path), out _);
            return new RsaSecurityKey(rsa) { KeyId = keyId };
        }

        private byte[] LoadPrivateKey(string path)
        {
            using (var stream = this.Environment.ContentRootFileProvider.GetFileInfo(path).CreateReadStream())
            using (var reader = new StreamReader(stream))
            {
                var rawData = this.ReadAllLinesFromStream(reader);
                var data = string.Join("", rawData);
                return Convert.FromBase64String(data);
            }
        }

        private string[] ReadAllLinesFromStream(StreamReader reader)
        {
            var lines = new List<string>();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                lines.Add(line);
            }

            return lines.Skip(1).SkipLast(1).ToArray();
        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var configurationDbContext = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                configurationDbContext.Database.Migrate();

                if (!configurationDbContext.Clients.Any())
                {
                    Log.Debug("Clients being populated");
                    foreach (var client in Config.Clients)
                    {
                        configurationDbContext.Clients.Add(client.ToEntity());
                    }

                    configurationDbContext.SaveChanges();
                }
                else
                {
                    Log.Debug("Clients already populated.");
                }

                if (!configurationDbContext.IdentityResources.Any())
                {
                    Log.Debug("IdentityResources being populated.");
                    foreach (var resource in Config.IdentityResources)
                    {
                        configurationDbContext.IdentityResources.Add(resource.ToEntity());
                    }

                    configurationDbContext.SaveChanges();
                }
                else
                {
                    Log.Debug("IdentityResources already populated.");
                }

                if (!configurationDbContext.ApiScopes.Any())
                {
                    Log.Debug("ApiScopes being populated.");
                    foreach (var scope in Config.ApiScopes)
                    {
                        configurationDbContext.ApiScopes.Add(scope.ToEntity());
                    }

                    configurationDbContext.SaveChanges();
                }
                else
                {
                    Log.Debug("ApiScopes already populated.");
                }

                var userStoreDbContext = serviceScope.ServiceProvider.GetRequiredService<UserStoreDbContext>();
                userStoreDbContext.Database.Migrate();

                var userMgr = serviceScope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var alice = userMgr.FindByNameAsync("alice").Result;
                if (alice == null)
                {
                    alice = new User
                    {
                        UserName = "alice",
                        Email = "alice.smith@email.com",
                        EmailConfirmed = true,
                    };
                    var result = userMgr.CreateAsync(alice, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(alice, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, "Alice Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Alice"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.WebSite, "http://alice.com"),
                    }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    Log.Debug("Alice created.");
                }
                else
                {
                    Log.Debug("Alice already exists");
                }

                var bob = userMgr.FindByNameAsync("bob").Result;
                if (bob == null)
                {
                    bob = new User
                    {
                        UserName = "bob",
                        Email = "bob.smith@email.com",
                        EmailConfirmed = true,
                    };
                    var result = userMgr.CreateAsync(bob, "Pass123$").Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    result = userMgr.AddClaimsAsync(bob, new Claim[]
                    {
                        new Claim(JwtClaimTypes.Name, "Bob Smith"),
                        new Claim(JwtClaimTypes.GivenName, "Bob"),
                        new Claim(JwtClaimTypes.FamilyName, "Smith"),
                        new Claim(JwtClaimTypes.WebSite, "http://bob.com"),
                        new Claim("location", "somewhere"),
                    }).Result;
                    if (!result.Succeeded)
                    {
                        throw new Exception(result.Errors.First().Description);
                    }

                    Log.Debug("Bob created.");
                }
                else
                {
                    Log.Debug("Bob already exists");
                }
            }
        }
    }
}
