using System;
using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.ViewModels;

namespace CG4U.Donate.WebAPI.ViewModels
{
    public class DesiredViewModel : EntityModel<DesiredViewModel>
    {
        [Display(Name = "Desired.IdDonationsDesired.DisplayName")]
        public int IdDonationsDesired { get; set; }

        [Required(ErrorMessage = "Desired.Donation.NotEmpty")]
        [ValidateObject(ErrorMessage = "Desired.Donation.NotEmpty")]
        public DonationViewModel Donation { get; set; }

        [Required(ErrorMessage = "Desired.User.NotEmpty")]
        [ValidateObject(ErrorMessage = "Desired.User.NotEmpty")]
        public UserViewModel User { get; set; }

        [Display(Name = "Desired.DtUpdate.DisplayName")]
        [Required(ErrorMessage = "Desired.DtUpdate.NotEmpty")]
        public DateTime DtUpdate { get; set; }

        [Display(Name = "Desired.DtExp.DisplayName")]
        public DateTime? DtExp { get; set; }

        [Required(ErrorMessage = "Desired.Location.NotEmpty")]
        [ValidateObject(ErrorMessage = "Desired.Location.NotEmpty")]
        public LocationViewModel Location { get; set; }

        [Display(Name = "Desired.MaxGetinMeters.DisplayName")]
        public double? MaxGetinMeters { get; set; }

        public DesiredViewModel()
        {
            Donation = new DonationViewModel();
            User = new UserViewModel();
            Location = new LocationViewModel();
        }
    }
}
