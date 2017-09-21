using Organizer.Model;
using Organizer.UI.Data;
using Organizer.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

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
            Persons = new ObservableCollection<LookupItem>();
        }

        public async Task LoadAsync()
        {
            var lookup  = await _personLookupService.GetPersonLookupAsync();
            Persons.Clear();
            foreach (var item in lookup)
            {
                Persons.Add(item);
            }
        }

        public ObservableCollection<LookupItem> Persons { get; }

        private LookupItem _selectedPerson;
        public LookupItem SelectedPerson
        {
            get { return _selectedPerson; }
            set {
                _selectedPerson = value;
                OnPropertyChanged();
                if (_selectedPerson != null)
                {
                    _eventAggregator.GetEvent<OpenPersonDetailViewEvent>()
                        .Publish(_selectedPerson.Id);
                }
            }
        }

    }
}
