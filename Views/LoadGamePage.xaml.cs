using GameLibraryClient.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace GameLibraryClient.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadGamePage : Page
    {
        public LoadGameViewModel vm;
        public LoadGamePage()
        {
            this.InitializeComponent();
            vm = new LoadGameViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if(e.Parameter is string && !string.IsNullOrWhiteSpace((string)e.Parameter))
            {
                vm.GameIdentifier = (string)e.Parameter;
                await vm.LoadGame();
                DataContext = vm;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Wrong type was passed as identifier: " + e.Parameter.GetType());
                var dialog = new MessageDialog("An error occured. Can not load game.");
                await dialog.ShowAsync();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.TryGoBack();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddGamePage));
        }
    }
}
