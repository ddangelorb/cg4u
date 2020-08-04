using System;
namespace CG4U.Donate.ClientApp.Med.Services.Root
{
    public class RootEvaluation : RootEntity<RootEvaluation>
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
    }
}
