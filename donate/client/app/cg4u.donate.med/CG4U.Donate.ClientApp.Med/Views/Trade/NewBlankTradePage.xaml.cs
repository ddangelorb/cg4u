using System;
using System.Collections.Generic;
using System.IO;
using CG4U.Donate.ClientApp.Med.Models;
using Xamarin.Forms;

namespace CG4U.Donate.ClientApp.Med.Views.Trade
{
    public partial class NewBlankTradePage : ContentPage
    {
        private ViewModels.NewBlankTradeViewModel _viewModel;

        public NewBlankTradePage(Donation donation)
        {
            InitializeComponent();

            BindingContext = _viewModel = new ViewModels.NewBlankTradeViewModel(donation);

            imgDonation.Source = ImageSource.FromStream(() => new MemoryStream(donation.Img));
            labelDonation.Text = donation.Name;
        }

        private async void OnToolbarItemAddClicked(object sender, System.EventArgs e)
        {
            await Application.Current.MainPage.DisplayAlert("Compartilhar", "Compartilhamento adicionado com sucesso", "Ok");
            await Navigation.PopToRootAsync();
        }

        private void OnButtonRGTapped(Object sender, EventArgs e)
        {
            var paramRG = ((string)((Button)sender).CommandParameter);
            if (paramRG.Length > 0)
            {
                
            }
        }

        private void OnButtonCpfCnpjTapped(Object sender, EventArgs e)
        {
            var paramCpfCnpj = ((string)((Button)sender).CommandParameter);
            if (paramCpfCnpj.Length > 0)
            {

            }
        }

        private void OnSwitchPrescriptionToggled(object sender, ToggledEventArgs e)
        {
            //if (switchImage.IsToggled)
                //await GetImageClickedAsync();
        }

        private void OnSwitchPrescriptionBackToggled(object sender, ToggledEventArgs e)
        {
            //if (switchImage.IsToggled)
            //await GetImageClickedAsync();
        }
    }
}
