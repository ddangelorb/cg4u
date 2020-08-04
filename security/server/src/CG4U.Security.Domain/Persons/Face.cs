using System;
using CG4U.Core.Common.Domain.Models;
using FluentValidation;

namespace CG4U.Security.Domain.Persons
{
    public class Face : Entity<Face>
    {
        public int IdPersons { get; set; }
        public Guid FaceId { get; set; }
        public byte[] Image { get; set; }

        public override bool IsValid()
        {
            RuleFor(c => c.IdPersons)
                .GreaterThan(0).WithMessage("Face.IdPersons.GreaterThanZero");

            RuleFor(c => c.FaceId)
                .NotEmpty().WithMessage("Face.FaceId.NotEmpty");

            return ValidationResult.IsValid;
        }
    }
}
