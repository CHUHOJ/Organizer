using Organizer.UI.Event;
using Organizer.UI.View.Services;
using Prism.Events;
using System;
using System.Threading.Tasks;

namespace Organizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly Func<IPersonDetailViewModel> _personDetailViewModelCreator;
        private readonly IMessageDialogService _messageDialogService;

        public MainViewModel(INavigationViewModel navigationViewModel,
            Func<IPersonDetailViewModel> personDetailViewModelCreator,
            IEventAggregator eventAggregator,
            IMessageDialogService messageDialogService)
        {
            _eventAggregator = eventAggregator;
            _personDetailViewModelCreator = personDetailViewModelCreator;
            _messageDialogService = messageDialogService;
            _eventAggregator.GetEvent<OpenPersonDetailViewEvent>()
                .Subscribe(OnOpenPersonDetailView);

            NavigationViewModel = navigationViewModel;
        }


        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public INavigationViewModel NavigationViewModel { get; }

        private IPersonDetailViewModel _personDetailViewModel;
        public IPersonDetailViewModel PersonDetailViewModel
        {
            get { return _personDetailViewModel; }
            private set
            {
                _personDetailViewModel = value;
                OnPropertyChanged();
            }
        }


        private async void OnOpenPersonDetailView(int personId)
        {
            if (PersonDetailViewModel != null && PersonDetailViewModel.HasChanges)
            {
                var result = _messageDialogService.ShowOkCancelDialog("You've made changes. Navigate away?", "Question");
                if (result == MessageDialogResult.Cancel)
                {
                    return;
                }
            }
            PersonDetailViewModel = _personDetailViewModelCreator();
            await PersonDetailViewModel.LoadAsync(personId);
        }
    }
}
