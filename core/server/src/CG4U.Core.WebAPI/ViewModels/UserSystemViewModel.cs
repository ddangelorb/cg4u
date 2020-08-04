using System.ComponentModel.DataAnnotations;

namespace CG4U.Core.WebAPI.ViewModels
{
    public class UserSystemViewModel
    {
        [Display(Name = "User.IdUserIdentity.DisplayName")]
        [Required(ErrorMessage = "User.IdUserIdentity.NotEmpty")]
        public string IdUserIdentity { get; set; }

        [Display(Name = "User.IdSystems.DisplayName")]
        [Required(ErrorMessage = "User.IdSystems.NotEmpty")]
        public int IdSystems { get; set; }
    }
}
