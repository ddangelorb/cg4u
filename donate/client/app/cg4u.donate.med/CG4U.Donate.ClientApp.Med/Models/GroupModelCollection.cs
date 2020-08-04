using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace CG4U.Donate.ClientApp.Med.Models
{
    public class GroupModelCollection<T> : ObservableCollection<T>, INotifyPropertyChanged
    {
        private bool _expanded;

        public string Title { get; set; }
        public string TitleWithItemCount
        {
            get { return string.Format("{0} ({1})", Title, GroupCount); }
        }
        public string ShortName { get; set; }
        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged("Expanded");
                    OnPropertyChanged("StateIcon");
                }
            }
        }
        public string StateIcon
        {
            get { return Expanded ? "expanded_blue.png" : "collapsed_blue.png"; }
        }
        public int GroupCount { get; set; }

        public GroupModelCollection(string title, string shortName, bool expanded = true)
        {
            Title = title;
            ShortName = shortName;
            Expanded = expanded;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
