namespace Dehopre.Sso.Application.AutoMapper
{
    using global::AutoMapper;

    public static class EmailMapping
    {
        internal static IMapper Mapper { get; }

        static EmailMapping() => Mapper = new MapperConfiguration(cfg => cfg.AddProfile<EmailMapperProfile>()).CreateMapper();
    }
}
