using System;
using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.ViewModels;

namespace CG4U.Security.WebAPI.ViewModels
{
    public class ImageProcessAnalyzeViewModel : EntityModel<ImageProcessAnalyzeViewModel>
    {
        [Display(Name = "ImageProcessAnalyze.IdReference.DisplayName")]
        public Guid IdReference { get; set; }

        [Display(Name = "ImageProcessAnalyze.IdImageProcesses.DisplayName")]
        [Required(ErrorMessage = "ImageProcessAnalyze.IdImageProcesses.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "ImageProcessAnalyze.IdImageProcesses.GreaterThanZero")]
        public int IdImageProcesses { get; set; }

        [Display(Name = "ImageProcessAnalyze.IdAnalyzesRequestsVideoCameras.DisplayName")]
        public int IdAnalyzesRequestsVideoCameras { get; set; }

        [Display(Name = "ImageProcessAnalyze.DtAnalyze.DisplayName")]
        [Required(ErrorMessage = "ImageProcessAnalyze.DtAnalyze.NotEmpty")]
        public DateTime DtAnalyze { get; set; }

        [Display(Name = "ImageProcessAnalyze.ReturnResponseType.DisplayName")]
        [Required(ErrorMessage = "ImageProcessAnalyze.ReturnResponseType.NotEmpty")]
        [MinLength(1, ErrorMessage = "ImageProcessAnalyze.ReturnResponseType.LengthMin1")]
        [MaxLength(5, ErrorMessage = "ImageProcessAnalyze.ReturnResponseType.LengthMax5")]
        public string ReturnResponseType { get; set; }

        [Display(Name = "ImageProcessAnalyze.ReturnResponse.DisplayName")]
        public string ReturnResponse { get; set; }

        [Display(Name = "ImageProcessAnalyze.Commited.DisplayName")]
        [Required(ErrorMessage = "ImageProcessAnalyze.Commited.NotEmpty")]
        [Range(0, 1, ErrorMessage = "ImageProcessAnalyze.Commited.MustBe0Or1")]
        public int Commited { get; set; }
    }
}
