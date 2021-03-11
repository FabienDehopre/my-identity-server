namespace Dehopre.Sso.Domain.Models
{
    public class Sender
    {
        public Sender() { }
        public Sender(string address, string name)
        {
            this.Address = address;
            this.Name = name;
        }

        public string Address { get; set; }
        public string Name { get; set; }

        public bool IsValid() => !string.IsNullOrEmpty(this.Name) || !string.IsNullOrEmpty(this.Address);
    }
}
