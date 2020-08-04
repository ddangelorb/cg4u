using System.Collections.Generic;
using CG4U.Core.Common.Domain.Models;
using FluentValidation;

namespace CG4U.Security.Domain.Configuration
{
    public class VideoCamera : Entity<VideoCamera>
    {
        public int IdPersonGroups { get; set; }
        public string IdPersonGroupsAPI { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<AnalyzeRequest> AnalyzesRequests { get; set; }

        public VideoCamera()
        {
            AnalyzesRequests = new List<AnalyzeRequest>();
        }

        public void AddAnalyzesRequest(AnalyzeRequest analyzeRequest)
        {
            AnalyzesRequests.Add(analyzeRequest);
        }

        public override bool IsValid()
        {
            RuleFor(c => c.IdPersonGroups)
                .GreaterThan(0).WithMessage("VideoCamera.IdPersonGroups.GreaterThanZero");

            RuleFor(c => c.IdPersonGroupsAPI)
                .NotEmpty().WithMessage("VideoCamera.IdPersonGroupsAPI.NotEmpty");

            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("VideoCamera.Name.NotEmpty")
                .Length(1, 50).WithMessage("VideoCamera.Name.LengthBetween1And50");

            RuleFor(c => c.Description)
                .NotEmpty().WithMessage("VideoCamera.Description.NotEmpty")
                .Length(2, 100).WithMessage("VideoCamera.Description.LengthBetween2And100");

            ValidateAnalyzesRequests();

            return ValidationResult.IsValid;
        }

        private void ValidateAnalyzesRequests()
        {
            foreach (var ar in AnalyzesRequests)
            {
                if (!ar.IsValid())
                    AddOtherErrorList(ar.ValidationResult.Errors);
            }
        }
    }

}
