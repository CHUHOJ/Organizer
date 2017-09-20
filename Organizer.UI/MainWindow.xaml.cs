﻿using Organizer.UI.ViewModel;
using System.Windows;

namespace Organizer.UI
{

    public partial class MainWindow : Window
    {
        private readonly MainViewModel _viewModel;

        public MainWindow(MainViewModel viewModel)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.Load();
        }
    }
}