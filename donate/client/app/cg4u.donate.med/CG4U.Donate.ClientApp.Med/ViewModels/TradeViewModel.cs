using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CG4U.Donate.ClientApp.Med.Models;
using CG4U.Donate.ClientApp.Med.Services.Interfaces;
using Xamarin.Forms;

namespace CG4U.Donate.ClientApp.Med.ViewModels
{
    public class TradeViewModel : BaseViewModel
    {
        private readonly ITradeService _tradeService;
        private int _idDonationsMatched;
        private double _maxDistanceMatch;
        private bool _isTabHistoryVisible;
        private bool _isTabGivenVisible;
        private bool _isTabDesiredVisible;
        private string _queryMatch;
        private Command _matchDesiredsCommand;
        private Command _matchGivensCommand;

        public ObservableCollection<GroupModelCollection<Trade>> GroupedTradeList { get; set; }

        public int IdDonationsMatched         
        {
            get { return _idDonationsMatched; }
            set { SetProperty(ref _idDonationsMatched, value, "IdDonationsMatched"); }
        }
        public double MaxDistanceMatch
        {
            get { return _maxDistanceMatch; }
            set { SetProperty(ref _maxDistanceMatch, value, "MaxDistanceMatch"); }
        }
        public bool IsTabHistoryVisible
        {
            get { return _isTabHistoryVisible; }
            set { SetProperty(ref _isTabHistoryVisible, value, "IsTabHistoryVisible"); }
        }
        public bool IsTabGivenVisible
        {
            get { return _isTabGivenVisible; }
            set { SetProperty(ref _isTabGivenVisible, value, "IsTabGivenVisible"); }
        }
        public bool IsTabDesiredVisible
        {
            get { return _isTabDesiredVisible; }
            set { SetProperty(ref _isTabDesiredVisible, value, "IsTabDesiredVisible"); }
        }
        public string QueryMatch
        { 
            get { return _queryMatch; } 
            set { SetProperty(ref _queryMatch, value, "QueryMatch"); }
        }
        public Command MatchDesiredsCommand
        {
            get
            {
                return _matchDesiredsCommand ?? (_matchDesiredsCommand = new Command(async () => await ExecuteMatchDesiredsCommand()));
            }
        }
        public Command MatchGivensCommand
        {
            get
            {
                return _matchGivensCommand ?? (_matchGivensCommand = new Command(async () => await ExecuteMatchGivensCommand()));
            }
        }

        public TradeViewModel()
        {
            Title = "Compartilhamento";
            _maxDistanceMatch = 5;
            _isTabHistoryVisible = true;
            _isTabGivenVisible = false;
            _isTabDesiredVisible = false;
            _tradeService = DependencyService.Get<ITradeService>();
            GroupedTradeList = new ObservableCollection<GroupModelCollection<Trade>>();
        }

        protected async Task ExecuteMatchDesiredsCommand()
        {
            if (IsBusy) return;
            IsBusy = true;

            var trades = await _tradeService.ListMatchDesiredsByPositionAsync(_idDonationsMatched, _maxDistanceMatch);

            IsBusy = false;
        }

        protected async Task ExecuteMatchGivensCommand()
        {
            if (IsBusy) return;
            IsBusy = true;

            var trades = await _tradeService.ListMatchGivensByPositionAsync(_idDonationsMatched, _maxDistanceMatch);

            IsBusy = false;
        }
    }
}

