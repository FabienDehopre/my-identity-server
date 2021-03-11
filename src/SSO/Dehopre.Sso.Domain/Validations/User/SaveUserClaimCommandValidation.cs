namespace Dehopre.Sso.Domain.Validations.User
{
    using Dehopre.Sso.Domain.Commands.User;

    public class SaveUserClaimCommandValidation : UserClaimValidation<SaveUserClaimCommand>
    {
        public SaveUserClaimCommandValidation()
        {
            this.ValidateUsername();
            this.ValidateKey();
            this.ValidateValue();
        }
    }
}
