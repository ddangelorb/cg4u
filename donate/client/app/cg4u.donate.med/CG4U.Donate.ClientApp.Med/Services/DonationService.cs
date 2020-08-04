using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CG4U.Donate.ClientApp.Med.Services.Root;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace CG4U.Donate.ClientApp.Med.Services
{
    public class DonationService : Interfaces.IDonationService
    {
        HttpClient client;

        public DonationService()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri($"http://localhost:5002/");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", (string)Application.Current.Properties["token"]);
        }

        public async Task<List<RootDonation>> ListDonationsByLanguageAndNameAsync(string query)
        {
            var response = await client.GetAsync(string.Format("Donation/ListDonationsByLanguageAndNameAsync/{0}/{1}/{2}", query, App.IdSystems, App.IdLanguages));
            return JsonConvert.DeserializeObject<List<RootDonation>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> AddDesiredAsync(RootDesired desired)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(desired), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("Donation/AddDesiredAsync", postContent);
            
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddGivenAsync(RootGiven given)
        {
            var postContent = new StringContent(JsonConvert.SerializeObject(given), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("Donation/AddGivenAsync", postContent);

            return response.IsSuccessStatusCode;
        }
    }
}
