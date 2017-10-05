using MahApps.Metro.Controls;
using Organizer.UI.ViewModel;
using System.Windows;

namespace Organizer.UI
{

    public partial class MainWindow : MetroWindow
    {
        private readonly MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}
