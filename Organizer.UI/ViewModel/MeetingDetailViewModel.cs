using Organizer.Model;
using Organizer.UI.Data.Repositories;
using Organizer.UI.View.Services;
using Organizer.UI.Wrapper;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Generic;
using System.Linq;

namespace Organizer.UI.ViewModel
{
    public class MeetingDetailViewModel : DetailViewModelBase, IMeetingDetailViewModel
    {
        private readonly IMeetingRepository _meetingRepository;
        private readonly IMessageDialogService _messageDialogService;
        private Person _selectedAvailablePerson;
        private Person _selectedAddedPerson;
        private MeetingWrapper _meeting;
        private List<Person> _allPersons;

        public MeetingDetailViewModel(IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService,
            IMeetingRepository meetingRepository) : base(eventAggregator)
        {
            _messageDialogService = messageDialogService;
            _meetingRepository = meetingRepository;

            AddedPersons = new ObservableCollection<Person>();
            AvailablePersons = new ObservableCollection<Person>();
            AddPersonCommand = new DelegateCommand(OnAddPersonExecute, OnAddPersonCanExecute);
            RemovePersonCommand = new DelegateCommand(OnRemovePersonExecute, OnRemovePersonCanExecute);
        }

        public MeetingWrapper Meeting
        {
            get { return _meeting; }
            set { _meeting = value; OnPropertyChanged(); }
        }

        public ObservableCollection<Person> AvailablePersons { get; }
        public ObservableCollection<Person> AddedPersons { get; }
        public ICommand AddPersonCommand { get; }
        public ICommand RemovePersonCommand { get; }


        public Person SelectedAvailablePerson
        {
            get { return _selectedAvailablePerson; }
            set
            {
                _selectedAvailablePerson = value;
                OnPropertyChanged();
                ((DelegateCommand)AddPersonCommand).RaiseCanExecuteChanged();
            }
        }
        public Person SelectedAddedPerson
        {
            get { return _selectedAddedPerson; }
            set
            {
                _selectedAddedPerson = value;
                OnPropertyChanged();
                ((DelegateCommand)RemovePersonCommand).RaiseCanExecuteChanged();
            }
        }

        public override async Task LoadAsync(int? meetingId)
        {
            var meeting = meetingId.HasValue
                ? await _meetingRepository.GetByIdAsync(meetingId.Value)
                : CreateNewMeeting();

            InitializeMeeting(meeting);

            _allPersons = await _meetingRepository.GetAllPersonsAsync();
            SetupPicklist();
        }

        private void SetupPicklist()
        {
            List<int> meetingPersonIds = Meeting.Model.Persons.Select(p => p.Id).ToList();
            var addedPersons = _allPersons.Where(x => meetingPersonIds.Contains(x.Id)).OrderBy(p => p.FirstName);
            var availablePersons = _allPersons.Except(addedPersons).OrderBy(p => p.FirstName);

            AddedPersons.Clear();
            AvailablePersons.Clear();
            foreach (var addedPerson in addedPersons)
            {
                AddedPersons.Add(addedPerson);
            }
            foreach (var availablePerson in availablePersons)
            {
                AvailablePersons.Add(availablePerson);
            }
        }

        protected override void OnDeleteExecute()
        {
            var result = _messageDialogService.ShowOkCancelDialog($"Do you really want to delete the meeting {Meeting.Title}?", "Question");
            if (result == MessageDialogResult.OK)
            {
                _meetingRepository.Remove(Meeting.Model);
                _meetingRepository.SaveAsync();
                RaiseDetailDeletedEvent(Meeting.Id);
            }
        }

        protected override bool OnSaveCanExecute()
        {
            return Meeting != null && !Meeting.HasErrors && HasChanges;
        }

        protected override async void OnSaveExecute()
        {
            await _meetingRepository.SaveAsync();
            HasChanges = _meetingRepository.HasChanges();
            RaiseDetailSavedEvent(Meeting.Id, Meeting.Title);
        }

        private void OnRemovePersonExecute()
        {
            Person personToRemove = SelectedAddedPerson;

            Meeting.Model.Persons.Remove(personToRemove);
            AddedPersons.Remove(personToRemove);
            AvailablePersons.Add(personToRemove);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnRemovePersonCanExecute()
        {
            return SelectedAddedPerson != null;
        }

        private void OnAddPersonExecute()
        {
            Person personToAdd = SelectedAvailablePerson;

            Meeting.Model.Persons.Add(personToAdd);
            AddedPersons.Add(personToAdd);
            AvailablePersons.Remove(personToAdd);
            HasChanges = _meetingRepository.HasChanges();
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
        }

        private bool OnAddPersonCanExecute()
        {
            return SelectedAvailablePerson != null;
        }

        private Meeting CreateNewMeeting()
        {
            var meeting = new Meeting
            {
                DateFrom = DateTime.Now.Date,
                DateTo = DateTime.Now.Date
            };
            _meetingRepository.Add(meeting);
            return meeting;
        }


        private void InitializeMeeting(Meeting meeting)
        {
            Meeting = new MeetingWrapper(meeting);
            Meeting.PropertyChanged += (s, e) =>
            {
                if ((!HasChanges))
                {
                    HasChanges = _meetingRepository.HasChanges();
                }

                if (e.PropertyName == nameof(Meeting.HasErrors))
                {
                    ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();
                }
            };
            ((DelegateCommand)SaveCommand).RaiseCanExecuteChanged();

            if (Meeting.Id == 0)
            {
                // to trigger the validation
                Meeting.Title = "";
            }
        }
    }
}
