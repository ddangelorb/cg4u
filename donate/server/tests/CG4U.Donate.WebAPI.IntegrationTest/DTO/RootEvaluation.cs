using System;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.WebAPI.IntegrationTest.DTO
{
    public class RootEvaluation : EntityModel<RootEvaluation>
    {
        public int idEvaluation { get; set; }
        public int idParent { get; set; }
        public int userGetGrade { get; set; }
        public int userLetGrade { get; set; }
        public string commentsUserGet { get; set; }
        public string commentsUserLet { get; set; }
        public DateTime dtEvaluationGet { get; set; }
        public DateTime dtEvaluationLet { get; set; }
        public int activeGet { get; set; }
        public int activeLet { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            var objComp = obj as RootEvaluation;

            return idEvaluation == objComp.idEvaluation
                && idParent == objComp.idParent
                && userGetGrade == objComp.userGetGrade
                && userLetGrade == objComp.userLetGrade
                && commentsUserGet.ToLower().Equals(objComp.commentsUserGet.ToLower())
                && commentsUserLet.ToLower().Equals(objComp.commentsUserLet.ToLower())
                && activeGet == objComp.activeGet
                && activeLet == objComp.activeLet;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (23 * 397) ^ DateTime.Now.GetHashCode();
            }
        }
    }
}
