using System;
using Plugin.Badge.Abstractions;
using Xamarin.Forms;

namespace CG4U.Security.ClientApp.Views
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            Page homePage, watchPage, alertPage, chatPage, videoPage = null;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    homePage = new NavigationPage(new HomePage())
                    {
                        Title = "Início"
                    };
                    homePage.Icon = "tab_home.png";

                    watchPage = new NavigationPage(new WatchPage())
                    {
                        Title = "Assistir"
                    };
                    watchPage.Icon = "tab_watch.png";

                    alertPage = new NavigationPage(new AlertPage())
                    {
                        Title = "Alertas",
                        Icon = "tab_alert.png"
                    };
                    alertPage.SetBinding(TabBadge.BadgeTextProperty, new Binding("CountBadge"));

                    videoPage = new NavigationPage(new VideoPage())
                    {
                        Title = "Videos",
                        Icon = "tab_video.png"
                    };

                    chatPage = new NavigationPage(new ChatPage())
                    {
                        Title = "Chat"
                    };
                    chatPage.Icon = "tab_chat.png";
                    TabBadge.SetBadgeText(chatPage, "2+");
                    //https://github.com/jsuarezruiz/xamarin-forms-tab-badge

                    /*
                    settingsPage = new NavigationPage(new SettingsPage())
                    {
                        Title = "Config"
                    };
                    settingsPage.Icon = "tab_settings.png";*/

                    break;
                default:
                    homePage = new HomePage()
                    {
                        Title = "Início"
                    };

                    watchPage = new WatchPage()
                    {
                        Title = "Assistir"
                    };

                    alertPage = new AlertPage()
                    {
                        Title = "Alertas"
                    };

                    videoPage = new VideoPage()
                    {
                        Title = "Videos"
                    };

                    chatPage = new ChatPage()
                    {
                        Title = "Chat"
                    };

                    /*
                    settingsPage = new SettingsPage()
                    {
                        Title = "Config"
                    };*/
                    break;
            }

            Children.Add(homePage);
            Children.Add(watchPage);
            Children.Add(alertPage);
            Children.Add(videoPage);
            Children.Add(chatPage);
            //Children.Add(settingsPage);

            Title = Children[0].Title;
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage?.Title ?? string.Empty;
        }
    }
}
