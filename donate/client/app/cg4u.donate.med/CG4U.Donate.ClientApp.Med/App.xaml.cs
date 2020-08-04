using Xamarin.Forms;
using CG4U.Donate.ClientApp.Med.Services;
using CG4U.Donate.ClientApp.Med.Services.Mock;

namespace CG4U.Donate.ClientApp.Med
{
    public partial class App : Application
    {
        public static bool IsMockedServices = true;
        public static int IdSystems = 1;    //CG4U.Med
        public static int IdLanguages = 1;  //Brazilian Portuguese
        public static bool IsLoggedIn = false;

        public App()
        {
            InitializeComponent();
            RegisterServicesIoc();

            if (!IsLoggedIn)
                MainPage =  new Views.LoginPage();
            else 
            {
                if (Device.RuntimePlatform == Device.iOS)
                    MainPage = new MainPage();
                else
                    MainPage = new NavigationPage(new MainPage());
            }
        }

        private void RegisterServicesIoc()
        {
            if (IsMockedServices)
            {
                DependencyService.Register<MockAccountService>();
                DependencyService.Register<MockDonationService>();
                DependencyService.Register<MockTradeService>();
            }
            else
            {
                DependencyService.Register<AccountService>();
                DependencyService.Register<DonationService>();
                DependencyService.Register<TradeService>();
            }
        }
    }
}
