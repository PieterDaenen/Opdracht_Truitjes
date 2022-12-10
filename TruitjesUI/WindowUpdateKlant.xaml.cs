using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VerkoopTruithesBL.Managers;
using VerkoopTruithesBL.Model;

namespace TruitjesUI
{
    /// <summary>
    /// Interaction logic for WindowUpdateKlant.xaml
    /// </summary>
    public partial class WindowUpdateKlant : Window
    {
        private Klant Klant;
        private KlantManager klantManager;
        public WindowUpdateKlant(Klant? klant, KlantManager km)
        {
            InitializeComponent();
            klantManager = km;
            Klant= klant;
            if (klant != null)
            {
                tbNaam.Text = klant.Naam;
                tbAdres.Text = klant.Adres;
                tbKlantNr.Text = klant.KlantNr.ToString();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Klant nieuweKlant = new Klant(tbNaam.Text, tbAdres.Text);
            if (string.IsNullOrEmpty(tbKlantNr.Text)) {
                klantManager.KlantToevoegen(nieuweKlant);
                DialogResult = true;
                Close();
            } 
            else
            {
                nieuweKlant.ZetKlantNr(int.Parse(tbKlantNr.Text));
                klantManager.KlantUpdaten(nieuweKlant);
                DialogResult = true;
                Close();
            }
        }
    }
}
