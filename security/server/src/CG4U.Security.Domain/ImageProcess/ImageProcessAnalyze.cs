using System;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using FluentValidation;

namespace CG4U.Security.Domain.ImageProcess
{
    public class ImageProcessAnalyze : Entity<ImageProcessAnalyze>
    {
        public Guid IdReference { get; set; }
        public int IdImageProcesses { get; set; }
        public int IdAnalyzesRequestsVideoCameras { get; set; }
        public DateTime DtAnalyze { get; set; }
        public string ReturnResponseType { get; set; }
        public string ReturnResponse { get; set; }
        public int Commited { get; set; }

        public override bool IsValid()
        {
            RuleFor(c => c.IdReference)
                .NotEmpty().WithMessage("ImageProcessAnalyze.IdReference.NotEmpty");
            
            RuleFor(c => c.IdImageProcesses)
                .GreaterThan(0).WithMessage("ImageProcessAnalyze.IdImageProcesses.GreaterThanZero");
            
            RuleFor(c => c.IdAnalyzesRequestsVideoCameras)
                .GreaterThan(0).WithMessage("ImageProcessAnalyze.IdAnalyzesRequestsVideoCameras.GreaterThanZero");

            RuleFor(c => c.DtAnalyze)
                .GreaterThan(DateTime.Now)
                .WithMessage("ImageProcessAnalyze.DtAnalyze.GreaterThanNow");

            RuleFor(c => c.ReturnResponseType)
                .NotEmpty().WithMessage("ImageProcessAnalyze.ReturnResponseType.NotEmpty")
                .Equals("JSON");

            RuleFor(c => c.Commited)
                .Must(active => Enum.IsDefined(typeof(Commits), active) == true)
                .WithMessage("ImageProcessAnalyze.Commited.MemberOfCommitsEnum");

            RuleFor(c => c.Active)
                .Must(active => Enum.IsDefined(typeof(Actives), active) == true)
                .WithMessage("ImageProcessAnalyze.Active.MemberOfActivesEnum");

            return ValidationResult.IsValid;
        }
    }
}
