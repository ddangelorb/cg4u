using System;
using System.IO;
using CG4U.Donate.ClientApp.Med.Models;
using Xamarin.Forms;
using System.Threading.Tasks;
using CG4U.Donate.ClientApp.Med.Interfaces;
using Xamarin.Essentials;
using System.Linq;

namespace CG4U.Donate.ClientApp.Med.Views
{
    public partial class GivenPage : ContentPage
    {
        private ViewModels.GivenViewModel _viewModel;

        public GivenPage(Donation donation)
        {
            InitializeComponent();

            BindingContext = _viewModel = new ViewModels.GivenViewModel(donation);
            //imgDonation.Source = donation.Icon;
            imgDonation.Source = ImageSource.FromStream(() => new MemoryStream(donation.Img));
            labelDonation.Text = donation.Name;

            InitPickerState();
        }

        private async void OnToolbarItemAddClicked(object sender, System.EventArgs e)
        {
            await Application.Current.MainPage.DisplayAlert("Doar", "Doação adicionada com sucesso", "Ok");
            await Navigation.PopToRootAsync();
        }

        private async void OnSwitchImageToggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            if (switchImage.IsToggled)
                await GetImageClickedAsync();
        }

        private async void OnViewCellGPSTapped(object sender, System.EventArgs e)
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    var placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);

                    var placemark = placemarks?.FirstOrDefault();
                    if (placemark != null)
                    {
                        _viewModel.Latitude = location.Latitude;
                        _viewModel.Longitude = location.Longitude;
                        entryAddress.Text = string.Concat(
                            placemark.Thoroughfare,
                            " ",
                            placemark.SubThoroughfare
                        );
                        entryCity.Text = placemark.Locality;
                        entryZipCode.Text = placemark.PostalCode;

                        pickerState.SelectedItem = placemark.AdminArea;
                    }
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                await Application.Current.MainPage.DisplayAlert("GPS", "FeatureNotSupportedException", "Ok");
            }
            catch (PermissionException pEx)
            {
                await Application.Current.MainPage.DisplayAlert("GPS", "PermissionException", "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("GPS", "Exception", "Ok");
            }            
        }

        void OnMaxLetSliderChanged(Object sender, EventArgs e)
        {
            labelKM.Text = sliderMaxLet.Value.ToString("#.##") + " Kilometros";
        }

        private async Task GetImageClickedAsync()
        {
            //switchImage.IsEnabled = false;
            var stream = await DependencyService.Get<IPicturePicker>().GetImageStreamAsync();

            if (stream != null)
            {
                var imgBytes = new byte[stream.Length];
                stream.Read(imgBytes, 0, imgBytes.Length);
                stream.Position = 0;

                var originalContent = this.Content;
                var image = new Image
                {
                    Source = ImageSource.FromStream(() => stream),
                    BackgroundColor = Color.Gray
                };

                TapGestureRecognizer recognizer = new TapGestureRecognizer();
                recognizer.Tapped += (sender2, args) =>
                {
                    _viewModel.Img = imgBytes;
                    this.Content = originalContent;
                    //switchImage.IsEnabled = true;
                };
                image.GestureRecognizers.Add(recognizer);
                this.Content = image;
            }
            else
            {
                //switchImage.IsEnabled = true;
            }
        }

        private void InitPickerState()
        {
            pickerState.Items.Add("AC");
            pickerState.Items.Add("AL");
            pickerState.Items.Add("AM");
            pickerState.Items.Add("AP");
            pickerState.Items.Add("BA");
            pickerState.Items.Add("CE");
            pickerState.Items.Add("DF");
            pickerState.Items.Add("ES");
            pickerState.Items.Add("GO");
            pickerState.Items.Add("MA");
            pickerState.Items.Add("MG");
            pickerState.Items.Add("MS");
            pickerState.Items.Add("MT");
            pickerState.Items.Add("PA");
            pickerState.Items.Add("PB");
            pickerState.Items.Add("PE");
            pickerState.Items.Add("PI");
            pickerState.Items.Add("PR");
            pickerState.Items.Add("RJ");
            pickerState.Items.Add("RN");
            pickerState.Items.Add("RO");
            pickerState.Items.Add("RR");
            pickerState.Items.Add("RS");
            pickerState.Items.Add("SC");
            pickerState.Items.Add("SE");
            pickerState.Items.Add("SP");
            pickerState.Items.Add("TO");        
        }
    }
}
