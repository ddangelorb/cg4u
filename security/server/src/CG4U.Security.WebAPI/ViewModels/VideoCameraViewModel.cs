using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.ViewModels;

namespace CG4U.Security.WebAPI.ViewModels
{
    public class VideoCameraViewModel : EntityModel<VideoCameraViewModel>
    {
        [Display(Name = "VideoCamera.IdPersonGroups.DisplayName")]
        [Required(ErrorMessage = "VideoCamera.IdPersonGroups.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "VideoCamera.IdPersonGroups.GreaterThanZero")]
        public int IdPersonGroups { get; set; }

        [Display(Name = "VideoCamera.IdPersonGroupsAPI.DisplayName")]
        [Required(ErrorMessage = "VideoCamera.IdPersonGroupsAPI.NotEmpty")]
        public string IdPersonGroupsAPI { get; set; }

        [Display(Name = "VideoCamera.Name.DisplayName")]
        [Required(ErrorMessage = "VideoCamera.Name.NotEmpty")]
        [MinLength(2, ErrorMessage = "VideoCamera.Name.LengthMin2")]
        [MaxLength(50, ErrorMessage = "VideoCamera.Name.LengthMax50")]
        public string Name { get; set; }

        [Display(Name = "VideoCamera.Description.DisplayName")]
        [Required(ErrorMessage = "VideoCamera.Description.NotEmpty")]
        [MinLength(2, ErrorMessage = "VideoCamera.Description.LengthMin2")]
        [MaxLength(100, ErrorMessage = "VideoCamera.Description.LengthMax100")]
        public string Description { get; set; }

        [Required, ValidateListOfObject(ErrorMessage = "VideoCamera.AnalyzesRequests.NotEmpty")]
        public IList<AnalyzeRequestViewModel> AnalyzesRequests { get; set; }

        public VideoCameraViewModel()
        {
            AnalyzesRequests = new List<AnalyzeRequestViewModel>();
        }
    }
}
