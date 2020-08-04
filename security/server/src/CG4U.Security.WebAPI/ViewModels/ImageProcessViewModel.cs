using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.ViewModels;

namespace CG4U.Security.WebAPI.ViewModels
{
    public class ImageProcessViewModel : EntityModel<ImageProcessViewModel>
    {
        [Display(Name = "ImageProcess.IdVideoCameras.DisplayName")]
        [Required(ErrorMessage = "ImageProcess.IdVideoCameras.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "ImageProcess.IdVideoCameras.GreaterThanZero")]
        public int IdVideoCameras { get; set; }

        [Display(Name = "ImageProcess.IdReference.DisplayName")]
        public Guid IdReference { get; set; }

        [Display(Name = "ImageProcess.ImageFile.DisplayName")]
        public byte[] ImageFile { get; set; }

        [Display(Name = "ImageProcess.ImageName.DisplayName")]
        [Required(ErrorMessage = "ImageProcess.ImageName.NotEmpty")]
        [MinLength(2, ErrorMessage = "ImageProcess.ImageName.LengthMin2")]
        [MaxLength(100, ErrorMessage = "ImageProcess.ImageName.LengthMax100")]
        public string ImageName { get; set; }

        [Display(Name = "ImageProcess.IpUserRequest.DisplayName")]
        public string IpUserRequest { get; set; }

        [Display(Name = "ImageProcess.VideoPath.DisplayName")]
        [Required(ErrorMessage = "ImageProcess.VideoPath.NotEmpty")]
        [MinLength(5, ErrorMessage = "ImageProcess.VideoPath.LengthMin5")]
        [MaxLength(255, ErrorMessage = "ImageProcess.VideoPath.LengthMax255")]
        public string VideoPath { get; set; }

        [Display(Name = "ImageProcess.SecondsToStart.DisplayName")]
        [Required(ErrorMessage = "ImageProcess.SecondsToStart.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "ImageProcess.SecondsToStart.GreaterThanZero")]
        public int SecondsToStart { get; set; }

        [Display(Name = "ImageProcess.DtProcess.DisplayName")]
        [Required(ErrorMessage = "ImageProcess.DtProcess.NotEmpty")]
        public DateTime DtProcess { get; set; }

        [Required, ValidateListOfObject(ErrorMessage = "ImageProcess.Analyzes.NotEmpty")]
        public ICollection<ImageProcessAnalyzeViewModel> Analyzes { get; set; }

        public ImageProcessViewModel()
        {
            Analyzes = new List<ImageProcessAnalyzeViewModel>();
        }
    }
}
