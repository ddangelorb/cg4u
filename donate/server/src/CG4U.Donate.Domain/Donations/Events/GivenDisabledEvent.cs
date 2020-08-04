using System;
using CG4U.Core.Common.Domain.Messages;
using Microsoft.Extensions.Logging;

namespace CG4U.Donate.Domain.Donations.Events
{
    public class GivenDisabledEvent : Event
    {
        public int IdDonationsGivens { get; set; }

        public GivenDisabledEvent(ILogger logger, int idDonationsGivens)
            : base(logger)
        {
            IdDonationsGivens = idDonationsGivens;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ Donation.Given Disabled successfully. DonationGiven.Id {1}",
                    DateTime.Now.ToString("G"), IdDonationsGivens
                )
            );
        }
    }
}
