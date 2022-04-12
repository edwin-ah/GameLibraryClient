using GameLibraryClient.Models;
using MvvmHelpers;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace GameLibraryClient.ViewModels
{
    public class GameListViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Game> Games { get; set; } = new ObservableRangeCollection<Game>();

        private string gameName = "";

        public string GameName
        {
            get => gameName;
            set => SetProperty(ref gameName, value);
        }
        private string gameCompany = "";

        public string GameCompany
        {
            get => gameCompany;
            set => SetProperty(ref gameCompany, value);
        }

        public ICommand GetAllGamesCommand { get; }
        public ICommand GetFilteredGamesCommand { get; }

        public GameListViewModel()
        {
            GetAllGamesCommand = new AsyncCommand(GetAllGames);
            GetFilteredGamesCommand = new AsyncCommand(GetFilteredGames);
        }

        public async Task GetAllGames()
        {
            try
            {
                string url = BASE_URL + "Games";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(new Uri(url));

                if(response.IsSuccessStatusCode)
                {
                    Games.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    var gameList = JsonConvert.DeserializeObject<List<Game>>(content);
                    Games.AddRange(gameList);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                var dialog = new MessageDialog("An error occured. No games could be fetched.");
                await dialog.ShowAsync();
            }
        }

        public async Task GetFilteredGames()
        {
            try
            {
                string url = BASE_URL + $"Games/ListGames?name={GameName}&company={GameCompany}";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(new Uri(url));

                if(response.IsSuccessStatusCode)
                {
                    Games.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    var gameList = JsonConvert.DeserializeObject<List<Game>>(content);
                    Games.AddRange(gameList);
                }
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                var dialog = new MessageDialog("An error occured. No games could be fetched.");
                await dialog.ShowAsync();
            }
        }
    }
}
