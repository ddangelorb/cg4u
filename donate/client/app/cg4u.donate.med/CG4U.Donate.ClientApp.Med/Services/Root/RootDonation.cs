using System.Collections.Generic;

namespace CG4U.Donate.ClientApp.Med.Services.Root
{
    public class RootDonation : RootEntity<RootDonation>
    {
        public int idDonations { get; set; }
        public int idSystems { get; set; }
        public int? idDonationsDad { get; set; }
        public byte[] img { get; set; }
        public List<RootDonationName> names { get; set; }
    }
}
