using System;
using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.ViewModels;

namespace CG4U.Donate.WebAPI.ViewModels
{
    public class GivenViewModel : EntityModel<GivenViewModel>
    {
        [Display(Name = "Given.IdDonationsGivens.DisplayName")]
        public int IdDonationsGivens { get; set; }

        [Required(ErrorMessage = "Given.Donation.NotEmpty")]
        [ValidateObject(ErrorMessage = "Given.Donation.NotEmpty")]
        public DonationViewModel Donation { get; set; }

        [Required(ErrorMessage = "Given.User.NotEmpty")]
        [ValidateObject(ErrorMessage = "Given.User.NotEmpty")]
        public UserViewModel User { get; set; }

        [Display(Name = "Given.DtUpdate.DisplayName")]
        [Required(ErrorMessage = "Given.DtUpdate.NotEmpty")]
        public DateTime DtUpdate { get; set; }

        [Display(Name = "Given.DtExp.DisplayName")]
        public DateTime? DtExp { get; set; }

        [Display(Name = "Given.Img.DisplayName")]
        public byte[] Img { get; set; }

        [Required(ErrorMessage = "Given.Location.NotEmpty")]
        [ValidateObject(ErrorMessage = "Given.Location.NotEmpty")]
        public LocationViewModel Location { get; set; }

        [Display(Name = "Given.MaxGetinMeters.DisplayName")]
        public double? MaxLetinMeters { get; set; }

        public GivenViewModel()
        {
            Donation = new DonationViewModel();
            User = new UserViewModel();
            Location = new LocationViewModel();
        }
    }
}
