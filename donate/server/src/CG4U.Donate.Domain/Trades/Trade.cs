using System;
using System.Collections.Generic;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using CG4U.Donate.Domain.Common;
using CG4U.Donate.Domain.Donations;
using FluentValidation;

namespace CG4U.Donate.Domain.Trades
{
    public class Trade : Entity<Trade>
    {
        public int IdTrades { get; set; } 
        public User UserGet { get; set; }
        public User UserLet { get; set; }
        public Given Given { get; set; }
        public Desired Desired { get; set; }
        public DateTime DtTrade { get; set; }
        public int Commited { get; set; }
        public ICollection<Location> Locations { get; set; }
        public ICollection<Evaluation> Evaluations { get; set; }

        public Trade()
        {
            Locations = new List<Location>();
            Evaluations = new List<Evaluation>();
        }

        public void AddLocation(Location location)
        {
            if (location != null)
            {
                location.IdParent = this.Id;
                Locations.Add(location);
            }
        }

        public void AddEvaluation(Evaluation evaluation)
        {
            if (evaluation != null)
            {
                evaluation.IdParent = this.Id;
                Evaluations.Add(evaluation);
            }

        }

        public override bool IsValid()
        {
            RuleFor(c => c.DtTrade)
                .GreaterThan(DateTime.Now)
                .WithMessage("Trade.DtTrade.GreaterThanNow");

            RuleFor(c => c.Commited)
                .Must(active => Enum.IsDefined(typeof(Commits), active) == true)
                .WithMessage("Trade.Commited.MemberOfCommitsEnum");

            RuleFor(c => c.Active)
                .Must(active => Enum.IsDefined(typeof(Actives), active) == true)
                .WithMessage("Trade.Active.MemberOfActivesEnum");

            if (!UserGet.IsValid())
                AddOtherErrorList(UserGet.ValidationResult.Errors);

            if (!UserLet.IsValid())
                AddOtherErrorList(UserLet.ValidationResult.Errors);

            if (!Given.IsValid())
                AddOtherErrorList(Given.ValidationResult.Errors);

            if (!Desired.IsValid())
                AddOtherErrorList(Desired.ValidationResult.Errors);

            ValidateLocations();
            ValidateEvaluations();

            return ValidationResult.IsValid;
        }

        private void ValidateLocations()
        {
            foreach (var location in Locations)
            {
                if (!location.IsValid())
                    AddOtherErrorList(location.ValidationResult.Errors);
            }
        }

        private void ValidateEvaluations()
        {
            foreach (var evaluation in Evaluations)
            {
                if (!evaluation.IsValid())
                    AddOtherErrorList(evaluation.ValidationResult.Errors);
            }
        }
    }
}
