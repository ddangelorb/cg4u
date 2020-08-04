using System.Threading.Tasks;
using CG4U.Security.ClientApp.Services.Interfaces;
using CG4U.Security.ClientApp.Views;
using MvvmHelpers;
using Xamarin.Forms;

namespace CG4U.Security.ClientApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IAccountService _accountService;
        private readonly IPersonService _personService;
        private string _email;
        private string _password;
        private Command _loginCommand;

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value, "Email"); }
        }
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value, "Password"); }
        }
        public Command LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand = new Command(async () => await ExecuteLoginCommand()));
            }
        }

        public LoginViewModel()
        {
            Title = "Login";
            _email = string.Empty;
            _password = string.Empty;
            _accountService = DependencyService.Get<IAccountService>();
            _personService = DependencyService.Get<IPersonService>();
        }

        protected async Task ExecuteLoginCommand()
        {
            if (IsBusy) return;
            IsBusy = true;

            var login = await _accountService.LoginAsync(this);
            if (login != null && login.access_token != null)
            {
                var person = await _personService.GetByIdUsers(login.user.id);
                if (person != null)
                {
                    App.IsLoggedIn = true;
                    Application.Current.Properties["token"] = login.access_token;
                    Application.Current.Properties["userName"] = string.Concat(login.user.firstName, " ", login.user.surName);
                    Application.Current.Properties["idUser"] = login.user.id;
                    Application.Current.Properties["idPersonGroups"] = person.PersonGroup.Id;

                    if (Device.RuntimePlatform == Device.iOS)
                        Application.Current.MainPage = new MainPage();
                    else
                        Application.Current.MainPage = new NavigationPage(new MainPage());
                }
                else
                {
                    App.IsLoggedIn = false;
                    await Application.Current.MainPage.DisplayAlert("Login", "Login does not belong to a group.", "Ok");
                }
            }
            else
            {
                App.IsLoggedIn = false;
                await Application.Current.MainPage.DisplayAlert("Login", "Login or password invalid", "Ok");
            }
            IsBusy = false;
        }
    }
}
