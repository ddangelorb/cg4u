using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;
using CG4U.Core.Services.ViewModels;

namespace CG4U.Donate.WebAPI.ViewModels
{
    public class TradeViewModel : EntityModel<TradeViewModel>
    {
        public int IdTrades { get; set; }

        [Required(ErrorMessage = "Trade.UserGet.NotEmpty")]
        [ValidateObject(ErrorMessage = "Trade.UserGet.NotEmpty")]
        public UserViewModel UserGet { get; set; }

        [Required(ErrorMessage = "Trade.UserLet.NotEmpty")]
        [ValidateObject(ErrorMessage = "Trade.UserLet.NotEmpty")]
        public UserViewModel UserLet { get; set; }

        [Required(ErrorMessage = "Trade.Given.NotEmpty")]
        [ValidateObject(ErrorMessage = "Trade.Given.NotEmpty")]
        public GivenViewModel Given { get; set; }

        [Required(ErrorMessage = "Trade.Desired.NotEmpty")]
        [ValidateObject(ErrorMessage = "Trade.Desired.NotEmpty")]
        public DesiredViewModel Desired { get; set; }

        [Display(Name = "Trade.DtTrade.DisplayName")]
        [Required(ErrorMessage = "Trade.DtTrade.NotEmpty")]
        public DateTime DtTrade { get; set; }

        [Display(Name = "Trade.Commited.DisplayName")]
        [Required(ErrorMessage = "Trade.Commited.NotEmpty")]
        [Range(0, 1, ErrorMessage = "Trade.Commited.MustBe0Or1")]
        public int Commited { get; set; }

        [Required, ValidateListOfObject(ErrorMessage = "Trade.Locations.NotEmpty")]
        public ICollection<LocationViewModel> Locations { get; set; }

        [Required, ValidateListOfObject(ErrorMessage = "Trade.Evaluations.NotEmpty")]
        public ICollection<EvaluationViewModel> Evaluations { get; set; }

        public TradeViewModel()
        {
            Locations = new List<LocationViewModel>();
            Evaluations = new List<EvaluationViewModel>();
        }
    }
}
