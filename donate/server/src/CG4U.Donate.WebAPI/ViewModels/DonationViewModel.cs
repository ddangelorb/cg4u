using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.ViewModels;

namespace CG4U.Donate.WebAPI.ViewModels
{
    public class DonationViewModel : EntityModel<DonationViewModel>
    {
        [Required(ErrorMessage = "Donation.IdDonations.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "Donation.IdDonations.GreaterThanZero")]
        public int IdDonations { get; set; }

        [Required(ErrorMessage = "Donation.IdSystems.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "Donation.IdSystems.GreaterThanZero")]
        public int IdSystems { get; set; }

        public int? IdDonationsDad { get; set; }

        public object Img { get; set; }

        [Required, ValidateListOfObject(ErrorMessage = "Donation.Names.NotEmpty")]
        public ICollection<DonationNameViewModel> Names { get; set; }

        public DonationViewModel()
        {
            Names = new List<DonationNameViewModel>();
        }
    }
}
