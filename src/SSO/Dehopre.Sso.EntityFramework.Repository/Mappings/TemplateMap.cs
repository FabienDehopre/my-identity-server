namespace Dehopre.Sso.EntityFramework.Repository.Mappings
{
    using Dehopre.Sso.Domain.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class TemplateMap : IEntityTypeConfiguration<Template>
    {
        public void Configure(EntityTypeBuilder<Template> builder)
        {
            _ = builder.HasKey(k => k.Id);
            _ = builder.Property(p => p.Content).HasMaxLength(int.MaxValue).IsRequired();
            _ = builder.Property(p => p.Name).IsRequired().HasMaxLength(250);
        }
    }
}
