namespace Dehopre.Sso.EntityFramework.Repository.Mappings
{
    using Dehopre.Sso.Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EmailMap : IEntityTypeConfiguration<Email>
    {
        public void Configure(EntityTypeBuilder<Email> builder)
        {
            _ = builder.HasKey(k => k.Id);
            _ = builder.Property(p => p.Subject).IsRequired().HasMaxLength(250);

            _ = builder.OwnsOne(o => o.Sender, c =>
              {
                  _ = c.Property(p => p.Address).IsRequired().HasMaxLength(250);
                  _ = c.Property(p => p.Name).IsRequired().HasMaxLength(250);
              });
            _ = builder.OwnsOne(typeof(BlindCarbonCopy), "Bcc", o =>
              {
                  _ = o.Ignore("Recipients");
                  _ = o.Property("_recipientsCollection");
              });

        }

    }
}
