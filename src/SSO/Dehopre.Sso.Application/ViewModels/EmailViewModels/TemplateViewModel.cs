namespace Dehopre.Sso.Application.ViewModels.EmailViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class TemplateViewModel
    {
        [Required]
        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Name { get; set; }
        public string Username { get; set; }
        public string OldName { get; set; }

        public TemplateViewModel SetOldName(string oldname)
        {
            this.OldName = oldname;
            return this;
        }

    }
}
