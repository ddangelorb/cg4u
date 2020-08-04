namespace CG4U.Security.ClientApp.Services.Roots
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
    }
}
