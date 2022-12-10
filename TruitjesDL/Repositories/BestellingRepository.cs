using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruitjesDL.Exceptions;
using VerkoopTruithesBL.Interfaces;
using VerkoopTruithesBL.Model;
using VerkoopTruitjesDL.Exceptions;

namespace VerkoopTruitjesDL.Repositories
{
    public class BestellingRepository : IBestellingRepository
    {
        private string connectionString;
        private KlantRepository klantRepository;
        public BestellingRepository(string connectionString)
        {
            this.connectionString = connectionString;
            klantRepository = new KlantRepository(connectionString);
        }

        public bool BestaatBestelling(Bestelling bestelling)
        {
            string query = "SELECT count(*) FROM bestelling WHERE bestellingNr=@bestellingNr";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@bestellingNr", bestelling.BestellingNr);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new BestellingRepositoryException("BestaatBestelling", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool BestaatBestelling(int bestellingNr)
        {
            string query = "SELECT count(*) FROM bestelling WHERE bestellingNr=@bestellingNr";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@bestellingNr", bestellingNr);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new BestellingRepositoryException("BestaatBestelling", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void VoegBestellingToe(Bestelling bestelling)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlTransaction transaction = null;
            string queryBestelling = "INSERT INTO bestelling(prijs, klantNr, betaald, tijdstip) output INSERTED.bestellingNr VALUES(@prijs, @klantNr, @betaald, @tijdstip )";
            string queryDetail = "INSERT INTO bestellingdetail(bestellingNr, truitjeId, aantal) VALUES(@bestellingNr, @truitjeId, @aantal)";
            using (SqlCommand cmd = connection.CreateCommand())
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                try
                {
                    cmd.CommandText = queryBestelling;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@prijs", bestelling.BerekenPrijs());
                    cmd.Parameters.AddWithValue("@klantNr", bestelling.Klant.KlantNr);
                    cmd.Parameters.AddWithValue("@betaald", bestelling.Betaald);
                    cmd.Parameters.AddWithValue("@tijdstip", bestelling.Tijdstip);
                    int bestellingNr = (int)cmd.ExecuteScalar();
                    bestelling.ZetBestellingNr(bestellingNr);

                    IReadOnlyDictionary<Truitje, int> truitjes = bestelling.GetTruitjes();

                    cmd.Parameters.Clear();
                    cmd.CommandText = queryDetail;
                    cmd.Parameters.AddWithValue("@bestellingNr", bestellingNr);
                    //bestellingNr = cmd.Parameters.Add("@bestellingNr");
                    cmd.Parameters.Add(new SqlParameter("@truitjeId", System.Data.SqlDbType.Int));
                    cmd.Parameters.Add(new SqlParameter("@aantal", System.Data.SqlDbType.Int));


                    foreach (KeyValuePair<Truitje, int> kvp in truitjes)
                    {
                        cmd.Parameters["@truitjeId"].Value = kvp.Key.Id;
                        cmd.Parameters["@aantal"].Value = kvp.Value;
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new BestellingRepositoryException("VoegBestellingToe", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void UpdateBestelling(Bestelling bestelling)
        {

            SqlConnection connection = new SqlConnection(connectionString);
            SqlTransaction transaction = null;

            //queryUpdateMainbestelling
            string query = "UPDATE bestelling SET prijs=@prijs, klantNr=@klantNr, betaald=@betaald, tijdstip=@tijdstip WHERE bestellingNr=@bestellingNr";
            //queryVerwijderOudBestellingDetail
            string query2 = "DELETE from bestellingDetail WHERE bestellingNr=@bestellingNr";
            //queryNieuwInsertBestellingDetail
            string query3 = "INSERT INTO bestellingdetail(bestellingNr, truitjeId, aantal) VALUES(@bestellingNr, @truitjeId, @aantal)";
            using (SqlCommand cmd = connection.CreateCommand())
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                try
                {
                    cmd.CommandText = query;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@prijs", bestelling.Prijs);
                    cmd.Parameters.AddWithValue("@klantNr", bestelling.Klant.KlantNr);
                    cmd.Parameters.AddWithValue("@betaald", bestelling.Betaald);
                    cmd.Parameters.AddWithValue("@tijdstip", bestelling.Tijdstip);
                    cmd.Parameters.AddWithValue("@bestellingNr", bestelling.BestellingNr);

                    cmd.ExecuteNonQuery();
                    cmd.CommandText = query2;
                    cmd.ExecuteNonQuery();
                    cmd.CommandText = query3;
                    foreach (var kvp in bestelling.GetTruitjes())
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@bestellingNr", bestelling.BestellingNr);
                        cmd.Parameters.AddWithValue("@truitjeId", kvp.Key.Id);
                        cmd.Parameters.AddWithValue("@aantal", kvp.Value);
                        cmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new BestellingRepositoryException("UpdateBestelling", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void VerwijderBestelling(Bestelling bestelling)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlTransaction transaction = null;
            string query = "DELETE from bestellingdetail WHERE bestellingNr=@bestellingNr";
            string query2 = "DELETE FROM bestelling WHERE bestellingNr=@bestellingNr";
            using (SqlCommand cmd = connection.CreateCommand())
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                try
                {
                    cmd.CommandText = query;
                    cmd.Transaction = transaction;
                    cmd.Parameters.AddWithValue("@bestellingNr", bestelling.BestellingNr);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = query2;
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new BestellingRepositoryException("VerwijderBestelling", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool ZoekTruitjeInBestellingen(Truitje truitje)
        {
            string query = "SELECT count(*) FROM bestellingdetail WHERE truitjeId=@truitjeId";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@truitjeId", truitje.Id);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new BestellingRepositoryException("ZoekTruitjeInBestelling", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public Bestelling GeefBestelling(int bestellingNr)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "SELECT t1.KlantNr,t1.Naam,t1.Adres,t2.BestellingNr,t2.Prijs prijsbestelling,t2.Betaald,t2.Tijdstip,"
                + "t3.TruitjeId,t3.Aantal,t4.Prijs prijstruitje, t4.maat,t5.Competitie,t5.Ploegnaam,t5.Seizoen,"
                + "t6.Uit,t6.Versie "
                + " FROM Klant t1 "
                + "left join Bestelling t2 on t1.KlantNr = t2.KlantNr "
                + "left join BestellingDetail t3 on t2.BestellingNr = t3.BestellingNr "
                + "left join Truitje t4 on t3.TruitjeId = t4.TruitjeId "
                + "left join club t5 on t4.ClubId = t5.ClubId "
                + "left join clubset t6 on t4.ClubsetId = t6.ClubSetId "
                + "where t2.bestellingNr = @bestellingNr "
                + "order by t2.BestellingNr";
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@bestellingNr", bestellingNr);

                    double prijs = 0;
                    DateTime tijdstip = DateTime.Now;
                    bool betaald = false;

                    int klantNr;
                    string klantAdres;
                    string klantNaam;
                    Klant klant = null;

                    Dictionary<Truitje, int> truitjes = new Dictionary<Truitje, int>();
                    Truitje truitje;
                    int truitjeAantal;

                    Bestelling bestelling = null;
                    int bestellingNrOld;

                    int truitjeId;
                    double truitjePrijs;
                    string truitjeSeizoen;
                    Club truitjeClub;
                    ClubSet truitjeClubSet;
                    KledingMaat truitjeKledingMaat;

                    string clubCompetitie;
                    string clubNaam;

                    bool csUit;
                    int csVersie;

                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if (klant == null || klant.KlantNr != (int)reader["klantNr"])
                        {
                            klantNr = (int)reader["klantNr"];
                            klant = klantRepository.GeefKlant(klantNr);
                        }
                        if (bestelling == null)
                        {
                            betaald = (bool)reader["betaald"];
                            prijs = (double)reader["prijsbestelling"];
                            tijdstip = (DateTime)reader["tijdstip"];
                        }

                        truitjeId = (int)reader["truitjeId"];
                        truitjePrijs = (double)reader["prijstruitje"];
                        truitjeSeizoen = (string)reader["seizoen"];
                        truitjeKledingMaat = Enum.Parse<KledingMaat>((string)reader["maat"]);

                        clubCompetitie = (string)reader["competitie"];
                        clubNaam = (string)reader["ploegnaam"];
                        truitjeClub = new Club(clubCompetitie, clubNaam);

                        csUit = (bool)reader["uit"];
                        csVersie = (int)reader["versie"];
                        truitjeClubSet = new ClubSet(csUit, csVersie);

                        truitje = new Truitje(truitjePrijs, truitjeId, truitjeSeizoen, truitjeClub, truitjeClubSet, truitjeKledingMaat);
                        truitjeAantal = (int)reader["aantal"];

                        truitjes.Add(truitje, truitjeAantal);

                    }
                    //klant.VerwijderBestelling(bestelling);
                    bestelling = new Bestelling(truitjes, bestellingNr, tijdstip, prijs, klant, betaald);
                    //bestelling = (Bestelling)klant.GeefBestellingen().Where(x => x.BestellingNr == bestellingNr);

                    return bestelling;
                }
                catch (Exception ex)
                {
                    throw new BestellingRepositoryException("GeefBestelling", ex);
                }
                finally
                {
                    connection.Close();
                }
            }


        }

        public IEnumerable<Bestelling> GeefBestellingen(int? klantId, DateTime? startDatum, DateTime? eindDatum)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            List<Klant> klanten = new List<Klant>();
            string query = "SELECT t1.KlantNr,t1.Naam,t1.Adres,t2.BestellingNr,t2.Prijs prijsbestelling,t2.Betaald,t2.Tijdstip,"
                + "t3.TruitjeId,t3.Aantal,t4.Prijs prijstruitje, t4.maat,t5.Competitie,t5.Ploegnaam,t5.Seizoen,"
                + "t6.Uit,t6.Versie "
                + " FROM Klant t1 "
                + "left join Bestelling t2 on t1.KlantNr = t2.KlantNr "
                + "left join BestellingDetail t3 on t2.BestellingNr = t3.BestellingNr "
                + "left join Truitje t4 on t3.TruitjeId = t4.TruitjeId "
                + "left join club t5 on t4.ClubId = t5.ClubId "
                + "left join clubset t6 on t4.ClubsetId = t6.ClubSetId "
                + "WHERE 1=1";

            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();

                    if (klantId is not null)
                    {
                        query += "AND t2.klantNr = @klantNr ";
                        cmd.Parameters.AddWithValue("@klantNr", klantId);
                    }
                    if (startDatum is not null)
                    {
                        query += "AND tijdstip > @startDatum ";
                        cmd.Parameters.AddWithValue("@startDatum", startDatum);
                    }
                    if (eindDatum is not null)
                    {
                        query += "AND tijdstip < @eindDatum ";
                        cmd.Parameters.AddWithValue("@eindDatum", eindDatum);
                    }

                    query += "ORDER BY t2.KlantNr, t2.BestellingNr";

                    cmd.CommandText = query;

                    // nu neemt die nieuwe klant, voegt bestellingen toe, en gaat naar nieuwe klant
                    // ik moet enkel bestellingen


                    SqlDataReader reader = cmd.ExecuteReader();
                    Klant klant = null;
                    int bestellingNrOld = -1;
                    int bestellingnr = 0;
                    bool betaald = false;
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
                    Dictionary<Truitje, int> truitjes = new Dictionary<Truitje, int>();
                    List<Bestelling> bestellingen = new List<Bestelling>();
                    KledingMaat maat;
                    DateTime tijdstip = DateTime.Now;
                    while (reader.Read())
                    {
                        if (klant == null || klant.KlantNr != (int)reader["klantNr"])
                        {
                            string naamKlant = (string)reader["naam"];
                            string adresKlant = (string)reader["adres"];
                            klant = new Klant((int)reader["klantNr"], naamKlant, adresKlant);
                            klanten.Add(klant);
                        }
                        if (!reader.IsDBNull(reader.GetOrdinal("bestellingnr"))) //heeft bestellingen
                        {
                            bestellingnr = (int)reader["bestellingnr"];
                            if (bestellingnr != bestellingNrOld)
                            {
                                //nieuwe bestelling of eerste
                                //shit doen
                                //maak bestelling want we zitten op het einde
                                betaald = (bool)reader["betaald"];
                                prijs = (double)reader["prijsbestelling"];
                                tijdstip = (DateTime)reader["tijdstip"];
                                truitjes = new Dictionary<Truitje, int>();
                                Bestelling b = new Bestelling(truitjes, bestellingnr, tijdstip, prijs, klant, betaald);
                                bestellingen.Add(b);
                                first = true;
                                bestellingNrOld = bestellingnr;
                                first = false;
                            }
                            //bestelling heeft altijd truitjes
                            aantal = (int)reader["aantal"];
                            truitjeid = (int)reader["truitjeid"];
                            competitie = (string)reader["competitie"];
                            seizoen = (string)reader["seizoen"];
                            ploegnaam = (string)reader["ploegnaam"];
                            prijstruitje = (double)reader["prijstruitje"];
                            maat = Enum.Parse<KledingMaat>((string)reader["maat"]);
                            versie = (int)reader["versie"];
                            uit = (bool)reader["uit"];
                            Truitje truitje = new Truitje(prijstruitje, truitjeid, seizoen, new Club(competitie, ploegnaam), new ClubSet(uit, versie), maat);
                            truitjes.Add(truitje, aantal);
                        }
                    }
                    reader.Close();
                    //if (bestellingnr > 0)
                    //{
                    //    Bestelling b = new Bestelling(truitjes, bestellingnr, tijdstip, prijs, klant, betaald);
                    //    bestellingen.Add(b);
                    //}
                    return bestellingen;
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
    }
}
