namespace Dehopre.Sso.EntityFramework.Repository.Mappings
{
    using Dehopre.EntityFrameworkCore.Mappings;
    using Microsoft.EntityFrameworkCore;

    public static class ModelBuilderExtensions
    {
        public static void ConfigureEventStoreContext(this ModelBuilder builder)
        {
            _ = builder.ApplyConfiguration(new StoredEventMap());
            _ = builder.ApplyConfiguration(new EventDetailsMap());
        }

        public static void ConfigureSsoContext(this ModelBuilder builder)
        {
            _ = builder.ApplyConfiguration(new EmailMap());
            _ = builder.ApplyConfiguration(new TemplateMap());
        }
    }
}
