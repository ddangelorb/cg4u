using System;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using FluentValidation;
using FluentValidation.Results;

namespace CG4U.Donate.Domain.Donations
{
    public class DonationName : Entity<DonationName>
    {
        public int IdDonationsNames { get; set; }
        public int IdDonations { get; set; }
        public int IdLanguages { get; set; }
        public string Name { get; set; }

        public override bool IsValid()
        {
            RuleFor(c => c.IdDonationsNames)
                .GreaterThan(0).WithMessage("DonationName.IdDonationsNames.GreaterThanZero");

            RuleFor(c => c.IdLanguages)
                .GreaterThan(0)
                    .WithMessage("DonationName.IdLanguages.GreaterThanZero")
                .Must(idLanguages => Enum.IsDefined(typeof(Languages), idLanguages) == true)
                    .WithMessage("DonationName.IdLanguages.MemberOfLanguagesEnum");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("DonationName.Name.NotEmpty")
                .Length(2, 50).WithMessage("DonationName.Name.LengthBetween2And50");

            RuleFor(c => c.Active)
                .Must(active => Enum.IsDefined(typeof(Actives), active) == true)
                .WithMessage("DonationName.Active.MemberOfActivesEnum");
            
            return ValidationResult.IsValid;
        }
    }
}
