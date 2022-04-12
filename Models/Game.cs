using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace GameLibraryClient.Models
{
    public class Game : INotifyPropertyChanged
    {
        // public string Identifier { get; set; }
        private string identifier;

        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set 
            {
                name = value; 
                RaisePropertyChanged("Name");
            }
        }

        //public string Name { get; set; }
        private string company;

        public string Company
        {
            get { return company; }
            set { company = value; }
        }
        private double price;

        public double Price
        {
            get { return price; }
            set { price = value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
