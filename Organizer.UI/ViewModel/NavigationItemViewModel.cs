using Prism.Commands;
using System.Windows.Input;
using System;
using Organizer.UI.Event;
using Prism.Events;

namespace Organizer.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;

        public NavigationItemViewModel(int id, string displayMember,
            IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Id = id;
            DisplayMember = displayMember;
            OpenPersonDetailViewCommand = new DelegateCommand(OnOpenPersonDetailView);
        }

        public int Id { get; }

        private string _displayMember;
        public string DisplayMember
        {
            get { return _displayMember; }
            set { _displayMember = value; OnPropertyChanged(); }
        }

        public ICommand OpenPersonDetailViewCommand { get; }

        private void OnOpenPersonDetailView()
        {
            _eventAggregator.GetEvent<OpenPersonDetailViewEvent>()
                .Publish(Id);
        }
    }
}
