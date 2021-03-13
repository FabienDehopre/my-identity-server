namespace Dehopre.Sso.Application.AutoMapper
{
    using global::AutoMapper;

    public static class UserMapping
    {
        static UserMapping() => Mapper = new MapperConfiguration(cfg => cfg.AddProfile<UserMapperProfile>()).CreateMapper();

        internal static IMapper Mapper { get; private set; }

    }
}
