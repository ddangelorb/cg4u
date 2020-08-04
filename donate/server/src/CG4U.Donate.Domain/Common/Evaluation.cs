using System;
using CG4U.Core.Common.Domain.Interfaces;
using CG4U.Core.Common.Domain.Models;
using FluentValidation;

namespace CG4U.Donate.Domain.Common
{
    public class Evaluation : Entity<Evaluation>, IDirtyable
    {
        public int IdEvaluation { get; set; } 
        public int IdParent { get; set; }
        public int? UserGetGrade { get; private set; }
        public int? UserLetGrade { get; private set; }
        public string CommentsUserGet { get; private set; }
        public string CommentsUserLet { get; private set; }
        public DateTime? DtEvaluationGet { get; private set; }
        public DateTime? DtEvaluationLet { get; private set; }
        public int? ActiveGet { get; private set; }
        public int? ActiveLet { get; private set; }

        public bool IsDirty()
        {
            return (UserGetGrade != null && UserGetGrade > 0) ||
                (UserLetGrade != null && UserLetGrade > 0) ||
                (CommentsUserGet != null && CommentsUserGet.Length > 0) ||
                (CommentsUserLet != null && CommentsUserLet.Length > 0) ||
                (DtEvaluationGet != null) ||
                (DtEvaluationLet != null) ||
                (ActiveGet != null && ActiveGet > 0) ||
                (ActiveLet != null && ActiveLet > 0);
        }

        public override bool IsValid()
        {
            if (IsDirty())
            {
                RuleFor(c => c.IdParent)
                    .GreaterThan(0).WithMessage("Evaluation.IdParent.GreaterThanZero");

                if (UserGetGrade != null && UserGetGrade > 0)
                    RuleFor(c => c.UserGetGrade)
                        .Must(active => Enum.IsDefined(typeof(Grades), active) == true)
                        .WithMessage("Evaluation.UserGetGrade.MemberOfGradesEnum");

                if (UserLetGrade != null && UserLetGrade > 0)
                    RuleFor(c => c.UserLetGrade)
                        .Must(active => Enum.IsDefined(typeof(Grades), active) == true)
                        .WithMessage("Evaluation.UserLetGrade.MemberOfGradesEnum");

                if (CommentsUserGet != null && CommentsUserGet.Length > 0)
                    RuleFor(c => c.CommentsUserGet)
                        .Length(2, 25).WithMessage("Evaluation.CommentsUserGet.LengthBetween2And25")
                        .Must(c => !AvoidingWords.Words.Contains(c.ToUpper())).WithMessage("Evaluation.CommentsUserGet.AvoidingWords");

                if (CommentsUserLet != null && CommentsUserLet.Length > 0)
                    RuleFor(c => c.CommentsUserLet)
                        .Length(2, 25).WithMessage("Evaluation.CommentsUserLet.LengthBetween2And25")
                        .Must(c => !AvoidingWords.Words.Contains(c.ToUpper())).WithMessage("Evaluation.CommentsUserLet.AvoidingWords");

                if (DtEvaluationGet != null)
                    RuleFor(c => c.DtEvaluationGet)
                        .GreaterThan(DateTime.Now)
                        .WithMessage("Evaluation.DtEvaluationGet.GreaterThanNow");

                if (DtEvaluationLet != null)
                    RuleFor(c => c.DtEvaluationLet)
                        .GreaterThan(DateTime.Now)
                        .WithMessage("Evaluation.DtEvaluationLet.GreaterThanNow");
                
                if (ActiveGet != null && ActiveGet > 0)
                    RuleFor(c => c.ActiveGet)
                        .Must(active => Enum.IsDefined(typeof(Actives), active) == true)
                        .WithMessage("Evaluation.ActiveGet.MemberOfActivesEnum");
                    
                if (ActiveLet != null && ActiveLet > 0)
                    RuleFor(c => c.ActiveLet)
                        .Must(active => Enum.IsDefined(typeof(Actives), active) == true)
                        .WithMessage("Evaluation.ActiveLet.MemberOfActivesEnum");
            }

            return ValidationResult.IsValid;
        }
    }
}
