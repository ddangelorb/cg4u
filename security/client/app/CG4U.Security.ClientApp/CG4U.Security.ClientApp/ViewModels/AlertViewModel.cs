using System.ComponentModel;
using CG4U.Security.ClientApp.Models;
using MvvmHelpers;

namespace CG4U.Security.ClientApp.ViewModels
{
    public class AlertViewModel : BaseViewModel
    {
        public string CountBadge
        {
            get { return Alerts.Count == 0 ? "" : $"{Alerts.Count}+"; }
        }

        public ObservableRangeCollection<Alert> Alerts { get; }

        public AlertViewModel()
        {
            Alerts = new ObservableRangeCollection<Alert>();
        }
    }
}
