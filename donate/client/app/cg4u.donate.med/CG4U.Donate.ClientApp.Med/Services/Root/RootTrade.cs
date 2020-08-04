using System;
using System.Collections.Generic;

namespace CG4U.Donate.ClientApp.Med.Services.Root
{
    public class RootTrade : RootEntity<RootTrade>
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
    }
}
