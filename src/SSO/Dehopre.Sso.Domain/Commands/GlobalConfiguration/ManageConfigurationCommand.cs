namespace Dehopre.Sso.Domain.Commands.GlobalConfiguration
{
    using Dehopre.Sso.Domain.Models;
    using Dehopre.Sso.Domain.Validations.GlobalConfiguration;

    public class ManageConfigurationCommand : GlobalConfigurationCommand
    {
        public ManageConfigurationCommand(string key, string value, bool sensitive, bool isPublic)
        {
            this.Key = key;
            this.Value = value;
            this.Sensitive = sensitive;
            this.IsPublic = isPublic;
        }
        public override bool IsValid()
        {
            this.ValidationResult = new CreateConfigurationValidation().Validate(this);
            return this.ValidationResult.IsValid;
        }

        public GlobalConfigurationSettings ToEntity() => new(this.Key, this.Value, this.Sensitive, this.IsPublic);
    }
}
