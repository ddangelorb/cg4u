using System;
namespace CG4U.Security.WebAPI.IntegrationTest.DTO
{
    public class RootUser
    {
        public int id { get; set; }
        public string idUserIdentity { get; set; }
        public string firstName { get; set; }
        public string surName { get; set; }
        public string email { get; set; }
        public int idSystem { get; set; }
        public int idLanguage { get; set; }
        public RootClaim[] claims { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            var objComp = obj as RootUser;

            return id == objComp.id
                && idUserIdentity.Equals(objComp.idUserIdentity);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (21 * 397) ^ DateTime.Now.GetHashCode();
            }
        }
    }
}
