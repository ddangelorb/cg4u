using System;
using System.Collections.ObjectModel;
using System.Linq;
using CG4U.Donate.ClientApp.Med.Models;
using Xamarin.Forms;

namespace CG4U.Donate.ClientApp.Med.Views
{
    public partial class SearchPage : ContentPage
    {
        private string _action;
        private ViewModels.DonationViewModel _viewModel;

        public SearchPage(string action)
        {
            _action = action;
            InitializeComponent();
            BindingContext = _viewModel = new ViewModels.DonationViewModel();
        }

        private void OnButtonImageTapped(Object sender, EventArgs e)
        {
            var paramItem = ((GroupModelCollection<Donation>)((Button)sender).CommandParameter);
            var item = _viewModel.GroupedDonationList.Where
                (
                    gdl => gdl.Title == paramItem.Title
                        && gdl.ShortName == paramItem.ShortName
                ).FirstOrDefault();

            var indexItem = _viewModel.GroupedDonationList.IndexOf(item);
            _viewModel.GroupedDonationList[indexItem].Expanded = !_viewModel.GroupedDonationList[indexItem].Expanded;

            var newDonationListUpdated = new ObservableCollection<GroupModelCollection<Donation>>();
            foreach (var vmGroupedDonationList in _viewModel.GroupedDonationList)
            {
                var group = new GroupModelCollection<Donation>(vmGroupedDonationList.Title, vmGroupedDonationList.ShortName, vmGroupedDonationList.Expanded);
                group.GroupCount = vmGroupedDonationList.GroupCount;

                if (group.Expanded)
                {
                    foreach (var vmDonationCollection in vmGroupedDonationList)
                        group.Add(vmDonationCollection);
                }

                newDonationListUpdated.Add(group);
            }

            GroupedDonationView.ItemsSource = newDonationListUpdated;
        }


        private async void OnButtonTradeTappedAsync(Object sender, EventArgs e)
        {
            var paramItem = ((Donation)((Button)sender).CommandParameter);

            if (_action == "let")
                await Navigation.PushAsync(new GivenPage(paramItem));
            else if (_action == "get")
                await Navigation.PushAsync(new DesiredPage(paramItem));
            else if (_action == "newTrade")
                await Navigation.PushAsync(new Trade.NewBlankTradePage(paramItem));
        }
    }
}
