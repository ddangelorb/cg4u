using System;
using System.Threading.Tasks;
using Xunit;

namespace CG4U.Auth.WebAPI.IntegrationTest
{
    public class AccountControllerIntegrationTest
    {
        public AccountControllerIntegrationTest()
        {
        }

        /*
        [Fact(DisplayName = "DonationController: New User Gets an Unauthorized HttpStatus")]
        [Trait("Category", "DonationController.Donate.WebAPI.IntegrationTest")]
        public async Task DonationController_GetDonationByIdAsync_NewUser_Unauthorized()
        {
            //Arrange
            var email = "danieldrb@gmail.com";
            var pwd = "Daniel@123";

            var registerViewModel = new UserViewModel
            {
                FirstName = "Daniel",
                SurName = "Resende",
                Email = email,
                Password = pwd,
                PasswordConfirmation = pwd,
                IdSystems = (int)Systems.CG4U_Med,
                Roles = new List<UserRoles>() {
                    UserRoles.UserDonate
                }
            };
            var register = await UserUtils.DoRegister(Environment.ClientApiAuth, registerViewModel);

            var loginViewModel = new UserViewModel
            {
                FirstName = "Daniel",
                SurName = "Resende",
                Email = email,
                Password = pwd,
                PasswordConfirmation = pwd,
                IdSystems = (int)Systems.CG4U_Med,
                IdLanguages = (int)Languages.BrazilianPortuguese
            };
            var login = await UserUtils.DoLogin(Environment.ClientApiAuth, loginViewModel);

            var viewModel = new SystemLanguageViewModel
            {
                Id = 1,
                IdSystems = login.user.idSystem,
                IdLanguages = login.user.idLanguage
            };

            //Act
            var postContent = new StringContent(JsonConvert.SerializeObject(viewModel), Encoding.UTF8, "application/json");
            var response = await Environment.ServerApiDonate
                .CreateRequest("Donation/GetDonationByIdAsync")
                .AddHeader("Authorization", "Bearer " + login.access_token)
                .And(request => request.Content = postContent)
                .GetAsync();

            //Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }*/
    }
}
