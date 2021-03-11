namespace Dehopre.Sso.Domain.Commands.GlobalConfiguration
{
    using Dehopre.Domain.Core.Commands;

    public abstract class GlobalConfigurationCommand : Command
    {
        public string Key { get; protected set; }
        public string Value { get; protected set; }
        public bool Sensitive { get; protected set; }
        public bool IsPublic { get; protected set; }
        public bool Public { get; protected set; }
    }
}
