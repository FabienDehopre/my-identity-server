namespace Dehopre.Sso.Domain.Commands.Email
{
    using Dehopre.Domain.Core.Commands;

    public abstract class TemplateCommand : Command
    {
        public string Subject { get; protected set; }
        public string Content { get; protected set; }
        public string Name { get; protected set; }
        public string OldName { get; protected set; }
        public string UserName { get; protected set; }
        public bool Active { get; protected set; }

    }
}
