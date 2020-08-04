using System;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.WebAPI.IntegrationTest.DTO
{
    public class RootGiven : EntityModel<RootGiven>
    {
        public int idDonationsGivens { get; set; }
        public RootDonation donation { get; set; }
        public RootUser user { get; set; }
        public DateTime dtUpdate { get; set; }
        public object dtExp { get; set; }
        public string img { get; set; }
        public RootLocation location { get; set; }
        public double maxLetinMeters { get; set; }       

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            var objComp = obj as RootGiven;

            return idDonationsGivens == objComp.idDonationsGivens
                && donation.Equals(objComp.donation)
                && user.Equals(objComp.user)
                && location.Equals(objComp.location)
                && Math.Abs(maxLetinMeters - objComp.maxLetinMeters) < 0.0000001;

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
