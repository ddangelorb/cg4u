using System;
using CG4U.Core.Common.Domain.Models;
using CG4U.Donate.Domain.Common;

namespace CG4U.Donate.Domain.Donations.Models
{
    public class DesiredModel : EntityModel<DesiredModel>
    {
        public int IdDonationsDesired { get; set; }
        public Donation Donation { get; set; }
        public User User { get; set; }
        public DateTime DtUpdate { get; set; }
        public DateTime? DtExp { get; set; }
        public Location Location { get; set; }
        public double? MaxGetinMeters { get; set; }
    }
}
