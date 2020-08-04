using System;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.WebAPI.IntegrationTest.DTO
{
    public class RootDesired : EntityModel<RootDesired>
    {
        public int idDonationsDesired { get; set; }
        public RootDonation donation { get; set; }
        public RootUser user { get; set; }
        public DateTime dtUpdate { get; set; }
        public object dtExp { get; set; }
        public RootLocation location { get; set; }
        public double maxGetinMeters { get; set; }       

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            var objComp = obj as RootDesired;

            return idDonationsDesired == objComp.idDonationsDesired
                && donation.Equals(objComp.donation)
                && user.Equals(objComp.user)
                && location.Equals(objComp.location)
                && Math.Abs(maxGetinMeters - objComp.maxGetinMeters) < 0.0000001;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (20 * 397) ^ DateTime.Now.GetHashCode();
            }
        }
    }
}
