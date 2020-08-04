using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CG4U.Donate.WebAPI.IntegrationTest.DTO;
using CG4U.Donate.WebAPI.ViewModels;
using Newtonsoft.Json;
using Xunit;

namespace CG4U.Donate.WebAPI.IntegrationTest
{
    public class TradeControllerIntegrationTest : BaseControllerIntegrationTest
    {
        private List<TradeViewModel> listTrades;
        private List<EvaluationViewModel> listEvaluations;

        public TradeControllerIntegrationTest()
        {
            listTrades = new List<TradeViewModel>()
            {
                new TradeViewModel()
                {
                    Id = 1,
                    IdTrades = 1,
                    UserGet = listUsers[2],
                    UserLet = listUsers[1],
                    Given = listGivens[1],
                    Desired = listDesireds[1],
                    DtTrade = DateTime.Now,
                    Commited = 0,
                    Active = 1
                },
                new TradeViewModel()
                {
                    Id = 2,
                    IdTrades = 2,
                    UserGet = listUsers[2],
                    UserLet = listUsers[1],
                    Given = listGivens[2],
                    Desired = listDesireds[1],
                    DtTrade = DateTime.Now,
                    Commited = 0,
                    Active = 1
                },
                new TradeViewModel()
                {
                    Id = 3,
                    IdTrades = 3,
                    UserGet = listUsers[2],
                    UserLet = listUsers[0],
                    Given = listGivens[1],
                    Desired = listDesireds[1],
                    DtTrade = DateTime.Now,
                    Commited = 0,
                    Active = 1
                }
            };

            listEvaluations = new List<EvaluationViewModel>()
            {
                new EvaluationViewModel()
                {
                    Id = 1,
                    IdEvaluation = 1,
                    IdParent = 2,
                    UserGetGrade = 3,
                    UserLetGrade = 4,
                    CommentsUserGet = "Muito legal",
                    CommentsUserLet = "Tudo certo!",
                    DtEvaluationGet = DateTime.Now,
                    DtEvaluationLet = DateTime.Now,
                    ActiveGet = 1,
                    ActiveLet = 1
                },
                new EvaluationViewModel()
                {
                    Id = 2,
                    IdEvaluation = 2,
                    IdParent = 3,
                    UserGetGrade = 1,
                    UserLetGrade = 3,
                    CommentsUserGet = "Não gostei muito",
                    CommentsUserLet = "ok, conforme previsto",
                    DtEvaluationGet = DateTime.Now,
                    DtEvaluationLet = DateTime.Now,
                    ActiveGet = 1,
                    ActiveLet = 1
                }
            };
        }

        [Fact(DisplayName = "01 : Get an Unauthorized HttpStatus")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_GetTradeByIdAsync_Unauthorized()
        {
            //Arrange
            var Id = 1;
            var IdSystems = 1;
            var IdLanguages = 1;

            //Act
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Trade/GetTradeByIdSystemLanguageAsync/{0}/{1}/{2}", Id, IdSystems, IdLanguages)))
            {
                //Assert
                Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
            }
        }


        [Fact(DisplayName = "02 : Get a list of desired matched for Trades")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_ListMatchDesiredsByPositionAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var IdDonationsGivens = 2;
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;
            var Value = 12600;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Trade/ListMatchDesiredsByPositionAsync/{0}/{1}/{2}/{3}", IdDonationsGivens, IdSystems, IdLanguages, Value)))
            {
                var result = JsonConvert.DeserializeObject<List<RootDesired>>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.IsType<List<RootDesired>>(result);
                Assert.Equal(result.Count, 1);
                Assert.Equal(result[0], Mapper.Map<DesiredViewModel, RootDesired>(listDesireds[1]));
            }
        }

        [Fact(DisplayName = "03 : Get a list of given matched for Trades")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_ListMatchGivensByPositionAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var IdDonationsDesired = 2;
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;
            var Value = 12600;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Trade/ListMatchGivensByPositionAsync/{0}/{1}/{2}/{3}", IdDonationsDesired, IdSystems, IdLanguages, Value)))
            {
                var result = JsonConvert.DeserializeObject<List<RootGiven>>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.IsType<List<RootGiven>>(result);
                Assert.Equal(result.Count, 2);
                Assert.Equal(result[0], Mapper.Map<GivenViewModel, RootGiven>(listGivens[1]));
                Assert.Equal(result[1], Mapper.Map<GivenViewModel, RootGiven>(listGivens[2]));
            }
        }

        [Fact(DisplayName = "04 : Add Trades")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_AddTradeAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);

            //Act
            foreach (var trade in listTrades)
            {
                using (var postContent = new StringContent(JsonConvert.SerializeObject(trade), Encoding.UTF8, "application/json"))
                using (var response = await Environment.ServerApiDonate
                    .CreateRequest("Trade/AddTradeAsync")
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

        [Fact(DisplayName = "05 : Get an existed Trade By Id, System, Language")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_GetTradeByIdSystemLanguageAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var objComp = Mapper.Map<TradeViewModel, RootTrade>(listTrades[0]);
            var Id = 1;
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Trade/GetTradeByIdSystemLanguageAsync/{0}/{1}/{2}", Id, IdSystems, IdLanguages)))
            {
                var result = JsonConvert.DeserializeObject<RootTrade>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.IsType<RootTrade>(result);
                Assert.Equal(result, objComp);
            }
        }

        [Fact(DisplayName = "06 : Get a not found Trade By Id, System, Language")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_GetTradeByIdSystemLanguageAsync_NotFoundAsync()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var objComp = Mapper.Map<TradeViewModel, RootTrade>(listTrades[0]);
            var Id = 20;
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Trade/GetTradeByIdSystemLanguageAsync/{0}/{1}/{2}", Id, IdSystems, IdLanguages)))
            {
                var result = JsonConvert.DeserializeObject<RootTrade>(await response.Content.ReadAsStringAsync());

                //Assert
                Assert.Null(result);
                Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            }
        }

        [Fact(DisplayName = "07 : Update a Trade successfully")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_UpdateTradeAsync_SuccessfullyAsync()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var trade = listTrades[0];
            trade.Commited = 0;
            trade.Given = listGivens[2];

            //Act
            using (var postContent = new StringContent(JsonConvert.SerializeObject(trade), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiDonate
                .CreateRequest("Trade/UpdateTradeAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .And(request => request.Method = HttpMethod.Post)
                .PostAsync())
            {
                //Assert
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact(DisplayName = "08 : Disable a Trade successfully")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_DisableTradeAsync_SuccessfullyAsync()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);
            var trade = listTrades[0];

            //Act
            using (var postContent = new StringContent(trade.Id.ToString(), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiDonate
                .CreateRequest("Trade/DisableTradeAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .And(request => request.Method = HttpMethod.Post)
                .PostAsync())
            {
                //Assert
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact(DisplayName = "09 : Get a list of Trades By UserGet")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_ListByUserGetSystemLanguageAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);
            var Id = 3;
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Trade/ListByUserGetSystemLanguageAsync/{0}/{1}/{2}", Id, IdSystems, IdLanguages)))
            {
                var result = JsonConvert.DeserializeObject<List<RootTrade>>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.IsType<List<RootTrade>>(result);
                Assert.Equal(result.Count, 2);
                Assert.Equal(result[0], Mapper.Map<TradeViewModel, RootTrade>(listTrades[1]));
                Assert.Equal(result[1], Mapper.Map<TradeViewModel, RootTrade>(listTrades[2]));
            }
        }

        [Fact(DisplayName = "10 : Get a list of Trades By UserLet")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_ListByUserLetSystemLanguageAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var Id = 2;
            var IdSystems = login.user.idSystem;
            var IdLanguages = login.user.idLanguage;

            //Act
            Environment.ClientApiDonate.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", login.access_token);
            using (var response = await Environment.ClientApiDonate.
                   GetAsync(string.Format("Trade/ListByUserLetSystemLanguageAsync/{0}/{1}/{2}", Id, IdSystems, IdLanguages)))
            {
                var result = JsonConvert.DeserializeObject<List<RootTrade>>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.IsType<List<RootTrade>>(result);
                Assert.Equal(result.Count, 1);
                Assert.Equal(result[0], Mapper.Map<TradeViewModel, RootTrade>(listTrades[1]));
            }
        }

        [Fact(DisplayName = "11: Add Trade Location")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_AddTradeLocationAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);
            var location = locationBoaVistaShopping;
            location.IdParent = listTrades[1].Id;

            //Act
            using (var postContent = new StringContent(JsonConvert.SerializeObject(location), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiDonate
                .CreateRequest("Trade/AddTradeLocationAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .And(request => request.Method = HttpMethod.Post)
                .PostAsync())
            {
                //Assert
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact(DisplayName = "12: Get a list of Location By IdTrades")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_ListTradeLocationsByIdTradesAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var idTrades = listTrades[1].Id;
            var objComp = locationBoaVistaShopping;
            objComp.IdParent = idTrades;

            //Act
            using (var postContent = new StringContent(idTrades.ToString(), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiDonate
                .CreateRequest("Trade/ListTradeLocationsByIdTradesAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .GetAsync())
            {
                var result = JsonConvert.DeserializeObject<List<RootLocation>>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.IsType<List<RootLocation>>(result);
                Assert.Equal(result.Count, 1);
                Assert.Equal(result[0], Mapper.Map<LocationViewModel, RootLocation>(objComp));
            }
        }

        [Fact(DisplayName = "13: Update Trade Location")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_UpdateTradeLocationAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);
            var location = locationBorbaGato;
            location.IdParent = listTrades[1].Id;
            location.Id = 1;

            //Act
            using (var postContent = new StringContent(JsonConvert.SerializeObject(location), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiDonate
                .CreateRequest("Trade/UpdateTradeLocationAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .And(request => request.Method = HttpMethod.Post)
                .PostAsync())
            {
                //Assert
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact(DisplayName = "14: Disable Trade Location")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_DisableTradeLocationAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);
            var idTradeLocations = 1;

            //Act
            using (var postContent = new StringContent(idTradeLocations.ToString(), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiDonate
                .CreateRequest("Trade/DisableTradeLocationAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .And(request => request.Method = HttpMethod.Post)
                .PostAsync())
            {
                //Assert
                response.EnsureSuccessStatusCode();
            }
        }

        [Fact(DisplayName = "15: Add Trade Evaluation")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_AddTradeEvaluationAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);

            //Act
            foreach (var evaluation in listEvaluations)
            {
                using (var postContent = new StringContent(JsonConvert.SerializeObject(evaluation), Encoding.UTF8, "application/json"))
                using (var response = await Environment.ServerApiDonate
                    .CreateRequest("Trade/AddTradeEvaluationAsync")
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

        [Fact(DisplayName = "16: Get a list of Evaluation By IdTrades")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_ListTradeEvaluationsByIdTradesAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(1);
            var idTrades = listTrades[1].Id;
            var objComp = listEvaluations[0];

            //Act
            using (var postContent = new StringContent(idTrades.ToString(), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiDonate
                .CreateRequest("Trade/ListTradeEvaluationsByIdTradesAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .GetAsync())
            {
                var result = JsonConvert.DeserializeObject<List<RootEvaluation>>(await response.Content.ReadAsStringAsync());

                //Assert
                response.EnsureSuccessStatusCode();
                Assert.IsType<List<RootEvaluation>>(result);
                Assert.Equal(result.Count, 1);
                Assert.Equal(result[0], Mapper.Map<EvaluationViewModel, RootEvaluation>(objComp));
            }
        }

        [Fact(DisplayName = "17: Update Trade Evaluation")]
        [Trait("Category", "TradeController.Donate.WebAPI.IntegrationTest")]
        public async Task TradeController_UpdateTradeEvaluationAsync_Successfully()
        {
            //Arrange
            var login = await GetLoginUserAsync(2);
            var evaluation = listEvaluations[0];
            evaluation.UserGetGrade = 1;
            evaluation.CommentsUserGet = "porcaria";
            evaluation.ActiveGet = 0;

            //Act
            using (var postContent = new StringContent(JsonConvert.SerializeObject(evaluation), Encoding.UTF8, "application/json"))
            using (var response = await Environment.ServerApiDonate
                .CreateRequest("Trade/UpdateTradeEvaluationAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .And(request => request.Method = HttpMethod.Post)
                .PostAsync())
            {
                //Assert
                response.EnsureSuccessStatusCode();
            }
        }

        /*
    public class AccountControllerIntegrationTests
    {
        public AccountControllerIntegrationTests()
        {
            Environment.CriarServidor();
        }

        [Fact(DisplayName = "Registrar organizador com sucesso")]
        [Trait("Category","Testes de integração API")]
        public async Task AccountController_RegistrarNovoOrganizador_RetornarComSucesso()
        {
            // Arrange 
            var userFaker = new Faker<RegisterViewModel>("pt_BR")
                .RuleFor(r => r.Nome, c => c.Name.FullName(Name.Gender.Male))
                .RuleFor(r => r.CPF, c => c.Person.Cpf().Replace(".","").Replace("-",""))
                // remoção de acento no email
                .RuleFor(r => r.Email, (c, r) => RemoverAcentos(c.Internet.Email(r.Nome).ToLower()));

            var user = userFaker.Generate();
            user.Senha = "Teste@123";
            user.SenhaConfirmacao = "Teste@123";

            usinng ()var postContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            // Act 
            usinng ()var response = await Environment.Client.PostAsync("api/v1/nova-conta", postContent);
            var userResult = JsonConvert.DeserializeObject<UserReturnJson>(await response.Content.ReadAsStringAsync());
            var token = userResult.data.result.access_token;

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotEmpty(token);
        }

        private static string RemoverAcentos(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }         * */
    }
}
