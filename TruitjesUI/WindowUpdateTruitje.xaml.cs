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
    /// Interaction logic for WindowUpdateTruitje.xaml
    /// </summary>
    public partial class WindowUpdateTruitje : Window
    {
        private Truitje Truitje;
        private TruitjeManager truitjeManager;
        private ClubManager clubManager;
        private string seizoen = "2022-2023";
        public WindowUpdateTruitje(Truitje? truitje, TruitjeManager tm)
        {
            InitializeComponent();

            if (truitje != null) seizoen = truitje.Seizoen;
            clubManager = new ClubManager(new ClubRepository(ConfigurationManager.ConnectionStrings["VerkoopDBConnection"].ToString(), seizoen));

            truitjeManager = tm;
            Truitje = truitje;
                List<string> maten = Enum.GetNames(typeof(KledingMaat)).ToList();
                maten.Insert(0, "<alles>");
                cbKledingMaat.ItemsSource = maten;

                List<string> competities = clubManager.GeefCompetities().ToList();
                cbCompetitie.ItemsSource = competities;

            if (truitje != null)
                {
                tbId.Text = Truitje.Id.ToString();
                tbVersie.Text = Truitje.ClubSet.Versie.ToString();
                tbPrijs.Text = Truitje.Prijs.ToString();

                if (Truitje.ClubSet.Uit == true) rbUitUit.IsChecked = true;
                else rbUitThuis.IsChecked = true;

                cbKledingMaat.SelectedItem = Truitje.KledingMaat.ToString();
                cbCompetitie.SelectedItem = Truitje.Club.Competitie.ToString();

                List<string> clubs = clubManager.GeefClubs(Truitje.Club.Competitie).ToList();
                cbClub.ItemsSource = clubs;
                cbClub.SelectedItem = Truitje.Club.Ploegnaam;
            } 
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                    Truitje nieuwTruitje = new Truitje();
                    nieuwTruitje.ZetSeizoen(seizoen);
                    nieuwTruitje.ZetClub(new Club(cbCompetitie.SelectedItem.ToString(), cbClub.SelectedItem.ToString()));
                    nieuwTruitje.ZetPrijs(double.Parse(tbPrijs.Text));
                    var x = cbKledingMaat.SelectedItem;
                    nieuwTruitje.KledingMaat = (KledingMaat)Enum.Parse(typeof(KledingMaat), cbKledingMaat.SelectedValue.ToString());
                    int ntVersie = int.Parse(tbVersie.Text);
                    bool thuis;
                    if (rbUitThuis.IsChecked == true) thuis = false;
                    else thuis = true;
                    nieuwTruitje.ZetClubSet(new ClubSet(thuis, ntVersie));
                if (string.IsNullOrEmpty(tbId.Text))
                {
                    truitjeManager.VoegTruitjeToe(nieuwTruitje);
                    DialogResult = true;
                    Close();
                } else
                {
                    nieuwTruitje.ZetId(int.Parse(tbId.Text));
                    truitjeManager.UpdateVoetbaltruitje(nieuwTruitje);
                    DialogResult = true;
                    Close();
                }
            }
            catch (Exception ex)
            {
                    MessageBox.Show("Er ging iets mis");
            }
        }

        private void cbCompetitie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbClub.ItemsSource = clubManager.GeefClubs(cbCompetitie.SelectedItem.ToString());
        }
    }
}
