using System;
using System.ComponentModel.DataAnnotations;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.WebAPI.ViewModels
{
    public class EvaluationViewModel : EntityModel<EvaluationViewModel>
    {
        [Display(Name = "Evaluation.IdEvaluation.DisplayName")]
        public int IdEvaluation { get; set; }

        [Display(Name = "Evaluation.IdParent.DisplayName")]
        [Required(ErrorMessage = "Evaluation.IdParent.NotEmpty")]
        [Range(1, int.MaxValue, ErrorMessage = "Evaluation.IdParent.GreaterThanZero")]
        public int IdParent { get; set; }

        [Display(Name = "Evaluation.UserGetGrade.DisplayName")]
        [Range(0, 5, ErrorMessage = "Evaluation.UserGetGrade.MustBetween0And5")]
        public int? UserGetGrade { get; set; }

        [Display(Name = "Evaluation.UserLetGrade.DisplayName")]
        [Range(0, 5, ErrorMessage = "Evaluation.UserLetGrade.MustBetween0And5")]
        public int? UserLetGrade { get; set; }

        [Display(Name = "Evaluation.CommentsUserGet.DisplayName")]
        [Required(ErrorMessage = "Evaluation.CommentsUserGet.NotEmpty")]
        [MinLength(2, ErrorMessage = "Evaluation.CommentsUserGet.LengthMin2")]
        [MaxLength(25, ErrorMessage = "Evaluation.CommentsUserGet.LengthMax25")]
        public string CommentsUserGet { get; set; }

        [Display(Name = "Evaluation.CommentsUserLet.DisplayName")]
        [Required(ErrorMessage = "Evaluation.CommentsUserLet.NotEmpty")]
        [MinLength(2, ErrorMessage = "Evaluation.CommentsUserLet.LengthMin2")]
        [MaxLength(25, ErrorMessage = "Evaluation.CommentsUserLet.LengthMax25")]
        public string CommentsUserLet { get; set; }

        [Display(Name = "Evaluation.DtEvaluationGet.DisplayName")]
        public DateTime? DtEvaluationGet { get; set; }

        [Display(Name = "Evaluation.DtEvaluationLet.DisplayName")]
        public DateTime? DtEvaluationLet { get; set; }

        [Display(Name = "Evaluation.ActiveGet.DisplayName")]
        [Range(0, 1, ErrorMessage = "Evaluation.ActiveGet.MustBe0Or1")]
        public int? ActiveGet { get; set; }

        [Display(Name = "Evaluation.ActiveLet.DisplayName")]
        [Range(0, 1, ErrorMessage = "Evaluation.ActiveLet.MustBe0Or1")]
        public int? ActiveLet { get; set; }
    }
}
