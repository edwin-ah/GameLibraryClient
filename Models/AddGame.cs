using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibraryClient.Models
{
    // This class is for sending a new game to Api
    public class AddGame
    {
        public string Identifier { get; set; }
        public Game Game { get; set; }
    }
}
