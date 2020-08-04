using System;
using System.Collections.Generic;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using FluentValidation;
using FluentValidation.Results;

namespace CG4U.Donate.Domain.Donations
{
    public class Donation : Entity<Donation>
    {
        public int IdDonations { get; set; }
        public int IdSystems { get; set; }
        public int? IdDonationsDad { get; set; }
        public object Img { get; set; }
        public ICollection<DonationName> Names { get; set; }

        public Donation()
        {
            Names = new List<DonationName>();
        }

        public void AddName(DonationName name) 
        {
            Names.Add(name);
        }

        public override bool IsValid()
        {
            RuleFor(c => c.IdDonations)
                .GreaterThan(0).WithMessage("Donation.IdDonations.GreaterThanZero");

            RuleFor(c => c.IdSystems)
                .GreaterThan(0).WithMessage("Donation.IdSystems.GreaterThanZero")
                .Must(idLanguages => Enum.IsDefined(typeof(Systems), idLanguages) == true)
                .WithMessage("Donation.IdSystems.MemberOfSystemsEnum");

            RuleFor(c => c.Active)
                .Must(active => Enum.IsDefined(typeof(Actives), active) == true)
                .WithMessage("Donation.Active.MemberOfActivesEnum");

            ValidateNames();

            return ValidationResult.IsValid;
        }

        private void ValidateNames()
        {
            foreach (var name in Names)
            {
                if (!name.IsValid())
                    AddOtherErrorList(name.ValidationResult.Errors);
            }
        }
    }
}
