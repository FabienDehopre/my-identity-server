namespace Dehopre.Domain.Core.ViewModels
{
    public class ClaimViewModel
    {
        public ClaimViewModel() { }

        public ClaimViewModel(string type, string value)
        {
            this.Type = type;
            this.Value = value;
        }

        public string Type { get; set; }
        public string Value { get; set; }
    }
}
