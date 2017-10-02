using Organizer.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Organizer.UI.Data.Lookups;
using System;

namespace Organizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IPersonLookupDataService _personLookupService;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMeetingLookupDataService _meetingLookupDataService;

        public NavigationViewModel(IPersonLookupDataService personLookupService,
            IEventAggregator eventAggregator, IMeetingLookupDataService meetingLookupDataService)
        {
            _personLookupService = personLookupService;
            _eventAggregator = eventAggregator;
            _meetingLookupDataService = meetingLookupDataService;
            Persons = new ObservableCollection<NavigationItemViewModel>();
            Meetings = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public async Task LoadAsync()
        {
            var lookup = await _personLookupService.GetPersonLookupAsync();
            Persons.Clear();
            foreach (var item in lookup)
            {
                Persons.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                    nameof(PersonDetailViewModel), _eventAggregator));
            }

            lookup = await _meetingLookupDataService.GetMeetingLookupAsync();
            Meetings.Clear();
            foreach (var item in lookup)
            {
                Meetings.Add(new NavigationItemViewModel(item.Id, item.DisplayMember,
                    nameof(MeetingDetailViewModel), _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Persons { get; }
        public ObservableCollection<NavigationItemViewModel> Meetings { get; }

        private void AfterDetailSaved(AfterDetailSavedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(PersonDetailViewModel):
                    AfterDetailsSaved(Persons, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailsSaved(Meetings, args);
                    break;
            }
        }

        private void AfterDetailsSaved(ObservableCollection<NavigationItemViewModel> items, AfterDetailSavedEventArgs args)
        {
            var lookupItem = items.SingleOrDefault(x => x.Id == args.Id);
            if (lookupItem == null)
            {
                Persons.Add(new NavigationItemViewModel(args.Id, args.DisplayMember,
                    args.ViewModelName, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = args.DisplayMember;
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(PersonDetailViewModel):
                    AfterDetailDeleted(Persons, args);
                    break;
                case nameof(MeetingDetailViewModel):
                    AfterDetailDeleted(Meetings, args);
                    break;
            }
        }

        private void AfterDetailDeleted(ObservableCollection<NavigationItemViewModel> items, AfterDetailDeletedEventArgs args)
        {
            var item = items.SingleOrDefault(x => x.Id == args.Id);
            if (item != null)
            {
                items.Remove(item);
            }
        }
    }
}
