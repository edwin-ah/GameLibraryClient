using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameLibraryClient.Models
{
    public class Game
    {
        public string Identifier { get; set; }

        public string Name { get; set; }

        public string Company { get; set; }

        public double Price { get; set; }
    }
}
