using Organizer.Model;
using Organizer.UI.Data.Lookups;
using Organizer.UI.Data.Repositories;
using Organizer.UI.Event;
using Organizer.UI.View.Services;
using Organizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Organizer.UI.ViewModel
{
    public class PersonDetailViewModel : DetailViewModelBase, IPersonDetailViewModel
    {
        private readonly IPersonRepository _personRepository;
        private readonly IProgrammingLanguageDataService _programmingLanguageDataService;

        public PersonDetailViewModel(IPersonRepository personRepository,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IProgrammingLanguageDataService programmingLanguageDataService) : base(eventAggregator, messageDialogService)
        {
            _personRepository = personRepository;
            _programmingLanguageDataService = programmingLanguageDataService;

            eventAggregator.GetEvent<AfterCollectionSavedEvent>()
                .Subscribe(AfterCollectionSaved);

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

        public override async Task LoadAsync(int personId)
        {
            var person = personId > 0 ?
                await _personRepository.GetByIdAsync(personId)
                : CreateNewPerson();

            Id = personId;

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
                if (e.PropertyName == nameof(Person.FirstName)
                || e.PropertyName == nameof(Person.LastName))
                {
                    SetTitle();
                }
            };

            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Person.Id == 0)
            {
                Person.FirstName = "";
                Person.LastName = "";
            }
            SetTitle();
        }

        private void SetTitle()
        {
            Title = $"{Person.FirstName} {Person.LastName}";
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

        public ICommand AddPhoneNumberCommand { get; }
        public ICommand RemovePhoneNumberCommand { get; }
        public ObservableCollection<LookupItem> ProgrammingLanguages { get; }
        public ObservableCollection<PersonPhoneNumberWrapper> PhoneNumbers { get; }

        protected override async void OnSaveExecute()
        {
            await SaveWithOptimisticConcurrencyAsync(_personRepository.SaveAsync,
                () =>
                {
                    HasChanges = _personRepository.HasChanges();
                    Id = Person.Id;
                    RaiseDetailSavedEvent(Person.Id, $"{Person.FirstName} {Person.LastName}");
                });
        }

        protected override bool OnSaveCanExecute()
        {
            return Person != null
                && Person.HasErrors == false
                && PhoneNumbers.All(n => !n.HasErrors)
                && HasChanges;
        }

        protected override async void OnDeleteExecute()
        {
            if (await _personRepository.HasMeetingsAsync(Person.Id))
            {
                MessageDialogService.ShowInfoDialog($"{Person.FirstName} {Person.LastName} can't be deleted, as this person is part of at least one meeting");
                return;
            }
            var result = MessageDialogService.ShowOkCancelDialog($"Do you really want to delete person {Person.FirstName} {Person.LastName}?", "Question");
            if (result == MessageDialogResult.OK)
            {
                _personRepository.Remove(Person.Model);
                await _personRepository.SaveAsync();
                RaiseDetailDeletedEvent(Person.Id);
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

        private async void AfterCollectionSaved(AfterCollectionSavedEventArgs args)
        {
            if (args.ViewModelName == nameof(ProgrammingLanguageDetailViewModel))
            {
                await LoadProgrammingLanguagesAsync();
            }
        }
    }
}
