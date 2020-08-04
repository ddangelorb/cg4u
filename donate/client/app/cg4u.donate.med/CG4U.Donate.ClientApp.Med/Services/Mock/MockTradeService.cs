using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CG4U.Donate.ClientApp.Med.Services.Root;

namespace CG4U.Donate.ClientApp.Med.Services.Mock
{
    public class MockTradeService : Interfaces.ITradeService
    {
        public MockTradeService()
        {
        }

        public Task<List<RootDesired>> ListMatchDesiredsByPositionAsync(int idDonationsGivens, double maxDistanceValue)
        {
            throw new NotImplementedException();
        }

        public Task<List<RootGiven>> ListMatchGivensByPositionAsync(int idDonationsDesired, double maxDistanceValue)
        {
            throw new NotImplementedException();
        }
    }
}
