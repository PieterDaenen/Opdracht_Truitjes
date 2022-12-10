using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace TruitjesUI
{
    /// <summary>
    /// Interaction logic for WindowUpdateBestelling.xaml
    /// </summary>
    public partial class WindowUpdateBestelling : Window
    {
        private BestellingManager bestellingManager;
        private KlantManager klantManager;
        private Bestelling bestelling;
        private Klant nieuweKlant;
        private ObservableCollection<TruitjeData> truitjes = new ObservableCollection<TruitjeData>();

        private Dictionary<Truitje, int> truitjesOrigineel;
        private Dictionary<Truitje, int> truitjesToevoegen;
        private Dictionary<Truitje, int> truitjesVerwijderen;

        public WindowUpdateBestelling(Bestelling? b, BestellingManager bm, KlantManager km)
        {
            InitializeComponent();
            bestellingManager = bm;
            if (b != null)
            {
                DataGridTextColumn c1 = new DataGridTextColumn();
                c1.Header = "Truitje";
                c1.IsReadOnly = true;
                c1.Binding = new Binding("Truitje");
                c1.Width = 100;
                dgTruitjes.Columns.Add(c1);
                DataGridTextColumn c2 = new DataGridTextColumn();
                c2.Header = "Aantal";
                c2.IsReadOnly = false;
                c2.Binding = new Binding("Aantal");
                c2.Width = 100;
                dgTruitjes.Columns.Add(c2);
                dgTruitjes.AutoGenerateColumns = false;
                dgTruitjes.ItemsSource = truitjes;

                bestelling = b;
                truitjesOrigineel = bestelling.GetTruitjes();
                truitjesToevoegen = new Dictionary<Truitje, int>();
                truitjesVerwijderen = new Dictionary<Truitje, int>();

                tbBestellingNr.Text = b.BestellingNr.ToString();
                tbKlant.Text = b.Klant.Naam;
                tbTijdstip.Text = b.Tijdstip.ToString();
                tbPrijs.Text = b.Prijs.ToString();
                if (b.Betaald == true) rbBetaaldJa.IsChecked = true;
                else rbBetaaldNee.IsChecked = true;
                foreach (var kvp in bestelling.GetTruitjes())
                {
                    TruitjeData td = new TruitjeData(kvp.Key, kvp.Value);
                    td.PropertyChanged += TruitjeData_PropertyChanged;
                    truitjes.Add(td);
                }
            }
        }

        private void btnSaveBestelling_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int teGebruikenBestellingNr = 0;
                if (!string.IsNullOrWhiteSpace(tbBestellingNr.Text) && int.TryParse(tbBestellingNr.Text, out int i))
                {
                    teGebruikenBestellingNr = i;
                } else
                {
                    MessageBox.Show("BestellingNr is foutief");
                }

                Klant teGebruikenKlant;
                if (nieuweKlant != null)
                {
                    teGebruikenKlant = nieuweKlant;
                } else
                {
                    teGebruikenKlant = bestelling.Klant;
                }

                bool teGebruikenBetaald = false;
                if (rbBetaaldJa.IsChecked == true) teGebruikenBetaald = true;

                foreach (var kvp in truitjesVerwijderen)
                {
                    truitjesOrigineel.Remove(kvp.Key, out int gelukt);
                }
                foreach (var kvp in truitjesToevoegen)
                {
                    truitjesOrigineel.Add(kvp.Key, kvp.Value);
                }

                Bestelling b = new Bestelling(truitjesOrigineel, teGebruikenBestellingNr, bestelling.Tijdstip, 0.0, teGebruikenKlant, teGebruikenBetaald);
                b.ZetPrijs(b.BerekenPrijs());
                bestellingManager.UpdateBestelling(b);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Vul alle data in");
            }

        }

        private void btnSelecteerTruitje_Click(object sender, RoutedEventArgs e)
        {

            WindowBestellingSelecteerTruitje w = new WindowBestellingSelecteerTruitje();
            if (w.ShowDialog() == true)
            {
                if (bestelling.Betaald == false)
                {
                    bestelling.VoegTruitjesToe(w.Voetbaltruitje, 1);
                    TruitjeData td = new TruitjeData(w.Voetbaltruitje, 1);
                    td.PropertyChanged += TruitjeData_PropertyChanged;
                    truitjes.Add(td);
                    tbPrijs.Text = bestelling.BerekenPrijs().ToString();
                }
                else
                {
                    MessageBox.Show("De truitjes van een betaalde bestelling kunnen niet worden aangepast");
                }
            }
        }

        private void btnVeranderKlant_Click(object sender, RoutedEventArgs e)
        {
            WindowBestellingSelecteerKlant w = new WindowBestellingSelecteerKlant();
            if (w.ShowDialog() == true)
            {
                nieuweKlant = w.Klant;
                tbKlant.Text = nieuweKlant.Naam;
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
            tbPrijs.Text = bestelling.BerekenPrijs().ToString();
            //CheckBestelling();
        }
        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Truitje tVerwijderen = (Truitje)dgTruitjes.SelectedItem;
            //int aantal = truitjesOrigineel.TryGetValue(tVerwijderen);

            truitjesVerwijderen.Add(tVerwijderen, 1);
            dgTruitjes.Items.Remove(tVerwijderen);


        }
    }
}
