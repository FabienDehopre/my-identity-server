namespace Dehopre.Sso.Domain.ViewModels.User
{
    using Dehopre.Sso.Domain.Interfaces;

    public class UserSearch<TKey> : IUserSearch
    {
        public TKey[] Id { get; set; }
        public string Sort { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
    }
}
