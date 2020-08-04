using System;
using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.WebAPI.ViewModels
{
    public class FaceViewModel : EntityModel<FaceViewModel>
    {
        [Display(Name = "Face.IdPersons.DisplayName")]
        [Required(ErrorMessage = "Face.IdPersons.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "Face.IdPersons.GreaterThanZero")]
        public int IdPersons { get; set; }

        [Display(Name = "Face.FaceId.DisplayName")]
        public Guid FaceId { get; set; }

        [Display(Name = "Face.Image.DisplayName")]
        [Required(ErrorMessage = "Face.Image.NotEmpty")]
        public byte[] Image { get; set; }
    }
}
