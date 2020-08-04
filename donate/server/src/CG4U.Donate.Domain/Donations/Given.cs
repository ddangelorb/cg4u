using System;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using CG4U.Donate.Domain.Common;
using FluentValidation;

namespace CG4U.Donate.Domain.Donations
{
    public class Given : Entity<Given>
    {
        public int IdDonationsGivens { get; set; }
        public Donation Donation { get; set; }
        public User User { get; set; }
        public DateTime DtUpdate { get; set; }
        public DateTime? DtExp { get; set; }
        public byte[] Img { get; set; }
        public Location Location { get; set; }
        public double? MaxLetinMeters { get; set; }

        public Given()
        {
            Donation = new Donation();
            User = new User();
            Location = new Location();
        }

        public override bool IsValid()
        {
            RuleFor(c => c.DtUpdate)
                .GreaterThan(DateTime.Now)
                .WithMessage("Given.DtUpdate.GreaterThanNow");

            if (DtExp != null) 
            {
                RuleFor(c => c.DtExp)
                    .GreaterThan(DateTime.Now)
                    .WithMessage("Given.DtExp.GreaterThanNow");
            }

            if (MaxLetinMeters != null)
            {
                RuleFor(c => c.MaxLetinMeters)
                    .GreaterThan(0).WithMessage("Given.MaxLetinMeters.GreaterThanZero");
            }

            RuleFor(c => c.Active)
                .Must(active => Enum.IsDefined(typeof(Actives), active) == true)
                .WithMessage("Given.Active.MemberOfActivesEnum");

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
