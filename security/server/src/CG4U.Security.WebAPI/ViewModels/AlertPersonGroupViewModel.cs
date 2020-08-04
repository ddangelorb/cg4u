using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.WebAPI.ViewModels
{
    public class AlertPersonGroupViewModel : EntityModel<AlertPersonGroupViewModel>
    {
        [Display(Name = "AlertPersonGroup.IdPersonGroups.DisplayName")]
        [Required(ErrorMessage = "AlertPersonGroup.IdPersonGroups.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "AlertPersonGroup.IdPersonGroups.GreaterThanZero")]
        public int IdPersonGroups { get; set; }

        [Display(Name = "AlertPersonGroup.IdAlerts.DisplayName")]
        [Required(ErrorMessage = "AlertPersonGroup.IdAlerts.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "AlertPersonGroup.IdAlerts.GreaterThanZero")]
        public int IdAlerts { get; set; }
    }
}
