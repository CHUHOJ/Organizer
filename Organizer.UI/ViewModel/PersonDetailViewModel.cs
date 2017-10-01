using Organizer.UI.Data;
using System.Threading.Tasks;
using Prism.Events;
using Organizer.UI.Event;
using System.Windows.Input;
using Prism.Commands;
using Organizer.UI.Wrapper;
using Organizer.UI.Data.Repositories;
using Organizer.Model;
using System;
using Organizer.UI.View.Services;

namespace Organizer.UI.ViewModel
{
    public class PersonDetailViewModel : ViewModelBase, IPersonDetailViewModel
    {
        private readonly IPersonRepository _personRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;

        public PersonDetailViewModel(IPersonRepository personRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _personRepository = personRepository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
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

        public async Task LoadAsync(int? personId)
        {
            var person = personId.HasValue ?
                await _personRepository.GetByIdAsync(personId.Value)
                : CreateNewPerson();

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

            if (Person.Id == 0)
            {
                Person.FirstName = "";
                Person.LastName = "";
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }

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

        private async void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete person {Person.FirstName} {Person.LastName}?", "Question");
            if (result == MessageDialogResult.OK)
            {
                _personRepository.Remove(Person.Model);
                await _personRepository.SaveAsync();
                _eventAggregator.GetEvent<AfterPersonDeletedEvent>()
                    .Publish(Person.Id);
            }
        }

        private Person CreateNewPerson()
        {
            var person = new Person();
            _personRepository.Add(person);
            return person;
        }
    }
}
