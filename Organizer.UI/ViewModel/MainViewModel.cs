using System.Threading.Tasks;

namespace Organizer.UI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(INavigationViewModel navigationViewModel,
            IPersonDetailViewModel personDetailViewModel)
        {
            NavigationViewModel = navigationViewModel;
            PersonDetailViewModel = personDetailViewModel;
        }


        public async Task LoadAsync()
        {
            await NavigationViewModel.LoadAsync();
        }

        public INavigationViewModel NavigationViewModel { get; }
        public IPersonDetailViewModel PersonDetailViewModel { get; }
    }
}
