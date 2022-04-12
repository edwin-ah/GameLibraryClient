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

        public bool HasError { get; set; }

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
                if(!isPriceValid)
                {
                    return;
                }

                AddGame game = new AddGame()
                {
                    Identifier = Guid.NewGuid().ToString(),
                    Game = CreateGame()
                };

                HttpClient client = new HttpClient();
                string jsonString = JsonConvert.SerializeObject(game, Formatting.Indented);
                var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                string url = BASE_URL + "Games";
                var response = await client.PostAsync(url, content);

                if(response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    var dialog = new MessageDialog("The game was created!");
                    await dialog.ShowAsync();
                }
                else
                {
                    var dialog = new MessageDialog("Could not create the game.");
                    await dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                var dialog = new MessageDialog("An error occured. Could not add game.");
                await dialog.ShowAsync();
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
            Game game = new Game()
            {
                Name = Name,
                Company = Company,
                Price = double.Parse(Price)
            };
            return game;
        }
    }
}
