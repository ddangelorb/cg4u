using System;
using CG4U.Donate.ClientApp.Med.Models;
using CG4U.Donate.ClientApp.Med.Services.Interfaces;
using Xamarin.Forms;

namespace CG4U.Donate.ClientApp.Med.ViewModels
{
    public class NewBlankTradeViewModel : BaseViewModel
    {
        private readonly ITradeService _tradeService;
        private Donation _donation;

        private string _donorName;
        private string _patientRG;
        private string _patientCpfCnpj;
        private string _patientName;
        private string _patientAddress;
        private byte[] _imgPrescription;
        private byte[] _imgPrescriptionBack;
        private int _quantityDonated;
        private string _whoCollected;
        private DateTime _returnDate;

        public string DonorName
        {
            get { return _donorName; }
            set { SetProperty(ref _donorName, value, "DonorName"); }
        }
        public string PatientRG
        {
            get { return _patientRG; }
            set { SetProperty(ref _patientRG, value, "PatientRG"); }
        }
        public string PatientCpfCnpj
        {
            get { return _patientCpfCnpj; }
            set { SetProperty(ref _patientCpfCnpj, value, "PatientCpfCnpj"); }
        }
        public string PatientName
        {
            get { return _patientName; }
            set { SetProperty(ref _patientName, value, "PatientName"); }
        }
        public string PatientAddress
        {
            get { return _patientAddress; }
            set { SetProperty(ref _patientAddress, value, "PatientAddress"); }
        }
        public bool ImgPrescriptionSelected { get; set; }
        public byte[] ImgPrescription
        {
            get { return _imgPrescription; }
            set { SetProperty(ref _imgPrescription, value, "ImgPrescription"); }
        }
        public bool ImgPrescriptionBackSelected { get; set; }
        public byte[] ImgPrescriptionBack
        {
            get { return _imgPrescriptionBack; }
            set { SetProperty(ref _imgPrescriptionBack, value, "ImgPrescriptionBack"); }
        }
        public int QuantityDonated
        {
            get { return _quantityDonated; }
            set { SetProperty(ref _quantityDonated, value, "QuantityDonated"); }
        }
        public string WhoCollected
        {
            get { return _whoCollected; }
            set { SetProperty(ref _whoCollected, value, "WhoCollected"); }
        }
        public DateTime ReturnDate
        {
            get { return _returnDate; }
            set { SetProperty(ref _returnDate, value, "ReturnDate"); }
        }

        public NewBlankTradeViewModel(Donation donation)
        {
            _donation = donation;
            _tradeService = DependencyService.Get<ITradeService>();

            Title = "Novo Compartilhamento";

            _donorName = (string)Application.Current.Properties["userName"];
            _returnDate = DateTime.Now;
        }
    }
}
