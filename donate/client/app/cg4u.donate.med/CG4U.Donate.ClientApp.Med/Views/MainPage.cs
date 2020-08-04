using System;
using Xamarin.Forms;

namespace CG4U.Donate.ClientApp.Med
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            Page homePage, givenPage, desiredPage, tradePage, settingsPage = null;

            BarBackgroundColor = Color.WhiteSmoke;
            BarTextColor = Color.FromHex("#494949");

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    homePage = new NavigationPage(new Views.HomePage())
                    {
                        Title = "Início",
                        Icon = "tab_home.png",
                    };

                    givenPage = new NavigationPage(new Views.SearchPage("let"))
                    {
                        Title = "Doar",
                        Icon = "tab_donate.png"
                    };

                    desiredPage = new NavigationPage(new Views.SearchPage("get"))
                    {
                        Title = "Receber",
                        Icon = "tab_search.png"
                    };

                    tradePage = new NavigationPage(new Views.TradePage())
                    {
                        Title = "Compartilhar",
                        Icon = "tab_trade.png"
                    };

                    settingsPage = new NavigationPage(new AboutPage())
                    {
                        Title = "Sobre",
                        Icon = "tab_about.png"
                    };

                    break;
                default:
                    homePage = new Views.HomePage()
                    {
                        Title = "Início"
                    };

                    givenPage = new Views.SearchPage("let")
                    {
                        Title = "Doar"
                    };

                    desiredPage = new Views.SearchPage("get")
                    {
                        Title = "Receber"
                    };

                    tradePage = new Views.TradePage()
                    {
                        Title = "Compartilhar"
                    };

                    settingsPage = new AboutPage()
                    {
                        Title = "Sobre"
                    };
                    break;
            }

            Children.Add(homePage);
            Children.Add(givenPage);
            Children.Add(desiredPage);
            Children.Add(tradePage);
            Children.Add(settingsPage);

            Title = Children[0].Title;
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage?.Title ?? string.Empty;
        }
    }
}
