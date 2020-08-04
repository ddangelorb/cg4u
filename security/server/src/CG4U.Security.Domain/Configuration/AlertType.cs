using System.ComponentModel.DataAnnotations;
namespace CG4U.Security.Domain.Configuration
{
    public enum AlertType
    {
        [Display(Name = "P", Description = "Panic")]
        Panic = 1,

        [Display(Name = "C", Description = "Critical")]
        Critical = 2,

        [Display(Name = "W", Description = "Warning")]
        Warning = 3
    }
}
