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
    public class AddGameViewModel : ViewModelBase
    {
        private string name;

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        private string company;

        public string Company
        {
            get => company;
            set => SetProperty(ref company, value);
        }

        private string price;

        public string Price
        {
            get => price;
            set => SetProperty(ref price, value);
        }

        private bool hasError;

        public bool HasError
        {
            get => hasError;
            set => SetProperty(ref hasError, value);
        }

        public ICommand StoreGameCommand { get; }

        public AddGameViewModel()
        {
            StoreGameCommand = new AsyncCommand(StoreGame);
        }

        public async Task StoreGame()
        {
            try
            {
                bool isPriceValid = await ValidatePrice();
                Game game = CreateGame();
                if(!isPriceValid || game == null)
                {
                    HasError = true;
                    return;
                }
                HasError = false;

                AddGame newGame = new AddGame()
                {
                    Identifier = Guid.NewGuid().ToString(),
                    Game = game
                };

                HttpClient client = new HttpClient();
                string jsonString = JsonConvert.SerializeObject(newGame, Formatting.Indented);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                string url = BASE_URL + "Games";
                var response = await client.PostAsync(url, content);

                if(response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    await DisplayDialogMessage("The game was created!");
                }
                else
                {
                    await DisplayDialogMessage("Could not create the game.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                await DisplayDialogMessage("An error occured. Could not add game.");
            }
            
        }

        private async Task<bool> ValidatePrice()
        {
            if(double.Parse(Price) <= 0)
            {
                var dialog = new MessageDialog("Price can not be zero or less.");
                await dialog.ShowAsync();
                return false;
            }
            return true;
        }

        private Game CreateGame()
        {
            if(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Company))
            {
                return null;
            }

            Game game = new Game()
            {
                Name = Name,
                Company = Company,
                Price = double.Parse(Price)
            };
            return game;
        }

        private async Task DisplayDialogMessage(string message)
        {
            var dialog = new MessageDialog(message);
            await dialog.ShowAsync();
        }
    }
}
