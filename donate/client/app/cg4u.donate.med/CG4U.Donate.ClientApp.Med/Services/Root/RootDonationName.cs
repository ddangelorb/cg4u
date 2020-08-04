namespace CG4U.Donate.ClientApp.Med.Services.Root
{
    public class RootDonationName : RootEntity<RootDonationName>
    {
        public int idDonationsNames { get; set; }
        public int idDonations { get; set; }
        public int idLanguages { get; set; }
        public string name { get; set; }
    }
}
