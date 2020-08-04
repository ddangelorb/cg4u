using System.IO;
using System.Threading.Tasks;
using CG4U.Donate.WebAPI.IntegrationTest.DTO;
using Newtonsoft.Json;
using Xunit;
using System.Net;
using System.Collections.Generic;
using CG4U.Donate.WebAPI.ViewModels;
using AutoMapper;
using System;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

namespace CG4U.Donate.WebAPI.IntegrationTest
{
    public class DonationControllerIntegrationTest : BaseControllerIntegrationTest
    {
        [Fact(DisplayName = "01 : Get an Unauthorized HttpStatus")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_GetDonationById_Unauthorized()
        {
            //Arrange
            var objComp = listRootDonations[0];
            var Id = 1;
            var IdSystems = 1;
            var IdLanguages = 1;

            //Act
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Donation/GetDonationByIdSystemLanguageAsync/{0}/{1}/{2}", Id, IdSystems, IdLanguages)))
            {
                //Assert
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }

        }

        [Fact(DisplayName = "02 : Get an existed Donation By Id")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_GetDonationById_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var objComp = listRootDonations[0];
            var id = 1;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Donation/GetDonationByIdSystemLanguageAsync/{0}/{1}/{2}", id, login.user.idSystem, login.user.idLanguage)))
            {
                var result = JsonConvert.DeserializeObject<RootDonation>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.IsType<RootDonation>(result);
                Assert.Equal(result, objComp);
            }
        }

        [Fact(DisplayName = "03 : Get a not found Donation By Id")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_GetDonationById_NotFound()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var Id = 4;
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Donation/GetDonationByIdSystemLanguageAsync/{0}/{1}/{2}", Id, IdSystems, IdLanguages)))
            {
                var result = JsonConvert.DeserializeObject<RootDonation>(await response.Content.ReadAsStringAsync());

                //Assert
                Assert.Null(result);
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        [Fact(DisplayName = "04 : Get a list of existed Donations By Language and Name")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_ListDonationsByLanguageAndNameAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var Name = "Analgésicos e Antitérmicos";
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;
            var objComp = listRootDonations[0];

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Donation/ListDonationsByLanguageAndNameAsync/{0}/{1}/{2}", Name, IdSystems, IdLanguages)))
            {
                var result = JsonConvert.DeserializeObject<List<RootDonation>>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.Equal(result.Count, 1);
                Assert.IsType<RootDonation>(result[0]);
                Assert.Equal(result[0], objComp);
            }
        }

        [Fact(DisplayName = "05 : Get a not found Donation By Language and Name")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_GetDonationsByLanguageAndName_NotFound()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var Name = "XPTO";
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Donation/ListDonationsByLanguageAndNameAsync/{0}/{1}/{2}", Name, IdSystems, IdLanguages)))
            {
                var result = JsonConvert.DeserializeObject<List<RootDonation>>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.Equal(result.Count, 0);
            }
        }

        [Fact(DisplayName = "06 : Add a donation desired")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_AddDesired_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);

            //Act
            foreach (var desired in listDesireds)
            {
                using (var postContent = new StringContent(JsonConvert.SerializeObject(desired), Encoding.UTF8, "application/json"))
                using (var response = await Environment.ServerApiDonate
                    .CreateRequest("Donation/AddDesiredAsync")
                    .AddHeader("Authorization", "Bearer " + login.access_token)
                    .And(request => request.Content = postContent)
                    .And(request => request.Method = HttpMethod.Post)
                       .PostAsync())                    
                {
                    //Assert
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        [Fact(DisplayName = "07 : Get an existed Donation Desired By Id")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_GetDesiredById_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var objComp = Mapper.Map<DesiredViewModel, RootDesired>(listDesireds[0]);
            var Id = 1;
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Donation/GetDesiredByIdSystemLanguageAsync/{0}/{1}/{2}", Id, IdSystems, IdLanguages)))
            {
                var result = JsonConvert.DeserializeObject<RootDesired>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.IsType<RootDesired>(result);
                Assert.Equal(result, objComp);
            }
        }

        [Fact(DisplayName = "08 : Get a not found Donation Desired By Id")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_GetDesiredById_NotFound()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var Id = 20;
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Donation/GetDesiredByIdSystemLanguageAsync/{0}/{1}/{2}", Id, IdSystems, IdLanguages)))
            {
                var result = JsonConvert.DeserializeObject<RootDesired>(await response.Content.ReadAsStringAsync());

                //Assert
                Assert.Null(result);
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        [Fact(DisplayName = "09 : List Donation Desireds By Owner successfully")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_ListDesiredsByOwner_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var Id = 2;
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Donation/ListDesiredsByOwnerAsync/{0}/{1}/{2}", Id, IdSystems, IdLanguages)))
            {
                var result = JsonConvert.DeserializeObject<List<RootDesired>>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.IsType<List<RootDesired>>(result);
                Assert.Equal(result.Count, 3);
                Assert.Equal(result[0], Mapper.Map<DesiredViewModel, RootDesired>(listDesireds[0]));
                Assert.Equal(result[1], Mapper.Map<DesiredViewModel, RootDesired>(listDesireds[1]));
            }
        }

        [Fact(DisplayName = "10 : Update a Donation Desireds successfully")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_UpdateDesired_SuccessfullyAsync()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var desired = listDesireds[2];
            desired.DtExp = DateTime.Now.AddDays(2);
            desired.Donation = Mapper.Map<RootDonation, DonationViewModel>(listRootDonations[1]);
            desired.Location = locationBoaVistaShopping;
            desired.MaxGetinMeters = 11500;

            //Act
            using (var postContent = new StringContent(JsonConvert.SerializeObject(desired), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiDonate
                .CreateRequest("Donation/UpdateDesiredAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .And(request => request.Method = HttpMethod.Post)
                 .PostAsync())
            {
                //Assert
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact(DisplayName = "11 : Disable a Donation Desireds successfully")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_DisableDesired_SuccessfullyAsync()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var desired = listDesireds[2];

            //Act
            using (var postContent = new StringContent(desired.Id.ToString(), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiDonate
                .CreateRequest("Donation/DisableDesiredAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .And(request => request.Method = HttpMethod.Post)
                .PostAsync())
            {
                //Assert
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact(DisplayName = "12 : Add a Donation Given successfully")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_AddGiven_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(0);

            //Act
            foreach (var given in listGivens)
            {
                using (var postContent = new StringContent(JsonConvert.SerializeObject(given), Encoding.UTF8, "application/json"))
                using (var response = await Environment.ServerApiDonate
                    .CreateRequest("Donation/AddGivenAsync")
                    .AddHeader("Authorization", "Bearer " + login.access_token)
                    .And(request => request.Content = postContent)
                    .And(request => request.Method = HttpMethod.Post)
                    .PostAsync())
                {
                    //Assert
                    response.EnsureSuccessStatusCode();
                }
            }
        }

        [Fact(DisplayName = "13 : Get a Donation Given ById successfully")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_GetGivenById_SuccessfullyAsync()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);
            var objComp = Mapper.Map<GivenViewModel, RootGiven>(listGivens[0]);
            var Id = 1;
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Donation/GetGivenByIdSystemLanguageAsync/{0}/{1}/{2}", Id, IdSystems, IdLanguages)))
            {
                var result = JsonConvert.DeserializeObject<RootGiven>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.IsType<RootGiven>(result);
                Assert.Equal(result, objComp);
            }
        }

        [Fact(DisplayName = "14 : Get a not found Donation Given ById successfully")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_GetGivenById_NotFoundAsync()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);
            var Id = 20;
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using(var response = await Environment.ClientApiDonate.
                  GetAsync(string.Format("Donation/GetGivenByIdSystemLanguageAsync/{0}/{1}/{2}", Id, IdSystems, IdLanguages)))
            {
                var result = JsonConvert.DeserializeObject<RootGiven>(await response.Content.ReadAsStringAsync());

                //Assert
                Assert.Null(result);
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        [Fact(DisplayName = "15 : List Donation Givens by Owner successfully")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_ListGivensByOwner_SuccessfullyAsync()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);
            var Id = 3;
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Donation/ListGivensByOwnerAsync/{0}/{1}/{2}", Id, IdSystems, IdLanguages)))
            {
                var result = JsonConvert.DeserializeObject<List<RootGiven>>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.IsType<List<RootGiven>>(result);
                Assert.Equal(result.Count, 2);
                Assert.Equal(result[0], Mapper.Map<GivenViewModel, RootGiven>(listGivens[0]));
                Assert.Equal(result[1], Mapper.Map<GivenViewModel, RootGiven>(listGivens[1]));
            }
        }

        [Fact(DisplayName = "16 : Update a Donation Given successfully")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_UpdateGiven_SuccessfullyAsync()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);
            var given = listGivens[0];
            given.DtExp = DateTime.Now.AddDays(2);
            given.Donation = Mapper.Map<RootDonation, DonationViewModel>(listRootDonations[1]);
            given.Location = locationBoaVistaShopping;
            given.MaxLetinMeters = 1500;

            //Act
            using (var postContent = new StringContent(JsonConvert.SerializeObject(given), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiDonate
                .CreateRequest("Donation/UpdateGivenAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .And(request => request.Method = HttpMethod.Post)
                .PostAsync())
            {
                //Assert
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact(DisplayName = "17 : Add an image to a Donation Given ById successfully")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_AddGivenImage_SuccessfullyAsync()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);
            var given = listGivens[0];
            using (var sc = new StreamContent(File.OpenRead("Img/analgesico.jpg")))
            using (var ms = new MemoryStream())
            {
                await sc.CopyToAsync(ms);
                given.Img = ms.ToArray();
            }

            //Act
            using (var postContent = new StringContent(JsonConvert.SerializeObject(given), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiDonate
                .CreateRequest("Donation/AddGivenImageAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .And(request => request.Method = HttpMethod.Post)
                .PostAsync())
            {
                //Assert
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact(DisplayName = "18 : Disable a Donation Given successfully")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_DisableGiven_SuccessfullyAsync()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);
            var given = listGivens[0];

            //Act
            using (var postContent = new StringContent(given.Id.ToString(), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiDonate
                .CreateRequest("Donation/DisableGivenAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .And(request => request.Method = HttpMethod.Post)
                .PostAsync())
            {
                //Assert
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
