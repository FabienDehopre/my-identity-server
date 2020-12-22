namespace MyIdentityServer4
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Security.Cryptography;
    using IdentityServer4;
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
            services.AddDbContext<UserStoreDbContext>(options => options.UseSqlServer(this.Configuration.GetConnectionString("UserStoreConnectionString")));
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
                    options.ConfigureDbContext = builder => builder.UseSqlServer(this.Configuration.GetConnectionString("ConfigurationStoreConnectionString"), sql => sql.MigrationsAssembly(migrationsAssembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder => builder.UseSqlServer(this.Configuration.GetConnectionString("OperationalStoreConnectionString"), sql => sql.MigrationsAssembly(migrationsAssembly));
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
                    var (ClientId, ClientSecret) = this.Configuration.GetSection("Authentication:Google").Get<(string ClientId, string ClientSecret)>();
                    options.ClientId = ClientId;
                    options.ClientSecret = ClientSecret;
                })
                .AddMicrosoftAccount(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    var (ClientId, ClientSecret) = this.Configuration.GetSection("Authentication:Microsoft").Get<(string ClientId, string ClientSecret)>();
                    options.ClientId = ClientId;
                    options.ClientSecret = ClientSecret;
                });

            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
#pragma warning disable IDE0053 // Use expression body for lambda expressions
            app.UseEndpoints(endpoints =>
#pragma warning restore IDE0053 // Use expression body for lambda expressions
            {
                endpoints.MapDefaultControllerRoute();
            });
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
    }
}
