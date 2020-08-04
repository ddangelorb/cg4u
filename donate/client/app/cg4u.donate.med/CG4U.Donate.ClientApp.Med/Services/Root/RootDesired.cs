using System;
namespace CG4U.Donate.ClientApp.Med.Services.Root
{
    public class RootDesired : RootEntity<RootDesired>
    {
        public int idDonationsDesired { get; set; }
        public RootDonation donation { get; set; }
        public RootUser user { get; set; }
        public DateTime dtUpdate { get; set; }
        public object dtExp { get; set; }
        public RootLocation location { get; set; }
        public double maxGetinMeters { get; set; }       
    }
}
