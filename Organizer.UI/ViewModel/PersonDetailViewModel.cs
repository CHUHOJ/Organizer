using Organizer.UI.Data;
using System.Threading.Tasks;
using Prism.Events;
using Organizer.UI.Event;
using System.Windows.Input;
using Prism.Commands;
using Organizer.UI.Wrapper;
using Organizer.UI.Data.Repositories;

namespace Organizer.UI.ViewModel
{
    public class PersonDetailViewModel : ViewModelBase, IPersonDetailViewModel
    {
        private readonly IPersonRepository _personRepository;
        private readonly IEventAggregator _eventAggregator;

        public PersonDetailViewModel(IPersonRepository personRepository,
            IEventAggregator eventAggregator)
        {
            _personRepository = personRepository;
            _eventAggregator = eventAggregator;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
        }

        private PersonWrapper _person;
        public PersonWrapper Person
        {
            get { return _person; }
            set { _person = value; OnPropertyChanged(); }
        }

        private bool _hasChanges;
        public bool HasChanges
        {
            get { return _hasChanges; }
            set
            {
                if (_hasChanges != value)
                {
                    _hasChanges = value;
                    OnPropertyChanged();
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public async Task LoadAsync(int personId)
        {
            var person = await _personRepository.GetByIdAsync(personId);

            Person = new PersonWrapper(person);
            Person.PropertyChanged += (s, e) =>
            {
                if (!HasChanges)
                {
                    HasChanges = _personRepository.HasChanges();
                }
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
            await _personRepository.SaveAsync();
            HasChanges = _personRepository.HasChanges();
            _eventAggregator.GetEvent<AfterPersonSavedEvent>().Publish(
                new AfterPersonSavedEventArgs
                {
                    Id = Person.Id,
                    DisplayMember = $"{Person.FirstName} {Person.LastName}"
                });
        }

        private bool OnSaveCanExecute()
        {
            return Person != null && Person.HasErrors == false && HasChanges;
        }
    }
}
