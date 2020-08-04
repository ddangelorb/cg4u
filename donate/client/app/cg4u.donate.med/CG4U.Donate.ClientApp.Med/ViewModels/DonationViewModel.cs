using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CG4U.Donate.ClientApp.Med.Models;
using Xamarin.Forms;
using CG4U.Donate.ClientApp.Med.Services.Interfaces;

namespace CG4U.Donate.ClientApp.Med.ViewModels
{
    public class DonationViewModel : BaseViewModel
    {
        private readonly IDonationService _donationService;
        private string _query;
        private Command _searchCommand;

        public ObservableCollection<GroupModelCollection<Donation>> GroupedDonationList { get; set; }

        public string Query
        {
            get { return _query; }
            set { SetProperty(ref _query, value, "Query"); }
        }
        public Command SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new Command(async () => await ExecuteSearchCommand()));
            }
        }

        public DonationViewModel()
        {
            Title = "Browse";
            _query = string.Empty;
            _donationService = DependencyService.Get<IDonationService>();
            GroupedDonationList = new ObservableCollection<GroupModelCollection<Donation>>();
        }

        protected async Task ExecuteSearchCommand()
        {
            if (IsBusy) return;
            IsBusy = true;

            GroupedDonationList.Clear();

            var donations = await _donationService.ListDonationsByLanguageAndNameAsync(_query);
            if (donations != null && donations.Count > 0)
            {
                var donationsDads = donations.Where(c => c.idDonationsDad == null);
                foreach (var itemDad in donationsDads)
                {
                    if (itemDad.names.Count > 0)
                    {
                        var groupName = itemDad.names[0].name;
                        var newGroup = new GroupModelCollection<Donation>(groupName, groupName.Substring(0, 1)){};

                        var donationsDadSons = donations.Where(c => c.idDonationsDad == itemDad.idDonations);
                        newGroup.GroupCount = donationsDadSons.Count();
                        foreach (var itemDadSon in donationsDadSons)
                        {
                            if (itemDadSon.names.Count > 0)
                                newGroup.Add(new Donation() { Id = itemDadSon.idDonations, Name = itemDadSon.names[0].name, Icon = "tab_about.png", Img = itemDadSon.img });
                        }

                        GroupedDonationList.Add(newGroup);
                    }
                }
            }
            else
                await Application.Current.MainPage.DisplayAlert("Query", "Donations not found", "Ok");
            
            IsBusy = false;
        }
    }
}
