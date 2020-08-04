using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CG4U.Donate.ClientApp.Med.Views
{
    public partial class TradePage : ContentPage
    {
        private ViewModels.TradeViewModel _viewModel;

        public TradePage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new ViewModels.TradeViewModel();
            //https://stackoverflow.com/questions/30807313/how-to-add-a-separator-space-between-rows-on-custom-xamarin-forms-viewcell?utm_medium=organic&utm_source=google_rich_qa&utm_campaign=google_rich_qa
        }

        private async void OnToolbarItemAddNewBlankTradeClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SearchPage("newTrade"));
        }

        private void OnTabHistoryTapped(object sender, System.EventArgs e)
        {
            _viewModel.IsTabHistoryVisible = true;
            _viewModel.IsTabGivenVisible = false;
            _viewModel.IsTabDesiredVisible = false;
        }

        private void OnTabGivenTapped(object sender, System.EventArgs e)
        {
            _viewModel.IsTabHistoryVisible = false;
            _viewModel.IsTabGivenVisible = true;
            _viewModel.IsTabDesiredVisible = false;
        }

        private void OnTabDesiredTapped(object sender, System.EventArgs e)
        {
            _viewModel.IsTabHistoryVisible = false;
            _viewModel.IsTabGivenVisible = false;
            _viewModel.IsTabDesiredVisible = true;
        }
    }
}
