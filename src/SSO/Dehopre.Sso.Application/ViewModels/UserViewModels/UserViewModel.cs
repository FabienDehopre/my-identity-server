namespace Dehopre.Sso.Application.ViewModels.UserViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Security.Claims;
    using Dehopre.Domain.Core.Util;
    using Dehopre.Sso.Domain.ViewModels;

    public class UserViewModel
    {
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Telephone")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Picture")]
        public string Picture { get; set; }
        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        public DateTime? Birthdate { get; set; }

        [Display(Name = "Provider")]
        public string Provider { get; set; }

        [Display(Name = "ProviderId")]
        public string ProviderId { get; set; }

        public string Url { get; set; }
        public string Company { get; set; }
        public string Bio { get; set; }
        public string JobTitle { get; set; }
        public string SocialNumber { get; set; }
        public List<Claim> CustomClaims { get; set; } = new List<Claim>();
        public bool ContainsFederationGateway() => this.Provider.IsPresent() && this.ProviderId.IsPresent();

        public void UpdateMetadata(List<Claim> claims)
        {
            this.Url = claims.ValueOf(JwtClaimTypes.WebSite);
            this.Company = claims.ValueOf("company");
            this.Bio = claims.ValueOf("bio");

            if (claims.Contains(JwtClaimTypes.BirthDate))
            {
                this.Birthdate = DateTime.Parse(claims.ValueOf(JwtClaimTypes.BirthDate));
            }

            this.JobTitle = claims.ValueOf("job_title");
            this.SocialNumber = claims.ValueOf("social_number");
            this.Picture = claims.ValueOf(JwtClaimTypes.Picture);
            this.Name = claims.ValueOf(JwtClaimTypes.GivenName);
        }
    }
}
