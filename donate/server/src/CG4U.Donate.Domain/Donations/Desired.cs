using System;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using CG4U.Donate.Domain.Common;
using FluentValidation;

namespace CG4U.Donate.Domain.Donations
{
    public class Desired : Entity<Desired>
    {
        public int IdDonationsDesired { get; set; }
        public Donation Donation { get; set; }
        public User User { get; set; }
        public DateTime DtUpdate { get; set; }
        public DateTime? DtExp { get; set; }
        public Location Location { get; set; }
        public double? MaxGetinMeters { get; set; }

        public Desired()
        {
            Donation = new Donation();
            User = new User();
            Location = new Location();
        }

        public override bool IsValid()
        {
            RuleFor(c => c.DtUpdate)
                .GreaterThan(DateTime.Now)
                .WithMessage("Desired.DtUpdate.GreaterThanNow");

            if (DtExp != null)
            {
                RuleFor(c => c.DtExp)
                    .GreaterThan(DateTime.Now)
                    .WithMessage("Desired.DtExp.GreaterThanNow");
            }

            if (MaxGetinMeters != null)
            {
                RuleFor(c => c.MaxGetinMeters)
                    .GreaterThan(0).WithMessage("Desired.MaxGetinMeters.GreaterThanZero");
            }

            RuleFor(c => c.Active)
                .Must(active => Enum.IsDefined(typeof(Actives), active) == true)
                .WithMessage("Desired.Active.MemberOfActivesEnum");

            if (!Donation.IsValid())
                AddOtherErrorList(Donation.ValidationResult.Errors);

            if (!User.IsValid())
                AddOtherErrorList(User.ValidationResult.Errors);

            if (!Location.IsValid())
                AddOtherErrorList(Location.ValidationResult.Errors);

            return ValidationResult.IsValid;
        }
    }
}
