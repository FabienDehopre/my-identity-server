namespace Dehopre.Sso.Application.ViewModels
{
    public class ConfigurationViewModel
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public bool IsPublic { get; set; }
        public bool IsSensitive { get; set; }

        public void UpdateSensitive()
        {
            if (this.IsSensitive)
            {
                this.Value = "**Sensitive Data**";
            }
        }
    }
}
