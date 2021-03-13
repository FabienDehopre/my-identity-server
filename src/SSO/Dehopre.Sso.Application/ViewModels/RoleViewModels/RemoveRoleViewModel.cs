namespace Dehopre.Sso.Application.ViewModels.RoleViewModels
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveRoleViewModel
    {
        public RemoveRoleViewModel(string name) => this.Name = name;

        [Required]
        public string Name { get; set; }
    }
}
