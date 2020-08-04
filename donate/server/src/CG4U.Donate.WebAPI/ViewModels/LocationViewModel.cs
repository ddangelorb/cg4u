using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.WebAPI.ViewModels
{
    public class LocationViewModel : EntityModel<LocationViewModel>
    {
        [Display(Name = "Location.IdLocation.DisplayName")]
        public int IdLocation { get; set; } 

        [Display(Name = "Location.IdParent.DisplayName")]
        [Required(ErrorMessage = "Location.IdParent.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "Location.IdParent.GreaterThanZero")]
        public int IdParent { get; set; }

        [Display(Name = "Location.Address.DisplayName")]
        [Required(ErrorMessage = "Location.Address.NotEmpty")]
        [MinLength(2, ErrorMessage = "Location.Address.LengthMin2")]
        [MaxLength(255, ErrorMessage = "Location.Address.LengthMax255")]
        public string Address { get; set; }

        [Display(Name = "Location.City.DisplayName")]
        [Required(ErrorMessage = "Location.City.NotEmpty")]
        [MinLength(2, ErrorMessage = "Location.City.LengthMin2")]
        [MaxLength(50, ErrorMessage = "Location.City.LengthMax50")]
        public string City { get; set; }

        [Display(Name = "Location.State.DisplayName")]
        [Required(ErrorMessage = "Location.State.NotEmpty")]
        [MinLength(2, ErrorMessage = "Location.State.LengthMin2")]
        [MaxLength(50, ErrorMessage = "Location.State.LengthMax50")]
        public string State { get; set; }

        [Display(Name = "Location.ZipCode.DisplayName")]
        [Required(ErrorMessage = "Location.ZipCode.NotEmpty")]
        [MinLength(2, ErrorMessage = "Location.ZipCode.LengthMin2")]
        [MaxLength(20, ErrorMessage = "Location.ZipCode.LengthMax20")]
        public string ZipCode { get; set; }

        [Display(Name = "Location.Latitude.DisplayName")]
        [Required(ErrorMessage = "Location.Latitude.NotEmpty")]
        public decimal Latitude { get; set; }

        [Display(Name = "Location.Longitude.DisplayName")]
        [Required(ErrorMessage = "Location.Longitude.NotEmpty")]
        public decimal Longitude { get; set; }
    }
}
