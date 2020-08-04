using System;
using Xamarin.Forms;
using CG4U.Donate.ClientApp.Med.Services.Interfaces;
using CG4U.Donate.ClientApp.Med.Models;
using System.Threading.Tasks;

namespace CG4U.Donate.ClientApp.Med.ViewModels
{
    public class GivenViewModel : BaseViewModel
    {
        private readonly IDonationService _donationService;
        private Donation _donation;

        private int _quantity;
        private byte[] _img;
        private DateTime _expirationDate;

        private string _address;
        private string _city;
        private string _state;
        private string _zipCode;
        private double _latitude;
        private double _longitude;
        private double _maxDistanceLet;

        private Command _addCommand;

        public int Quantity
        {
            get { return _quantity; }
            set { SetProperty(ref _quantity, value, "Quantity"); }
        }
        public bool ImgSelected { get; set; }
        public byte[] Img
        {
            get { return _img; }
            set { SetProperty(ref _img, value, "Img"); }
        }
        public DateTime ExpirationDate
        {
            get { return _expirationDate; }
            set { SetProperty(ref _expirationDate, value, "ExpirationDate"); }
        }
        public string Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value, "Address"); }
        }
        public string City
        {
            get { return _city; }
            set { SetProperty(ref _city, value, "City"); }
        }
        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value, "State"); }
        }
        public string ZipCode
        {
            get { return _zipCode; }
            set { SetProperty(ref _zipCode, value, "ZipCode"); }
        }
        public double Latitude
        {
            get { return _latitude; }
            set { SetProperty(ref _latitude, value, "Latitude"); }
        }
        public double Longitude
        {
            get { return _longitude; }
            set { SetProperty(ref _longitude, value, "Longitude"); }
        }
        public double MaxDistanceLet
        {
            get { return _maxDistanceLet; }
            set { SetProperty(ref _maxDistanceLet, value, "MaxDistanceLet"); }
        }
        public Command AddCommand
        {
            get
            {
                return _addCommand ?? (_addCommand = new Command(async () => await ExecuteAddCommand()));
            }
        }

        public GivenViewModel(Donation donation)
        {
            Title = "Doar";
            _quantity = 1;
            _maxDistanceLet = 5;
            _expirationDate = DateTime.Now;
            _donation = donation;
            _donationService = DependencyService.Get<IDonationService>();
        }

        protected async Task ExecuteAddCommand()
        {
            if (IsBusy) return;
            IsBusy = true;

            if (_img != null)
            {
                var ss = _img.ToString();
            }

            //await _donationService.AddGivenAsync(null);

            IsBusy = false;
        }
    }
}
