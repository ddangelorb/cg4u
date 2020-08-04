using System;
using System.Collections.Generic;
using System.Linq;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.WebAPI.IntegrationTest.DTO
{
    //http://json2csharp.com/    
    public class RootDonation : EntityModel<RootDonation>
    {
        public int idDonations { get; set; }
        public int idSystems { get; set; }
        public object idDonationsDad { get; set; }
        public object img { get; set; }
        public List<RootDonationName> names { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            var objComp = obj as RootDonation;

            return idDonations == objComp.idDonations
                && idSystems == objComp.idSystems
                && idDonationsDad == objComp.idDonationsDad
                && names.SequenceEqual(objComp.names);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (17 * 397) ^ DateTime.Now.GetHashCode();
            }
        }
    }
}
