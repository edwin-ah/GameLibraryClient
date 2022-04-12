using GameLibraryClient.Models;
using GameLibraryClient.ViewModels;
using GameLibraryClient.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GameLibraryClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        GameListViewModel vm;
        public MainPage()
        {
            this.InitializeComponent();
            vm = new GameListViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await vm.GetAllGames();
        }

        private void list_Games_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Game game = (Game)list_Games.SelectedItem;
            string gameId = game.Identifier;
            this.Frame.Navigate(typeof(LoadGamePage), gameId);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            App.TryGoBack();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddGamePage));
        }

        private async void NextButton_Click(object sender, RoutedEventArgs e)
        {
            vm.CurrentPage++;
            await vm.GetAllGames();
        }

        private async void PrevButton_Click(object sender, RoutedEventArgs e)
        {
            vm.CurrentPage--;
            await vm.GetAllGames();
        }
    }
}
