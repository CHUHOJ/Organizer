using Organizer.UI.Data;
using System.Threading.Tasks;
using Organizer.Model;
using Prism.Events;
using Organizer.UI.Event;
using System;
using System.Windows.Input;
using Prism.Commands;

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

        private void OnSaveExecute()
        {
            _personDataService.SaveAsync(Person);
            _eventAggregator.GetEvent<AfterPersonSavedEvent>().Publish(
                new AfterPersonSavedEventArgs
                {
                    Id = Person.Id,
                    DisplayMember = $"{Person.FirstName} {Person.LastName}"
                });
        }

        private bool OnSaveCanExecute()
        {
            // TODO: check if person is valid
            return true;
        }

        private async void OnOpenPersonDetailView(int personId)
        {
            await LoadAsync(personId);
        }

        private Person _person;
        public Person Person
        {
            get { return _person; }
            set { _person = value; OnPropertyChanged(); }
        }

        public async Task LoadAsync(int personId)
        {
            Person = await _personDataService.GetByIdAsync(personId);
        }

        public ICommand SaveCommand { get; }
    }
}
