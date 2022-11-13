using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Interfaces;
using VerkoopTruithesBL.Model;
using VerkoopTruitjesDL.Exceptions;

namespace VerkoopTruitjesDL.Repositories
{
    public class KlantRepository : IKlantRepository
    {
        private string connectionString;

        public KlantRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool BestaatKlant(Klant klant)
        {
            string query = "select count(*) from klant where klantNr=@klantNr;";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@klantNr", klant.KlantNr);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryException("BestaanKlant(klantnr)", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool BestaatKlant(int klantNr)
        {
            string query = "select count(*) from klant where klantNr=@klantNr;";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@klantNr", klantNr);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryException("BestaanKlant(klantnr)", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public Klant GeefKlant(int klantnr)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "SELECT t1.KlantNr,t1.Naam,t1.Adres,t2.BestellingNr,t2.Prijs prijsbestelling,t2.Betaald,t2.Tijdstip,"
                +"t3.TruitjeId,t3.Aantal,t4.Prijs prijstruitje, t4.maat,t5.Competitie,t5.Ploegnaam,t5.Seizoen,"
                +"t6.Uit,t6.Versie "
                +" FROM Klant t1 "
                +"left join Bestelling t2 on t1.KlantNr = t2.KlantNr "
                +"left join BestellingDetail t3 on t2.BestellingNr = t3.BestellingNr "
                +"left join Truitje t4 on t3.TruitjeId = t4.TruitjeId "
                +"left join club t5 on t4.ClubId = t5.ClubId "
                +"left join clubset t6 on t4.ClubsetId = t6.ClubSetId "
                +"where t1.klantnr = @klantnr "
                +"order by t2.BestellingNr";
            using(SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@klantnr", klantnr);
                    SqlDataReader reader=cmd.ExecuteReader();
                    Klant klant = null;
                    int bestellingNrOld = -1;
                    int bestellingnr=0;
                    bool betaald=false;
                    double prijs = 0.0;
                    double prijstruitje;
                    bool first = true;
                    bool uit;
                    int aantal;
                    int truitjeid;
                    int versie;
                    string competitie;
                    string ploegnaam;
                    string seizoen;
                    Dictionary<Truitje,int> truitjes=new Dictionary<Truitje,int>();
                    List<Bestelling> bestellingen=new List<Bestelling>();
                    KledingMaat maat;
                    DateTime tijdstip=DateTime.Now;
                    while(reader.Read())
                    {
                        if (klant == null)
                        {
                            string naamKlant = (string)reader["naam"];
                            string adresKlant = (string)reader["adres"];
                            klant = new Klant(klantnr,naamKlant, adresKlant);
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("bestellingnr"))) //heeft bestellingen
                        {
                            bestellingnr = (int)reader["bestellingnr"];
                            if (bestellingnr != bestellingNrOld)
                            {
                                //nieuwe bestelling of eerste
                                if (bestellingNrOld>0)
                                {
                                    //maak bestelling want we zitten op het einde
                                    Bestelling b = new Bestelling(truitjes,bestellingNrOld,tijdstip,prijs,klant,betaald);
                                    bestellingen.Add(b);
                                    truitjes = new Dictionary<Truitje, int>();
                                }
                                first = true;
                                bestellingNrOld=bestellingnr;
                            }
                            if (first)
                            {
                                betaald = (bool)reader["betaald"];
                                prijs = (double)reader["prijsbestelling"];
                                tijdstip = (DateTime)reader["tijdstip"];
                                first = false;
                            }
                            //bestelling heeft altijd truitjes
                            aantal = (int)reader["aantal"];
                            truitjeid = (int)reader["truitjeid"];
                            competitie = (string)reader["competitie"];
                            seizoen = (string)reader["seizoen"];
                            ploegnaam = (string)reader["ploegnaam"];
                            prijstruitje = (double)reader["prijstruitje"];
                            maat = Enum.Parse<KledingMaat>((string)reader["maat"]); //??
                            versie = (int)reader["versie"];
                            uit = (bool)reader["uit"];
                            Truitje truitje = new Truitje(prijstruitje,truitjeid,seizoen,new Club(competitie,ploegnaam),new ClubSet(uit,versie),maat);
                            truitjes.Add(truitje,aantal);
                        }
                    }
                    reader.Close();
                    if (bestellingnr > 0)
                    {
                        Bestelling b = new Bestelling(truitjes, bestellingnr, tijdstip, prijs, klant, betaald);
                        bestellingen.Add(b);
                    }
                    return klant;
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryException("GeefKlant", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
        //CONTROLEER DEZE JIMOFJDO¨FISMJ FKOQSFJKQIOJF MOIklWATTEFUCK
        public IEnumerable<Klant> GeefKlanten(string naam, string adres)
        {
            List<Klant> klanten = new List<Klant>();
            string query = "select naam, adres from klant where naam=@naam or adres=@adres";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@naam", naam);
                    cmd.Parameters.AddWithValue("@adres", adres);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string klantNaam = (string)reader["naam"];
                        string klantAdres = (string)reader["adres"];
                        Klant klant = new Klant(klantNaam, klantAdres);
                        klanten.Add(klant);
                    }
                    reader.Close();
                    return klanten;
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryException("GeefKlanten", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void UpdateKlant(Klant klant)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "UPDATE klant SET naam=@naam, adres=@adres where KlantNr=@KlantNr";
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                cmd.CommandText = query;
                cmd.Parameters.AddWithValue("@naam", klant.Naam);
                cmd.Parameters.AddWithValue("@adres", klant.Adres);
                cmd.Parameters.AddWithValue("@KlantNr", klant.KlantNr);
                connection.Open();
                cmd.ExecuteReader();
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryException("UpdateKlant", ex);
                }
                finally
                {
                    connection.Close();
                }

            }
        }

        public void VerwijderKlant(Klant klant)
        {
            string sql = "DELETE FROM klant WHERE klantNr=@klantNr";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@klantNr", klant.KlantNr);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryException("VerwijderKlant", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void VoegKlantToe(Klant klant)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "insert into Klant(naam, adres) output INSERTED.klantNr Values(@naam, @adres";
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    //parameters
                    cmd.Parameters.AddWithValue("@naam", klant.KlantNr);
                    cmd.Parameters.AddWithValue("@adres", klant.KlantNr);
                    int klantNr = (int)cmd.ExecuteScalar();
                    klant.ZetKlantNr(klantNr);
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryException("VoegKlantToe", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
