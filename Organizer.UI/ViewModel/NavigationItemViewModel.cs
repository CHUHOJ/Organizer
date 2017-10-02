using Prism.Commands;
using System.Windows.Input;
using System;
using Organizer.UI.Event;
using Prism.Events;

namespace Organizer.UI.ViewModel
{
    public class NavigationItemViewModel : ViewModelBase
    {
        private readonly string _detailViewModelName;
        private readonly IEventAggregator _eventAggregator;

        public NavigationItemViewModel(int id, string displayMember,
             string detailViewModelName, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            Id = id;
            DisplayMember = displayMember;
            _detailViewModelName = detailViewModelName;
            OpenDetailViewCommand = new DelegateCommand(OnOpenDetailViewExecute);
        }

        public int Id { get; }

        private string _displayMember;
        public string DisplayMember
        {
            get { return _displayMember; }
            set { _displayMember = value; OnPropertyChanged(); }
        }

        public ICommand OpenDetailViewCommand { get; }

        private void OnOpenDetailViewExecute()
        {
            _eventAggregator.GetEvent<OpenDetailViewEvent>()
                .Publish(new OpenDetailViewEventArgs
                {
                    Id = Id,
                    ViewModelName = _detailViewModelName
                });
        }
    }
}
