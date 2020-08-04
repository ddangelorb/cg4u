using System;
using CG4U.Core.Common.Domain.Models;
using FluentValidation;

namespace CG4U.Security.Domain.Configuration
{
    public class AnalyzeRequest : Entity<AnalyzeRequest>
    {
        public int IdBillableProcesses { get; set; }
        public int IdLanguages { get; set; }
        public int AnalyzeOrder { get; set; }
        public int TypeCode { get; set; }
        public string TypeName { get; set; }
        public int IdAnalyzesRequestsVideoCameras { get; set; }
        public string Location { get; set; }
        public string SubscriptionKey { get; set; }

        public override bool IsValid()
        {
            RuleFor(c => c.IdBillableProcesses)
                .GreaterThan(0).WithMessage("AnalyzeRequest.IdBillableProcesses.GreaterThanZero");

            RuleFor(c => c.IdLanguages)
                .GreaterThan(0).WithMessage("AnalyzeRequest.IdLanguages.GreaterThanZero");

            RuleFor(c => c.AnalyzeOrder)
                .GreaterThan(0).WithMessage("AnalyzeRequest.AnalyzeOrder.GreaterThanZero");

            RuleFor(c => c.TypeCode)
                .GreaterThan(0)
                .WithMessage("AnalyzeRequest.TypeCode.GreaterThanZero")
                .Must(typeCode => Enum.IsDefined(typeof(RequestType), typeCode) == true)
                .WithMessage("AnalyzeRequest.TypeCode.MemberOfRequestTypeEnum");

            RuleFor(c => c.TypeName)
                .NotEmpty().WithMessage("AnalyzeRequest.TypeName.NotEmpty")
                .Length(1, 50).WithMessage("AnalyzeRequest.TypeName.LengthBetween1And50");

            return ValidationResult.IsValid;
        }
    }
}
