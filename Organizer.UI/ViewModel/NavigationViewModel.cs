using Organizer.UI.Data;
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
            _eventAggregator.GetEvent<AfterPersonSavedEvent>().Subscribe(AfterPersonSaved);
        }

        private void AfterPersonSaved(AfterPersonSavedEventArgs obj)
        {
            var lookupItem = Persons.SingleOrDefault(x => x.Id == obj.Id);
            if (lookupItem == null)
            {
                Persons.Add(new NavigationItemViewModel(obj.Id, obj.DisplayMember, _eventAggregator));
            }
            else
            {
                lookupItem.DisplayMember = obj.DisplayMember;
            }
        }

        public async Task LoadAsync()
        {
            var lookup = await _personLookupService.GetPersonLookupAsync();
            Persons.Clear();
            foreach (var item in lookup)
            {
                Persons.Add(new NavigationItemViewModel(item.Id, item.DisplayMember, _eventAggregator));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Persons { get; }
    }
}
