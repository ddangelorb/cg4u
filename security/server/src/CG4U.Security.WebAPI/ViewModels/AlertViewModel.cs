using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.WebAPI.ViewModels
{
    public class AlertViewModel : EntityModel<AlertViewModel>
    {
        [Display(Name = "Alert.IdAnalyzesRequests.DisplayName")]
        [Required(ErrorMessage = "Alert.IdAnalyzesRequests.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "Alert.IdAnalyzesRequests.GreaterThanZero")]
        public int IdAnalyzesRequests { get; set; }

        [Display(Name = "Alert.TypeCode.DisplayName")]
        [Required(ErrorMessage = "Alert.TypeCode.NotEmpty")]
        [Range(1, 3, ErrorMessage = "Alert.TypeCode.GreaterThanZero")]
        public int TypeCode { get; set; }

        [Display(Name = "Alert.Message.DisplayName")]
        [Required(ErrorMessage = "Alert.Message.NotEmpty")]
        [MinLength(2, ErrorMessage = "Alert.Message.LengthMin2")]
        [MaxLength(255, ErrorMessage = "Alert.Message.LengthMax255")]
        public string Message { get; set; }

        [Display(Name = "Alert.ProcessingMethod.DisplayName")]
        [Required(ErrorMessage = "Alert.ProcessingMethod.NotEmpty")]
        [MinLength(2, ErrorMessage = "Alert.ProcessingMethod.LengthMin2")]
        [MaxLength(50, ErrorMessage = "Alert.ProcessingMethod.LengthMax50")]
        public string ProcessingMethod { get; set; }

        [Display(Name = "Alert.ProcessingParam.DisplayName")]
        public string ProcessingParam { get; set; }
    }
}
