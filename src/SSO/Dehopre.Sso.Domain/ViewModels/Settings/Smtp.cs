namespace Dehopre.Sso.Domain.ViewModels.Settings
{
    public class Smtp
    {
        public Smtp(string server, string port, string useSsl, string password, string username)
        {
            this.Server = server;
            this.Password = password;
            this.Username = username;

            if (int.TryParse(port, out var portNumber))
            {
                this.Port = portNumber;
            }

            if (bool.TryParse(useSsl, out var ssl))
            {
                this.UseSsl = ssl;
            }
        }
        public string Server { get; }
        public int Port { get; }
        public bool UseSsl { get; }
        public string Password { get; }
        public string Username { get; }
    }
}
