using System.ComponentModel.DataAnnotations;

namespace CG4U.Core.Common.Domain.Interfaces
{
    public enum UserRoles
    {
        [Display(Name = "Admin", Description = "CG4U system Admin")]
        Admin,
        [Display(Name = "UserDonate", Description = "CG4U Donate ordinary user")]
        UserDonate
    }
}
