using System;
using System.Collections.Generic;
using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.WebAPI.IntegrationTest.DTO
{
    public class RootTrade : EntityModel<RootTrade>
    {
        public int idTrades { get; set; }
        public RootUser userGet { get; set; }
        public RootUser userLet { get; set; }
        public RootGiven given { get; set; }
        public RootDesired desired { get; set; }
        public DateTime dtTrade { get; set; }
        public int commited { get; set; }
        public List<RootLocation> locations { get; set; }
        public List<object> evaluations { get; set; }

        public RootTrade()
        {
            locations = new List<RootLocation>();
            evaluations = new List<object>();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj == this) return true;
            var objComp = obj as RootTrade;

            return idTrades == objComp.idTrades
                && userGet.Equals(objComp.userGet)
                && userLet.Equals(objComp.userLet)
                && given.idDonationsGivens == objComp.given.idDonationsGivens
                && desired.idDonationsDesired ==  objComp.desired.idDonationsDesired
                && commited == objComp.commited;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (22 * 397) ^ DateTime.Now.GetHashCode();
            }
        }

    }
}
