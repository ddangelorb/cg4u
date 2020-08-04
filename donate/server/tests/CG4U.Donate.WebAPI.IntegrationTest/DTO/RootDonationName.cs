using System;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.WebAPI.IntegrationTest.DTO
{
    public class RootDonationName : EntityModel<RootDonationName>
    {
        public int idDonationsNames { get; set; }
        public int idDonations { get; set; }
        public int idLanguages { get; set; }
        public string name { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            var objComp = obj as RootDonationName;

            return idDonationsNames == objComp.idDonationsNames
                && idDonations == objComp.idDonations
                && idLanguages == objComp.idLanguages
                && name.Equals(objComp.name, StringComparison.CurrentCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (16 * 397) ^ DateTime.Now.GetHashCode();
            }
        }
    }
}
