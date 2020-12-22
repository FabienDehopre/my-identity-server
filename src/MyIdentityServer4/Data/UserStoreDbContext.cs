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
            base.OnModelCreating(builder);
        }
    }
}
