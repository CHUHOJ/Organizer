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
using Organizer.UI.Data.Lookups;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Organizer.UI.ViewModel
{
    public class PersonDetailViewModel : ViewModelBase, IPersonDetailViewModel
    {
        private readonly IPersonRepository _personRepository;
        private readonly IEventAggregator _eventAggregator;
        private readonly IMessageDialogService _messageDialogService;
        private readonly IProgrammingLanguageDataService _programmingLanguageDataService;

        public PersonDetailViewModel(IPersonRepository personRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgrammingLanguageDataService programmingLanguageDataService)
        {
            _personRepository = personRepository;
            _eventAggregator = eventAggregator;
            _messageDialogService = messageDialogService;
            _programmingLanguageDataService = programmingLanguageDataService;

            SaveCommand = new DelegateCommand(OnSaveExecute, OnSaveCanExecute);
            DeleteCommand = new DelegateCommand(OnDeleteExecute);
            AddPhoneNumberCommand = new DelegateCommand(OnAddPhoneNumberExecute);
            RemovePhoneNumberCommand = new DelegateCommand(OnRemovePhoneNumberExecute, OnRemovePhoneNumberCanExecute);

            ProgrammingLanguages = new ObservableCollection<LookupItem>();
            PhoneNumbers = new ObservableCollection<PersonPhoneNumberWrapper>();
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

            InitializePerson(person);
            InitializePersonPhoneNumbers(person.PhoneNumbers);

            await LoadProgrammingLanguagesAsync();
        }

        private void InitializePerson(Person person)
        {
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

        private void InitializePersonPhoneNumbers(ICollection<PersonPhoneNumber> phoneNumbers)
        {
            foreach (PersonPhoneNumberWrapper wrapper in PhoneNumbers)
            {
                wrapper.PropertyChanged -= PersonPhoneNumberWrapper_PropertyChanged;
            }
            PhoneNumbers.Clear();
            foreach (PersonPhoneNumber personPhoneNumber in phoneNumbers)
            {
                var wrapper = new PersonPhoneNumberWrapper(personPhoneNumber);
                PhoneNumbers.Add(wrapper);
                wrapper.PropertyChanged += PersonPhoneNumberWrapper_PropertyChanged;
            }
        }

        private void PersonPhoneNumberWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (!HasChanges)
            {
                HasChanges = _personRepository.HasChanges();
            }
            if (e.PropertyName == nameof(PersonPhoneNumberWrapper.HasErrors))
            {
                ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
            }
        }

        private async Task LoadProgrammingLanguagesAsync()
        {
            ProgrammingLanguages.Clear();
            ProgrammingLanguages.Add(new NullLookupItem { DisplayMember = " - " });
            var programmingLanguages = await _programmingLanguageDataService.GetProgrammingLanguageAsync();

            foreach (var item in programmingLanguages)
            {
                ProgrammingLanguages.Add(item);
            }
        }

        private PersonPhoneNumberWrapper _selectedPhoneNumber;
        public PersonPhoneNumberWrapper SelectedPhoneNumber
        {
            get { return _selectedPhoneNumber; }
            set
            {
                _selectedPhoneNumber = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePhoneNumberCommand).RaiseCanExecuteChanged();
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand AddPhoneNumberCommand { get; }
        public ICommand RemovePhoneNumberCommand { get; }
        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }
        public ObservableCollection<PersonPhoneNumberWrapper> PhoneNumbers { get; }

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
            return Person != null
                && Person.HasErrors == false
                && PhoneNumbers.All(n => !n.HasErrors)
                && HasChanges;
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

        private void OnAddPhoneNumberExecute()
        {
            var newNumber = new PersonPhoneNumberWrapper(new PersonPhoneNumber());
            newNumber.PropertyChanged += PersonPhoneNumberWrapper_PropertyChanged;
            PhoneNumbers.Add(newNumber);
            Person.Model.PhoneNumbers.Add(newNumber.Model);
            newNumber.Number = "";
        }

        private void OnRemovePhoneNumberExecute()
        {
            SelectedPhoneNumber.PropertyChanged -= PersonPhoneNumberWrapper_PropertyChanged;
            _personRepository.RemovePhoneNumber(SelectedPhoneNumber.Model);
            PhoneNumbers.Remove(SelectedPhoneNumber);
            SelectedPhoneNumber = null;
            HasChanges = _personRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemovePhoneNumberCanExecute()
        {
            return SelectedPhoneNumber != null;
        }

        private Person CreateNewPerson()
        {
            var person = new Person();
            _personRepository.Add(person);
            return person;
        }
    }
}
