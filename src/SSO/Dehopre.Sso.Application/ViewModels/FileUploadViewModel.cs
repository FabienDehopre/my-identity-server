namespace Dehopre.Sso.Application.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using IdentityModel;

    public class FileUploadViewModel
    {
        public string Username { get; set; }

        [Required(ErrorMessage = "Invalid file")]
        public string Filename { get; set; }

        [Required(ErrorMessage = "Invalid file")]
        public string FileType { get; set; }

        [Required(ErrorMessage = "Invalid file")]
        public string Value { get; set; }

        public string VirtualLocation { get; set; }

        public void Normalize() => this.Filename = $"{this.Filename.ToSha256()}{Path.GetExtension(this.Filename)}";
    }
}
