using GameLibraryClient.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace GameLibraryClient.ViewModels
{
    public class LoadGameViewModel : ViewModelBase
    {
        private Game game;

        public Game Game
        {
            get => game; 
            set => SetProperty(ref game, value);
        }

        public string GameIdentifier { get; set; }

        public async Task LoadGame()
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = BASE_URL + $"Games/{GameIdentifier}";
                HttpResponseMessage response = await client.GetAsync(new Uri(url));

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Game = JsonConvert.DeserializeObject<Game>(content);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                var dialog = new MessageDialog("An error occured. Could not load game.");
                await dialog.ShowAsync();
            }
        }

    }
}
