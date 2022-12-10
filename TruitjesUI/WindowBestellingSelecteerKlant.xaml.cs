using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
using VerkoopTruitjesDL.Repositories;

namespace TruitjesUI
{
    /// <summary>
    /// Interaction logic for WindowSelecteerKlant.xaml
    /// </summary>
    public partial class WindowBestellingSelecteerKlant : Window
    {
        public Klant Klant { get; private set; }
        private KlantManager klantManager;
        public WindowBestellingSelecteerKlant()
        {
            InitializeComponent();
            klantManager = new KlantManager(new KlantRepository(ConfigurationManager.ConnectionStrings["VerkoopDBConnection"].ToString()));

        }

        private void KlantenListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KlantenListBox.SelectedIndex >= 0) SelecteerKlant.IsEnabled = true;
            else SelecteerKlant.IsEnabled = false;
        }

        private void SelecteerKlant_Click(object sender, RoutedEventArgs e)
        {
            Klant = (Klant)KlantenListBox.SelectedItem;
            DialogResult = true;
            Close();
        }

        private void ZoekKlant_Click(object sender, RoutedEventArgs e)
        {
            string naam = NaamTextBoxAanpassen.Text;
            string adres = AdresTextBoxAanpassen.Text;
            if (!string.IsNullOrWhiteSpace(KlantIdTextBoxAanpassen.Text))
            {
                int klantNr = int.Parse(KlantIdTextBoxAanpassen.Text);
                List<Klant> klanten = new List<Klant>(klantManager.ZoekKlant(klantNr, naam, adres));
                KlantenListBox.ItemsSource = klanten;
            } else
            {
                List<Klant> klanten = new List<Klant>(klantManager.ZoekKlant(null, naam, adres));
                KlantenListBox.ItemsSource = klanten;
            }
        }
        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Klant kVerwijderen = (Klant)KlantenListBox.SelectedItem;
            klantManager.KlantVerwijderen(kVerwijderen);
            ZoekKlant_Click(sender, e);
                
        }

        private void MenuItemUpdate_Click(object sender, RoutedEventArgs e)
        {
            WindowUpdateKlant w = new WindowUpdateKlant((Klant)KlantenListBox.SelectedItem, klantManager);
            if (w.ShowDialog() == true)
            {
                Close();
            }

        }
        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            WindowUpdateKlant w = new WindowUpdateKlant(null, klantManager);
            if (w.ShowDialog() == true)
            {

            }

        }
    }
}
