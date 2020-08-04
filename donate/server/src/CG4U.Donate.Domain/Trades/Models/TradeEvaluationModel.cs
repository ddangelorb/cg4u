using System;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.Domain.Trades.Models
{
    public class TradeEvaluationModel : EntityModel<TradeEvaluationModel>
    {
        public int IdEvaluation { get; set; }
        public int IdParent { get; set; }
        public int? UserGetGrade { get; set; }
        public int? UserLetGrade { get; set; }
        public string CommentsUserGet { get; set; }
        public string CommentsUserLet { get; set; }
        public DateTime? DtEvaluationGet { get; set; }
        public DateTime? DtEvaluationLet { get; set; }
        public int? ActiveGet { get; set; }
        public int? ActiveLet { get; set; }
    }
}
