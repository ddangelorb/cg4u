using System;
using CG4U.Core.Common.Domain.Models;
using FluentValidation;

namespace CG4U.Security.Domain.Configuration
{
    public class Alert : Entity<Alert>
    {
        public int IdAnalyzesRequests { get; set; }
        public int TypeCode { get; set; }
        public string Message { get; set; }
        public string ProcessingMethod { get; set; }
        public string ProcessingParam { get; set; }

        public override bool IsValid()
        {
            RuleFor(c => c.IdAnalyzesRequests)
                .GreaterThan(0).WithMessage("Alert.IdAnalyzesRequests.GreaterThanZero");

            RuleFor(c => c.TypeCode)
                .GreaterThan(0)
                .WithMessage("Alert.TypeCode.GreaterThanZero")
                .Must(typeCode => Enum.IsDefined(typeof(AlertType), typeCode) == true)
                .WithMessage("Alert.TypeCode.MemberOfAlertTypeEnum");

            RuleFor(c => c.Message)
                .NotEmpty().WithMessage("Alert.Message.NotEmpty")
                .Length(5, 255).WithMessage("Alert.Message.LengthBetween5And255");

            RuleFor(c => c.ProcessingMethod)
                .NotEmpty().WithMessage("Alert.ProcessingMethod.NotEmpty")
                .Length(5, 50).WithMessage("Alert.ProcessingMethod.LengthBetween5And50");

            return ValidationResult.IsValid;
        }
    }
}
