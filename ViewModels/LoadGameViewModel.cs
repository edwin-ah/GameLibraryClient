using GameLibraryClient.Models;
using MvvmHelpers.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
                else
                {
                    System.Diagnostics.Debug.WriteLine(response.StatusCode);
                    await DisplayDialogMessage("Something went wrong. Could not load game.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await DisplayDialogMessage("Something went wrong. Could not load game.");
            }
        }

        public async Task<bool> DeleteGame()
        {
            try
            {
                HttpClient client = new HttpClient();
                string url = BASE_URL + $"Games/{GameIdentifier}";
                HttpResponseMessage response = await client.DeleteAsync(new Uri(url));
                if(!response.IsSuccessStatusCode)
                {
                    return false;
                }

                return true;
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        private async Task DisplayDialogMessage(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }
    }
}
