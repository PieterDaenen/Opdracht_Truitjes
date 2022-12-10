using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for WindowBestellingSelecteerTruitje.xaml
    /// </summary>
    public partial class WindowBestellingSelecteerTruitje : Window
    {
        private ObservableCollection<string> competities;
        private ObservableCollection<string> clubs;
        private ClubManager clubManager;
        private TruitjeManager truitjeManager;
        private BestellingManager bestellingManager;
        public Truitje Voetbaltruitje;
        private ObservableCollection<Truitje> voetbaltruitjes;
        public WindowBestellingSelecteerTruitje()
        {
            InitializeComponent();
            List<string> maten = Enum.GetNames(typeof(KledingMaat)).ToList();
            maten.Insert(0, "<alles>");
            MaatComboBox.ItemsSource = maten;
            MaatComboBox.SelectedIndex = 0;
            clubManager = new ClubManager(new ClubRepository(ConfigurationManager.ConnectionStrings["VerkoopDBConnection"].ToString(), "2022-2023"));
            competities = new ObservableCollection<string>(clubManager.GeefCompetities());
            truitjeManager = new TruitjeManager(new TruitjeRepository(ConfigurationManager.ConnectionStrings["VerkoopDBConnection"].ToString()));
            bestellingManager = new BestellingManager(new BestellingRepository(ConfigurationManager.ConnectionStrings["VerkoopDBConnection"].ToString()));
            competities.Insert(0, "<geen competitie>");
            CompetitieComboBox.ItemsSource = competities;
            CompetitieComboBox.SelectedIndex = 0;
        }

        private void SelecteerTruitje_Click(object sender, RoutedEventArgs e)
        {
            Voetbaltruitje = (Truitje)VoetbaltruitjesSelectie.SelectedItem;
            DialogResult = true;
            Close();
        }

        private void Voetbaltruitjes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VoetbaltruitjesSelectie.SelectedIndex >= 0) SelecteerTruitjeButton.IsEnabled = true;
            else SelecteerTruitjeButton.IsEnabled = false;
        }

        private void Zoek_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int? id = null;
                if (!string.IsNullOrWhiteSpace(IdTextBox.Text) && int.TryParse(IdTextBox.Text, out int i))
                {
                    id = i;
                }

                string competitie = null;
                if (CompetitieComboBox.SelectedIndex > 0) competitie = CompetitieComboBox.SelectedValue.ToString();

                string club = null;
                if (ClubComboBox.SelectedIndex > 0) club = ClubComboBox.SelectedValue.ToString();

                string seizoen = null;
                if (!string.IsNullOrWhiteSpace(SeizoenTextBox.Text)) seizoen = SeizoenTextBox.Text;

                string kledingmaat = null;
                if (MaatComboBox.SelectedIndex > 0) kledingmaat = MaatComboBox.SelectedValue.ToString();

                int? versie = null;
                if (!string.IsNullOrWhiteSpace(VersieTextBox.Text) && int.TryParse(VersieTextBox.Text, out int v))
                {
                    versie = v;
                }

                bool? thuis;
                if (ThuisCheckBox.IsChecked == true) thuis = true;
                else if (UitCheckBox.IsChecked == true) thuis = false;
                else thuis = null;


                double? prijs = null;
                if (!string.IsNullOrWhiteSpace(PrijsTextBox.Text) && double.TryParse(PrijsTextBox.Text, out double p))
                {
                    prijs = p;
                }

                voetbaltruitjes = new ObservableCollection<Truitje>(truitjeManager.ZoekTruitjes(id, competitie, club, seizoen, kledingmaat, versie, thuis, prijs));
                VoetbaltruitjesSelectie.ItemsSource = voetbaltruitjes;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Geef een zoekterm in");
            }
            
        }

        private void UitCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (ThuisCheckBox.IsChecked == true) ThuisCheckBox.IsChecked = false; 
        }

        private void ThuisCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (UitCheckBox.IsChecked == true) UitCheckBox.IsChecked = false;
        }

        private void CompetitieComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CompetitieComboBox.SelectedIndex != 0)
            {
                clubs = new ObservableCollection<string>(
                    clubManager.GeefClubs(CompetitieComboBox.SelectedItem.ToString()));
                clubs.Insert(0, "<geen club>");
                ClubComboBox.ItemsSource = clubs;
                ClubComboBox.SelectedIndex = 0;
            }
            else
                ClubComboBox.ItemsSource = null;
        }
        private void MenuItemDelete_Click(object sender, RoutedEventArgs e)
        {
            Truitje tVerwijderen = (Truitje)VoetbaltruitjesSelectie.SelectedItem;
            if (!bestellingManager.ZoekTruitjeInBestellingen(tVerwijderen)) truitjeManager.VerwijderVoetbaltruitje(tVerwijderen);
            else MessageBox.Show("Dit truitje wordt nog gebruikt in actieve bestellingen");
            Zoek_Click(sender, e);

        }

        private void MenuItemUpdate_Click(object sender, RoutedEventArgs e)
        {
            WindowUpdateTruitje w = new WindowUpdateTruitje((Truitje)VoetbaltruitjesSelectie.SelectedItem, truitjeManager);
            if (w.ShowDialog() == true)
            {
                Close();
            }

        }
        private void MenuItemNew_Click(object sender, RoutedEventArgs e)
        {
            WindowUpdateTruitje w = new WindowUpdateTruitje(null, truitjeManager);
            if (w.ShowDialog() == true)
            {

            }

        }
    }
}