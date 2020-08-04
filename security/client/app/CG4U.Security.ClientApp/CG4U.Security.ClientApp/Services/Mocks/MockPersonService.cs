using System;
using System.Threading.Tasks;
using CG4U.Security.ClientApp.Services.Interfaces;
using CG4U.Security.ClientApp.Services.Roots;

namespace CG4U.Security.ClientApp.Services.Mocks
{
    public class MockPersonService : IPersonService
    {
        public MockPersonService()
        {
        }

        public Task<RootPerson> GetByIdUsers(int idUsers)
        {
            return Task.FromResult(
                new RootPerson()
                {
                    Id = 1,
                    PersonGroup = new RootPersonGroup()
                    {
                        Id = 1
                    }
                }
            );
        }
    }
}
