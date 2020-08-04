using System;
using FluentValidation;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using System.Collections.Generic;

namespace CG4U.Security.Domain.ImageProcess
{
    public class ImageProcess : Entity<ImageProcess>
    {
        public int IdVideoCameras { get; set; }
        public Guid IdReference { get; set; }
        public byte[] ImageFile { get; set; }
        public string ImageName { get; set; }
        public string IpUserRequest { get; set; }
        public string VideoPath { get; set; }
        public int SecondsToStart { get; set; }
        public DateTime DtProcess { get; set; }
        public ICollection<ImageProcessAnalyze> Analyzes { get; set; }

        public ImageProcess()
        {
            Analyzes = new List<ImageProcessAnalyze>();
        }

        public void AddAnalyze(ImageProcessAnalyze analyze)
        {
            Analyzes.Add(analyze);
        }

        public override bool IsValid()
        {
            RuleFor(c => c.IdVideoCameras)
                .GreaterThan(0).WithMessage("ImageProcess.IdVideoCameras.GreaterThanZero");

            RuleFor(c => c.IdReference)
                .NotEmpty().WithMessage("ImageProcess.IdReference.NotEmpty");

            RuleFor(c => c.ImageName)
                .NotEmpty().WithMessage("ImageProcess.ImageName.NotEmpty")
                .Length(2, 100).WithMessage("ImageProcess.ImageName.LengthBetween2And100");

            RuleFor(c => c.IpUserRequest)
                .NotEmpty().WithMessage("ImageProcess.IpUserRequest.NotEmpty")
                .Length(5, 255).WithMessage("ImageProcess.IpUserRequest.LengthBetween5And255");

            RuleFor(c => c.VideoPath)
                .NotEmpty().WithMessage("ImageProcess.VideoPath.NotEmpty")
                .Length(5, 255).WithMessage("ImageProcess.VideoPath.LengthBetween5And255");

            RuleFor(c => c.SecondsToStart)
                .GreaterThan(0).WithMessage("ImageProcess.SecondsToStart.GreaterThanZero");

            RuleFor(c => c.DtProcess)
                .GreaterThan(DateTime.Now)
                .WithMessage("ImageProcess.DtProcess.GreaterThanNow");

            RuleFor(c => c.Active)
                .Must(active => Enum.IsDefined(typeof(Actives), active) == true)
                .WithMessage("ImageProcess.Active.MemberOfActivesEnum");

            ValidateAnalyzes();

            return ValidationResult.IsValid;
        }

        private void ValidateAnalyzes()
        {
            foreach (var a in Analyzes)
            {
                if (!a.IsValid())
                    AddOtherErrorList(a.ValidationResult.Errors);
            }
        }
    }
}
