using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using FluentValidation;

namespace CG4U.Donate.Domain.Common
{
    public class Location : Entity<Location>, IDirtyable
    {
        public int IdLocation { get; set; } 
        public int IdParent { get; set; }
        public string Address { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }
        public decimal Latitude { get; private set; }
        public decimal Longitude { get; private set; }

        public bool IsDirty()
        {
            return (Address != null && Address.Length > 0) ||
                (City != null && City.Length > 0) ||
                (State != null && State.Length > 0) ||
                (Latitude > 0) ||
                (Longitude > 0);
        }

        public override bool IsValid()
        {
            if (IsDirty())
            {
                RuleFor(c => c.IdLocation)
                    .GreaterThan(0).WithMessage("Location.IdLocation.GreaterThanZero");

                RuleFor(c => c.Address)
                    .NotEmpty().WithMessage("Location.Address.NotEmpty")
                    .Length(2, 255).WithMessage("Location.Address.LengthBetween2And255");

                RuleFor(c => c.City)
                    .NotEmpty().WithMessage("Location.City.NotEmpty")
                    .Length(2, 50).WithMessage("Location.City.LengthBetween2And50");

                RuleFor(c => c.State)
                    .NotEmpty().WithMessage("Location.State.NotEmpty")
                    .Length(2, 50).WithMessage("Location.State.LengthBetween2And50");

                RuleFor(c => c.ZipCode)
                    .NotEmpty().WithMessage("Location.ZipCode.NotEmpty")
                    .Length(2, 20).WithMessage("Location.ZipCode.LengthBetween2And20");
            }

            return ValidationResult.IsValid;
        }
    }
}
