namespace Dehopre.Sso.Domain.ViewModels.User
{
    public struct AccountResult
    {
        public AccountResult(string username)
        {
            this.Username = username;
            this.Code = null;
            this.Url = null;
        }
        public AccountResult(string username, string code, string url)
        {
            this.Username = username;
            this.Code = code;
            this.Url = url;
        }

        public string Username { get; set; }
        public string Code { get; }
        public string Url { get; }
    }
}
