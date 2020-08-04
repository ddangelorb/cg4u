using System;
using CG4U.Core.Common.Domain.Messages;
using Microsoft.Extensions.Logging;

namespace CG4U.Donate.Domain.Donations.Events
{
    public class DesiredDisabledEvent : Event
    {
        public int IdDonationsDesired { get; set; }

        public DesiredDisabledEvent(ILogger logger, int idDonationsDesired)
            : base(logger)
        {
            IdDonationsDesired = idDonationsDesired;
        }

        public override void NotifyEventHandled()
        {
            Logger.LogInformation(
                string.Format(
                    "{0} __ Donation.Desired Disabled successfully. DonationDesired.Id {1}",
                    DateTime.Now.ToString("G"), IdDonationsDesired
                )
            );
        }
    }
}
