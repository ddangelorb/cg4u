using System;
using CG4U.Security.ClientApp.Services;
using CG4U.Security.ClientApp.Services.Mocks;
using CG4U.Security.ClientApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace CG4U.Security.ClientApp
{
    public partial class App : Application
    {
        public static bool IsMockedServices = true;
        public static int IdSystems = 4;    //CG4U.Security
        public static int IdLanguages = 1;  //Brazilian Portuguese
        public static bool IsLoggedIn = false;

        public App()
        {
            InitializeComponent();
            RegisterServicesIoc();

            if (!IsLoggedIn)
                MainPage = new Views.LoginPage();
            else
            {
                if (Device.RuntimePlatform == Device.iOS)
                    MainPage = new MainPage();
                else
                    MainPage = new NavigationPage(new MainPage());
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private void RegisterServicesIoc()
        {
            if (IsMockedServices)
            {
                DependencyService.Register<MockAccountService>();
                DependencyService.Register<MockPersonService>();
                DependencyService.Register<MockChatService>();
            }
            else
            {
                DependencyService.Register<AccountService>();
                DependencyService.Register<PersonService>();
                DependencyService.Register<ChatService>();
            }
        }
    }
}
