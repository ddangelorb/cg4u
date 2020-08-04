using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.WebAPI.ViewModels
{
    public class AnalyzeRequestVideoCameraViewModel : EntityModel<AnalyzeRequestVideoCameraViewModel>
    {
        [Display(Name = "AnalyzeRequestVideoCamera.IdAnalyzesRequests.DisplayName")]
        [Required(ErrorMessage = "AnalyzeRequestVideoCamera.IdAnalyzesRequests.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "AnalyzeRequestVideoCamera.IdAnalyzesRequests.GreaterThanZero")]
        public int IdAnalyzesRequests { get; set; }

        [Display(Name = "AnalyzeRequestVideoCamera.IdVideoCameras.DisplayName")]
        [Required(ErrorMessage = "AnalyzeRequestVideoCamera.IdVideoCameras.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "AnalyzeRequestVideoCamera.IdVideoCameras.GreaterThanZero")]
        public int IdVideoCameras { get; set; }
    }
}
