using CG4U.Core.Common.Domain.Models;

namespace CG4U.Donate.Domain.Trades.Models
{
    public class TradeLocationModel : EntityModel<TradeLocationModel>
    {
        public int IdLocation { get; set; }
        public int IdParent { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
