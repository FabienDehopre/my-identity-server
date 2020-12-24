namespace MyIdentityServer4.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using MyIdentityServer4.Models;

    public class UserStoreDbContext : IdentityDbContext<User>
    {
        public UserStoreDbContext(DbContextOptions<UserStoreDbContext> options) 
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
#pragma warning disable IDE0022 // Use expression body for methods
            base.OnModelCreating(builder);
#pragma warning restore IDE0022 // Use expression body for methods
        }
    }
}
