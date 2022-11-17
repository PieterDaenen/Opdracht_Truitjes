using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VerkoopTruithesBL.Managers;
using VerkoopTruithesBL.Model;
using VerkoopTruitjesDL.Repositories;

namespace TruitjesUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bestelling bestelling;
        private BestellingManager bestellingManager;
        private ObservableCollection<TruitjeData> truitjes = new ObservableCollection<TruitjeData>();
        public MainWindow()
        {
            InitializeComponent();
            bestellingManager = new BestellingManager(new BestellingRepository(ConfigurationManager.ConnectionStrings["VerkoopDBConnection"].ToString()));
            bestelling = new Bestelling(DateTime.Now);
            DataGridTextColumn c1 = new DataGridTextColumn();
            c1.Header = "Truitje";
            c1.IsReadOnly = true;
            c1.Binding = new Binding("Truitje");
            BestellingTruitjes.Columns.Add(c1);
            DataGridTextColumn c2 = new DataGridTextColumn();
            c2.Header = "Aantal";
            c2.IsReadOnly = false;
            c2.Binding = new Binding("Aantal");
            BestellingTruitjes.Columns.Add(c2);
            BestellingTruitjes.AutoGenerateColumns = false;
            BestellingTruitjes.ItemsSource = truitjes;
        }

        private void SelecteerKlant_Click(object sender, RoutedEventArgs e)
        {
            WindowBestellingSelecteerKlant w = new WindowBestellingSelecteerKlant();
            if (w.ShowDialog() == true)
            {
                bestelling.ZetKlant(w.Klant);
                PrijsTextBox.Text = bestelling.BerekenPrijs().ToString();
                KlantTextBox.Text = w.Klant.ToString();
                //CheckBestelling();
            }
        }

        private void SelecteerTruitje_Click(object sender, RoutedEventArgs e)
        {
            WindowBestellingSelecteerTruitje w = new WindowBestellingSelecteerTruitje();
            if (w.ShowDialog() == true)
            {
                bestelling.VoegTruitjesToe(w.Voetbaltruitje, 1);
                TruitjeData td = new TruitjeData(w.Voetbaltruitje, 1);
                td.PropertyChanged += TruitjeData_PropertyChanged;
                truitjes.Add(td);
                //CheckBestelling();
                PrijsTextBox.Text = bestelling.BerekenPrijs().ToString();
            }
        }

        private void PlaatsBestelling_Click(object sender, RoutedEventArgs e)
        {
            //bestellingManager.VoegBestellingToe(bestelling);

        }

        private void SelecteerKlantAanpassen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ZoekBestellingAanpassen_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MenuItemUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
        private void TruitjeData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            TruitjeData t = (TruitjeData)sender;
            int delta = t.Aantal - bestelling.GetTruitjes()[t.Truitje];
            if (delta < 0)
            {
                bestelling.VerwijderTruitjes(t.Truitje, Math.Abs(delta));
                if (!bestelling.GetTruitjes().ContainsKey(t.Truitje)) truitjes.Remove(t);
            }
            if (delta > 0)
            {
                bestelling.VoegTruitjesToe(t.Truitje, delta);
            }
            PrijsTextBox.Text = bestelling.BerekenPrijs().ToString();
            //CheckBestelling();
        }
    }
}
