using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.WebAPI.ViewModels
{
    public class AnalyzeRequestViewModel : EntityModel<AnalyzeRequestViewModel>
    {
        [Display(Name = "AnalyzeRequest.IdBillableProcesses.DisplayName")]
        [Required(ErrorMessage = "AnalyzeRequest.IdBillableProcesses.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "AnalyzeRequest.IdBillableProcesses.GreaterThanZero")]
        public int IdBillableProcesses { get; set; }

        [Display(Name = "AnalyzeRequest.IdLanguages.DisplayName")]
        [Required(ErrorMessage = "AnalyzeRequest.IdLanguages.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "AnalyzeRequest.IdLanguages.GreaterThanZero")]
        public int IdLanguages { get; set; }

        [Display(Name = "AnalyzeRequest.AnalyzeOrder.DisplayName")]
        [Required(ErrorMessage = "AnalyzeRequest.AnalyzeOrder.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "AnalyzeRequest.AnalyzeOrder.GreaterThanZero")]
        public int AnalyzeOrder { get; set; }

        [Display(Name = "AnalyzeRequest.TypeCode.DisplayName")]
        [Required(ErrorMessage = "AnalyzeRequest.TypeCode.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "AnalyzeRequest.TypeCode.GreaterThanZero")]
        public int TypeCode { get; set; }

        [Display(Name = "AnalyzeRequest.TypeName.DisplayName")]
        [Required(ErrorMessage = "AnalyzeRequest.TypeName.NotEmpty")]
        [MinLength(2, ErrorMessage = "AnalyzeRequest.TypeName.LengthMin2")]
        [MaxLength(50, ErrorMessage = "AnalyzeRequest.TypeName.LengthMax50")]
        public string TypeName { get; set; }

        [Display(Name = "AnalyzeRequest.IdAnalyzesRequestsVideoCameras.DisplayName")]
        public int IdAnalyzesRequestsVideoCameras { get; set; }

        [Display(Name = "AnalyzeRequest.Location.DisplayName")]
        public string Location { get; set; }

        [Display(Name = "AnalyzeRequest.SubscriptionKey.DisplayName")]
        public string SubscriptionKey { get; set; }
    }
}
