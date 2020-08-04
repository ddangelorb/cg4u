using System;
namespace CG4U.Donate.ClientApp.Med.Services.Root
{
    public class RootGiven : RootEntity<RootGiven>
    {
        public int idDonationsGivens { get; set; }
        public RootDonation donation { get; set; }
        public RootUser user { get; set; }
        public DateTime dtUpdate { get; set; }
        public object dtExp { get; set; }
        public string img { get; set; }
        public RootLocation location { get; set; }
        public double maxLetinMeters { get; set; }       
    }
}
