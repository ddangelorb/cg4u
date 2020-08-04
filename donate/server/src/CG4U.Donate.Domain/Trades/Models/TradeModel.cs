using System;
using System.Collections.Generic;
using CG4U.Core.Common.Domain.Models;
using CG4U.Donate.Domain.Common;
using CG4U.Donate.Domain.Donations;

namespace CG4U.Donate.Domain.Trades.Models
{
    public class TradeModel : EntityModel<TradeModel>
    {
        public int IdTrades { get; set; }
        public User UserGet { get; set; }
        public User UserLet { get; set; }
        public Given Given { get; set; }
        public Desired Desired { get; set; }
        public DateTime DtTrade { get; set; }
        public int Commited { get; set; }
        public ICollection<Location> Locations { get; private set; }
        public ICollection<Evaluation> Evaluations { get; private set; }

        public TradeModel()
        {
            Locations = new List<Location>();
            Evaluations = new List<Evaluation>();
        }
    }
}
