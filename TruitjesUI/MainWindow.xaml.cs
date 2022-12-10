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
using VerkoopTruithesBL.Exceptions;
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
        private KlantManager klantManager;
        private ObservableCollection<TruitjeData> truitjes = new ObservableCollection<TruitjeData>();
        private ObservableCollection<Bestelling> bestellingen = new ObservableCollection<Bestelling>();
        public MainWindow()
        {
            InitializeComponent();
            bestellingManager = new BestellingManager(new BestellingRepository(ConfigurationManager.ConnectionStrings["VerkoopDBConnection"].ToString()));
            klantManager = new KlantManager(new KlantRepository(ConfigurationManager.ConnectionStrings["VerkoopDBConnection"].ToString()));
            bestelling = new Bestelling(DateTime.Now);
            DataGridTextColumn c1 = new DataGridTextColumn();
            c1.Header = "Truitje";
            c1.IsReadOnly = true;
            c1.Binding = new Binding("Truitje");
            c1.Width = 100;
            BestellingTruitjes.Columns.Add(c1);
            DataGridTextColumn c2 = new DataGridTextColumn();
            c2.Header = "Aantal";
            c2.IsReadOnly = false;
            c2.Binding = new Binding("Aantal");
            c2.MaxWidth = 250;
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
            try
            {
            if (BetaaldCheckBox.IsChecked == true)
            {
                BetaaldCheckBox.IsChecked = false;
                bestelling.ZetBetaald();
            }
            bestellingManager.VoegBestellingToe(bestelling);
            KlantTextBox.Text = "";
            PrijsTextBox.Text = "";
            truitjes.Clear();

            }
            catch (BestellingManagerException ex)
            {
                MessageBox.Show("Vul alle data in");
            }
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

        private void ZoekBestellingAanpassen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? bestellingId = null;
                if (!string.IsNullOrWhiteSpace(BestellingIdAanpassenTextBox.Text) && int.TryParse(BestellingIdAanpassenTextBox.Text, out int i))
                {
                    bestellingId = i;
                }

                int? klantId = null;
                if (!string.IsNullOrWhiteSpace(KlantAanpassenTextBox.Text))
                {
                    string[] gesplitst = KlantAanpassenTextBox.Text.Split(',');
                    string k = gesplitst[0];
                    klantId = int.Parse(k);
                }

                DateTime? startDatum = null;
                if (StartdatumDatePicker.SelectedDate.HasValue)
                {
                    startDatum = StartdatumDatePicker.SelectedDate.Value;
                }

                DateTime? eindDatum = null;
                if (EinddatumDatePicker.SelectedDate.HasValue)
                {
                    eindDatum = EinddatumDatePicker.SelectedDate.Value;
                }
                
                BestellingenAanpassen.ItemsSource =  bestellingManager.ZoekBestellingen(bestellingId, klantId, startDatum, eindDatum);

                foreach (Bestelling b in bestellingen)
                {
                    BestellingenAanpassen.Items.Add(b);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vul alle data in");
            }
        }

        private void SelecteerKlantAanpassen_Click(object sender, RoutedEventArgs e)
        {
            WindowBestellingSelecteerKlant w = new WindowBestellingSelecteerKlant();
            if (w.ShowDialog() == true)
            {
                KlantAanpassenTextBox.Text = w.Klant.ToString();
            }
        }

        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Bestelling bVerwijderen = (Bestelling)BestellingenAanpassen.SelectedItem;
            bestellingManager.VerwijderBestelling(bVerwijderen);
            BestellingenAanpassen.ItemsSource = bestellingen;
        }

        private void MenuItemUpdate_Click(object sender, RoutedEventArgs e)
        {
            WindowUpdateBestelling w = new WindowUpdateBestelling((Bestelling)BestellingenAanpassen.SelectedItem, bestellingManager, klantManager);
            if (w.ShowDialog() == true)
            {

            }
        }
        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            WindowUpdateBestelling w = new WindowUpdateBestelling(null, bestellingManager, klantManager);
            if (w.ShowDialog() == true)
            {

            }

        }
    }
}
