using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Security.WebAPI.ViewModels
{
    public class CarViewModel : EntityModel<CarViewModel>
    {
        [Display(Name = "Car.IdPersons.DisplayName")]
        [Required(ErrorMessage = "Car.IdPersons.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "Car.IdPersons.GreaterThanZero")]
        public int IdPersons { get; set; }

        [Display(Name = "Car.PlateCode.DisplayName")]
        [Required(ErrorMessage = "Car.PlateCode.NotEmpty")]
        [MinLength(2, ErrorMessage = "Car.PlateCode.LengthMin2")]
        [MaxLength(15, ErrorMessage = "Car.PlateCode.LengthMax15")]
        public string PlateCode { get; set; }
    }
}
