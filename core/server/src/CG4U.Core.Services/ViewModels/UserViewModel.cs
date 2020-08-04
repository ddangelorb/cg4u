using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace CG4U.Core.Services.ViewModels
{
    public class UserViewModel : EntityModel<UserViewModel>
    {
        [Display(Name = "User.IdUserIdentity.DisplayName")]
        public string IdUserIdentity { get; set; }

        [Display(Name = "User.IdLanguages.DisplayName")]
        public int IdLanguages { get; set; }

        [MinLength(6, ErrorMessage = "User.Password.LengthMin2")]
        [MaxLength(100, ErrorMessage = "User.Password.LengthMax100")]
        [DataType(DataType.Password)]
        [Display(Name = "User.Password.DisplayName")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "User.PasswordConfirmation.DisplayName")]
        [Compare("Password", ErrorMessage = "User.PasswordConfirmation.ValueMustBeEqualPassword")]
        public string PasswordConfirmation { get; set; }

        [Display(Name = "User.Avatar.DisplayName")]
        public byte[] Avatar { get; set; }

        [Display(Name = "User.IdSystems.DisplayName")]
        [Required(ErrorMessage = "User.Email.NotEmpty")]
        public int IdSystems { get; set; }

        [Display(Name = "User.Email.DisplayName")]
        [Required(ErrorMessage = "User.Email.NotEmpty")]
        [EmailAddress(ErrorMessage = "User.Email.InvalidFormat")]
        [MaxLength(320, ErrorMessage = "User.Email.LengthMax320")]
        public string Email { get; set; }

        [Display(Name = "User.FirstName.DisplayName")]
        [MinLength(2, ErrorMessage = "User.FirstName.LengthMin2")]
        [MaxLength(50, ErrorMessage = "User.FirstName.LengthMax50")]
        public string FirstName { get; set; }

        [Display(Name = "User.SurName.DisplayName")]
        [MinLength(2, ErrorMessage = "User.SurName.LengthMin2")]
        [MaxLength(206, ErrorMessage = "User.SurName.LengthMax206")]
        public string SurName { get; set; }

        [Display(Name = "User.Authenticated.DisplayName")]
        [Range(0, 1, ErrorMessage = "User.Authenticated.MustBe0Or1")]
        public int Authenticated { get; set; }

        [Display(Name = "User.DtExpAuth.DisplayName")]
        public DateTime DtExpAuth { get; set; }

        [Display(Name = "User.IdentityUser.DisplayName")]
        public IdentityUser IdentityUser { get; set; }

        [Required, ValidateListOfObject(ErrorMessage = "User.Roles.NotEmpty")]
        public ICollection<UserRoles> Roles { get; set; }

        public UserViewModel()
        {
            IdentityUser = new IdentityUser();
            Roles = new List<UserRoles>();
        }
    }
}
