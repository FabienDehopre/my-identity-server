namespace Dehopre.Sso.Domain.Validations.GlobalConfiguration
{
    using Dehopre.Sso.Domain.Commands.GlobalConfiguration;

    public class CreateConfigurationValidation : GlobalConfigurationValidation<ManageConfigurationCommand>
    {
        public CreateConfigurationValidation()
        {
            this.ValidateValue();
            this.ValidateKey();
        }
    }
}
