using System;
using System.Collections.Generic;
using CG4U.Core.Common.Domain.Models;
using FluentValidation;

namespace CG4U.Security.Domain.Persons
{
    public class Person : Entity<Person>
    {
        public PersonGroup PersonGroup { get; set; }
        public Guid IdApi { get; set; }
        public int IdUsers { get; set; }
        public string Name { get; set; }
        public ICollection<Car> Cars { get; set; }
        public ICollection<Face> Faces { get; set; }

        public Person()
        {
            PersonGroup = new PersonGroup();
            Cars = new List<Car>();
            Faces = new List<Face>();
        }

        public void AddCar(Car car)
        {
            Cars.Add(car);
        }

        public void AddFace(Face face)
        {
            Faces.Add(face);
        }

        public override bool IsValid()
        {
            if (!PersonGroup.IsValid())
                AddOtherErrorList(PersonGroup.ValidationResult.Errors);

            RuleFor(c => c.IdApi)
                .NotEmpty().WithMessage("Person.IdApi.NotEmpty");

            RuleFor(c => c.IdUsers)
                .GreaterThan(0).WithMessage("Person.IdUser.GreaterThanZero");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Person.Name.NotEmpty")
                .Length(2, 255).WithMessage("Person.Name.LengthBetween2And255");

            ValidateCars();
            ValidateFaces();

            return ValidationResult.IsValid;
        }

        private void ValidateCars()
        {
            foreach (var c in Cars)
            {
                if (!c.IsValid())
                    AddOtherErrorList(c.ValidationResult.Errors);
            }
        }

        private void ValidateFaces()
        {
            foreach (var f in Faces)
            {
                if (!f.IsValid())
                    AddOtherErrorList(f.ValidationResult.Errors);
            }
        }

    }
}
