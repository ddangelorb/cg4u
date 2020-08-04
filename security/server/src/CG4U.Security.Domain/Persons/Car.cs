using CG4U.Core.Common.Domain.Models;
using FluentValidation;

namespace CG4U.Security.Domain.Persons
{
    public class Car : Entity<Car>
    {
        public int IdPersons { get; set; }
        public string PlateCode { get; set; }

        public override bool IsValid()
        {
            RuleFor(c => c.IdPersons)
                .GreaterThan(0).WithMessage("Car.IdPersons.GreaterThanZero");

            RuleFor(c => c.PlateCode)
                .NotEmpty().WithMessage("Car.PlateCode.NotEmpty")
                .Length(2, 15).WithMessage("Car.PlateCode.LengthBetween2And15");

            return ValidationResult.IsValid;
        }
    }
}
