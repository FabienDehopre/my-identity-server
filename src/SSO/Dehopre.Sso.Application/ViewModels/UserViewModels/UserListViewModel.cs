namespace Dehopre.Sso.Application.ViewModels.UserViewModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using Dehopre.Domain.Core.Util;
    using Dehopre.Sso.Domain.ViewModels;

    public class UserListViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        public string Name { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }

        [Display(Name = "UserName")]
        public string UserName { get; set; }

        public string Id { get; set; }

        internal void UpdateMetadata(IEnumerable<Claim> claim)
        {
            this.Picture = claim.ValueOf(JwtClaimTypes.Picture);
            this.Name = claim.ValueOf(JwtClaimTypes.GivenName);
        }
    }
}
