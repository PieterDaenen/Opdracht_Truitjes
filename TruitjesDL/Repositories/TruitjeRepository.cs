using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Interfaces;
using VerkoopTruithesBL.Model;
using VerkoopTruitjesDL.Exceptions;

namespace VerkoopTruitjesDL.Repositories
{
    public class TruitjeRepository : ITruitjeRepository
    {
        private string connectionString;

        public TruitjeRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public bool BestaatTruitje(Truitje truitje)
        {
            string query = "select count(*) from truitje where truitjeId=@truitjeId;";
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
                    throw new TruitjeRepositoryException("BestaatTruitje(truitjeid)", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public bool BestaatTruitje(int truitjeId)
        {
            string query = "select count(*) from truitje where truitjeId=@truitjeId";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@truitjeId", truitjeId);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new KlantRepositoryException("BestaatTruitje", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public Truitje GeefTruitje(int truitjeId)
        {
            string query = "SELECT * FROM truitje t left join Club c on t.ClubId = c.ClubId join ClubSet s on t.ClubSetId = s.ClubsetId where truitjeId=@truitjeId;";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@truitjeId", truitjeId);
                    SqlDataReader reader = cmd.ExecuteReader();
                    Truitje truitje = null;
                    double prijstruitje;
                    bool uit;
                    int truitjeid;
                    int versie;
                    string competitie;
                    string ploegnaam;
                    string seizoen;
                    KledingMaat maat;
                    while (reader.Read())
                    {
                        prijstruitje = (double)reader["prijstruitje"];
                        truitjeid = (int)reader["truitjeid"];
                        seizoen = (string)reader["seizoen"];

                        ploegnaam = (string)reader["ploegnaam"];
                        competitie = (string)reader["competitie"];

                        uit = (bool)reader["uit"];
                        versie = (int)reader["versie"];

                        maat = Enum.Parse<KledingMaat>((string)reader["maat"]);
                        
                        truitje = new Truitje(prijstruitje, truitjeid, seizoen, new Club(competitie, ploegnaam), new ClubSet(uit, versie), maat);

                    }
                    reader.Close();
                    return truitje;
                }
                catch (Exception ex)
                {
                    throw new TruitjeRepositoryException("GeefTruitje(truitjeid)", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public IEnumerable<Truitje> GeefTruitjes(string competitie, string club, string seizoen, string kledingmaat, int? versie, bool? thuis, double? prijs, bool v)
        {
            //wtf is dit
            List<Truitje> truitjes = new List<Truitje>();
            string query = "SELECT * FROM truitje t left join Club c on t.ClubId = c.ClubId join ClubSet s on t.ClubSetId = s.ClubsetId where competitie=@competitie and club=@club and seizoen=@seizoen and kledingmaat=@kledingmaat";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@competitie", competitie);
                    cmd.Parameters.AddWithValue("@club", club);
                    cmd.Parameters.AddWithValue("@seizoen", seizoen);
                    cmd.Parameters.AddWithValue("@kledingmaat", kledingmaat);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string tCompetitie = (string)reader[]
                    }
                }
            }
        }

        public void UpdateTruitje(Truitje truitje)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "UPDATE truitje SET prijs=@prijs, seizoen=@seizoen, club=@club, clubset=@clubset, kledingmaat=@kledingmaat WHERE truitjeId=@truitjeId";
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@prijs", truitje.Prijs);
                    cmd.Parameters.AddWithValue("@seizoen", truitje.Seizoen);
                    cmd.Parameters.AddWithValue("@club", truitje.Club);
                    cmd.Parameters.AddWithValue("@clubset", truitje.ClubSet);
                    cmd.Parameters.AddWithValue("@kledingmaat", truitje.KledingMaat);
                    cmd.Parameters.AddWithValue("@truitjeId", truitje.Id);
                    cmd.ExecuteNonQuery();
                }   
                catch (Exception ex)
                {
                    throw new TruitjeRepositoryException("UpdateTruitje", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void VerwijderTruitje(Truitje truitje)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string sql = "DELETE FROM truitje WHERE truitjeId=@truitjeId";
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                connection.Open();
                cmd.CommandText = sql;
                cmd.Parameters.AddWithValue("@truitjeId", truitje.Id);
                cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new TruitjeRepositoryException("VerwijderTruitje", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void VoegTruitjeToe(Truitje truitje)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            string sql = "insert into Truitje(prijs,clubsetid,clubid,maat) output INSERTED.truitjeid "
                    + "Values(@prijs,"
                    + "(select clubsetid from clubset where uit = @uit and versie = @versie),"
                    + "(Select clubid from club where competitie = @competitie and ploegnaam = @ploegnaam and seizoen = @seizoen)"
                    + ",@maat)";
            using (SqlCommand cmd = conn.CreateCommand())
            {
                try
                {
                    conn.Open();
                    cmd.CommandText = sql;
                    cmd.Parameters.AddWithValue("@prijs", truitje.Prijs);
                    cmd.Parameters.AddWithValue("@uit",truitje.ClubSet.Uit);
                    cmd.Parameters.AddWithValue("@versie", truitje.ClubSet.Versie);
                    cmd.Parameters.AddWithValue("@maat",truitje.KledingMaat.ToString());
                    cmd.Parameters.AddWithValue("@seizoen", truitje.Seizoen);
                    cmd.Parameters.AddWithValue("@competitie", truitje.Club.Competitie);
                    cmd.Parameters.AddWithValue("@ploegnaam", truitje.Club.Ploegnaam);
                    int id=(int)cmd.ExecuteScalar();
                    truitje.ZetId(id);
                }
                catch(Exception ex)
                {
                    throw new TruitjeRepositoryException("VoegTruitjeToe", ex);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
