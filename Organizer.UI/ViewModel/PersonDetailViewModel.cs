using Organizer.UI.Data;
using System.Threading.Tasks;
using Prism.Events;
using Organizer.UI.Event;
using System.Windows.Input;
using Prism.Commands;
using Organizer.UI.Wrapper;

namespace Organizer.UI.ViewModel
{
    public class PersonDetailViewModel : ViewModelBase, IPersonDetailViewModel
    {
        private readonly IPersonDataService _personDataService;
        private readonly IEventAggregator _eventAggregator;

        public PersonDetailViewModel(IPersonDataService personDataService,
            IEventAggregator eventAggregator)
        {
            _personDataService = personDataService;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<OpenPersonDetailViewEvent>()
                .Subscribe(OnOpenPersonDetailView);
            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        private PersonWrapper _person;
        public PersonWrapper Person
        {
            get { return _person; }
            set { _person = value; OnPropertyChanged(); }
        }

        public async Task LoadAsync(int personId)
        {
            var person = await _personDataService.GetByIdAsync(personId);
            Person = new PersonWrapper(person);
            Person.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Person.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        public ICommand SaveCommand { get; }

        private async void OnSaveExecute()
        {
            await _personDataService.SaveAsync(Person.Model);
            _eventAggregator.GetEvent<AfterPersonSavedEvent>().Publish(
                new AfterPersonSavedEventArgs
                {
                    Id = Person.Id,
                    DisplayMember = $"{Person.FirstName} {Person.LastName}"
                });
        }

        private bool OnSaveCanExecute()
        {
            // TODO: check if person has changes
            return Person != null && Person.HasErrors == false;
        }

        private async void OnOpenPersonDetailView(int personId)
        {
            await LoadAsync(personId);
        }
    }
}
