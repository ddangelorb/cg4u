using System;
using System.Net.Http;
using System.Threading.Tasks;
using CG4U.Security.ClientApp.Services.Interfaces;
using CG4U.Security.ClientApp.Services.Roots;

namespace CG4U.Security.ClientApp.Services
{
    public class PersonService : IPersonService
    {
        HttpClient client;

        public PersonService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"http://localhost:5001/");
        }

        public Task<RootPerson> GetByIdUsers(int idUsers)
        {
            throw new NotImplementedException();
        }
    }
}
