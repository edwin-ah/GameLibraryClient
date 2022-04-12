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

        
        // On first call to Api we want the first page
        private int currentPage = 1;

        public int CurrentPage
        {
            get => currentPage;
            set => SetProperty(ref currentPage, value);
        }

        private bool hasNextPage;

        public bool HasNextPage
        {
            get => hasNextPage;
            set => SetProperty(ref hasNextPage, value);
        }

        private bool hasPrevPage;

        public bool HasPrevPage
        {
            get => hasPrevPage; 
            set => SetProperty(ref hasPrevPage, value);
        }

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
                string url = BASE_URL + $"Games/AllGames/{CurrentPage}";
                HttpClient client = new HttpClient();
                HttpResponseMessage response = await client.GetAsync(new Uri(url));

                if(response.IsSuccessStatusCode)
                {
                    Games.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    var paginatedResult = JsonConvert.DeserializeObject<PaginatedResult>(content);
                    Games.AddRange(paginatedResult.Games);
                    CurrentPage = paginatedResult.CurrentPage;
                    CheckPages(paginatedResult.Pages);
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
            // Reset current page and hide buttons
            CurrentPage = 1;
            HasNextPage = false;
            HasNextPage = false;

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

        private void CheckPages(int totalPages)
        {
            if(CurrentPage < totalPages)
            {
                HasNextPage = true;
            }
            else
            {
                HasNextPage= false;
            }
            if(CurrentPage > 1)
            {
                HasPrevPage = true;
            }
            else
            {
                HasPrevPage= false;
            }
        }
    }
}
