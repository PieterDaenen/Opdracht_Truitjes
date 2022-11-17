using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Model;

namespace TruitjesUI
{
    public class TruitjeData : INotifyPropertyChanged
    {
        public TruitjeData(Truitje truitje, int aantal)
        {
            Truitje = truitje;
            Aantal = aantal;
        }
        public Truitje Truitje { get; set; }
        private int aantal;
        public int Aantal
        {
            get { return aantal; }
            set
            {
                aantal = value;
                OnPropertyChanged();
            }
        }
        private void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
