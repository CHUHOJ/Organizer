using Organizer.UI.Data;
using Organizer.UI.Event;
using Prism.Events;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

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
            var personSaved = Persons.Single(x => x.Id == obj.Id);
            personSaved.DisplayMember = obj.DisplayMember;
        }

        public async Task LoadAsync()
        {
            var lookup  = await _personLookupService.GetPersonLookupAsync();
            Persons.Clear();
            foreach (var item in lookup)
            {
                Persons.Add(new NavigationItemViewModel(item.Id, item.DisplayMember));
            }
        }

        public ObservableCollection<NavigationItemViewModel> Persons { get; }

        private NavigationItemViewModel _selectedPerson;
        public NavigationItemViewModel SelectedPerson
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
