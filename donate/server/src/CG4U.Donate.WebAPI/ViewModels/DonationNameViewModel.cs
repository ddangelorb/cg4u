using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.WebAPI.ViewModels
{
    public class DonationNameViewModel : EntityModel<DonationNameViewModel>
    {
        public int IdDonationsNames { get; set; }

        [Display(Name = "DonationName.IdDonations.DisplayName")]
        [Required(ErrorMessage = "DonationName.IdDonations.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "DonationName.IdDonations.GreaterThanZero")]
        public int IdDonations { get; set; }

        [Display(Name = "DonationName.IdLanguages.DisplayName")]
        [Required(ErrorMessage = "DonationName.IdLanguages.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "DonationName.IdLanguages.GreaterThanZero")]
        public int IdLanguages { get; set; }

        [Display(Name = "DonationName.Name.DisplayName")]
        [Required(ErrorMessage = "DonationName.Name.NotEmpty")]
        [MinLength(2, ErrorMessage = "DonationName.Name.LengthMin2")]
        [MaxLength(50, ErrorMessage = "DonationName.Name.LengthMax50")]
        public string Name { get; set; }
    }
}
