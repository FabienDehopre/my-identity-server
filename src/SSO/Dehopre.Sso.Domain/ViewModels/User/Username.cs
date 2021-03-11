namespace Dehopre.Sso.Domain.ViewModels.User
{
    public struct Username
    {
        private readonly string value;

        private Username(string value) => this.value = value;

        public static implicit operator Username(string value) => new Username(value);

        public override string ToString() => this.value;
    }
}
