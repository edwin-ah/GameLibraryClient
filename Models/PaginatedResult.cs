using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibraryClient.Models
{
    public class PaginatedResult
    {
        public Game[] Games { get; set; }
        public int CurrentPage { get; set; }
        public int Pages { get; set; }
    }
}
