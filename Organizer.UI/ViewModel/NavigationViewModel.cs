using Organizer.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Organizer.UI.Data.Lookups;

namespace Organizer.UI.ViewModel
{
    public class NavigationViewModel : ViewModelBase, INavigationViewModel
    {
        private readonly IPersonLookupDataService _personLookupService;
        private readonly IEventAggregator _eventAggregator;

        public NavigationViewModel(IPersonLookupDataService personLookupService,
            IEventAggregator eventAggregator)
        {
            _personLookupService = personLookupService;
            _eventAggregator = eventAggregator;
            Persons = new ObservableCollection<NavigationItemViewModel>();
            _eventAggregator.GetEvent<AfterDetailSavedEvent>().Subscribe(AfterDetailSaved);
            _eventAggregator.GetEvent<AfterDetailDeletedEvent>().Subscribe(AfterDetailDeleted);
        }

        public async Task LoadAsync()
        {
            var lookup = await _personLookupService.GetPersonLookupAsync();
            Persons.Clear();
            foreach (var item in lookup)
            {
                Persons.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, nameof(PersonDetailViewModel), _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Persons { get; }

        private void AfterDetailSaved(AfterDetailSavedEventArgs obj)
        {
            switch (obj.ViewModelName)
            {
                case nameof(PersonDetailViewModel):
                    var lookupItem = Persons.SingleOrDefault(x => x.Id == obj.Id);
                    if (lookupItem == null)
                    {
                        Persons.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, nameof(PersonDetailViewModel), _eventAggregator));
                    }
                    else
                    {
                        lookupItem.DisplayMember = obj.DisplayMember;
                    }
                    break;
            }
        }

        private void AfterDetailDeleted(AfterDetailDeletedEventArgs args)
        {
            switch (args.ViewModelName)
            {
                case nameof(PersonDetailViewModel):
                    var person = Persons.SingleOrDefault(x => x.Id == args.Id);
                    if (person != null)
                    {
                        Persons.Remove(person);
                    }
                    break;
            }
        }
    }
}
