namespace Dehopre.Sso.Application.ViewModels
{
    using System.Text.Json.Serialization;

    public class ProfilePictureViewModel : FileUploadViewModel
    {
        [JsonIgnore]
        public string Picture { get; set; }
    }
}
