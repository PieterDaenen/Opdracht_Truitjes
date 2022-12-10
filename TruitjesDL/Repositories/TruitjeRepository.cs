using System;
using System.Collections.Generic;
using System.Data;
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
                        prijstruitje = (double)reader["prijs"];
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

        //!
        public IEnumerable<Truitje> GeefTruitjes(string? competitie, string? club, string? seizoen, string? kledingmaat, int? versie, bool? thuis, double? prijs)
        {
            List<Truitje> truitjes = new();

            string query = "SELECT * FROM Truitje t " +
                "JOIN Club c on(t.ClubId = c.ClubId) " +
                "JOIN Clubset cs on(t.ClubsetId = cs.ClubsetId) " +
                "WHERE 1=1 ";

            SqlConnection sqlConnection = new(connectionString);
            using SqlCommand sqlCommand = sqlConnection.CreateCommand();
            try
            {

                sqlConnection.Open();

                if (competitie is not null)
                {
                    competitie = competitie.Trim();
                    query += "AND Competitie LIKE @Competitie ";
                    sqlCommand.Parameters.AddWithValue("@Competitie", $"%{competitie}%");
                }
                if (club is not null)
                {
                    club = club.Trim();
                    query += "AND Ploegnaam LIKE @Club ";
                    sqlCommand.Parameters.AddWithValue("@Club", $"%{club}%");
                }
                if (seizoen is not null)
                {
                    seizoen = seizoen.Trim();
                    query += "AND Seizoen LIKE @Seizoen ";
                    sqlCommand.Parameters.AddWithValue("@Seizoen", $"%{seizoen}%");
                }
                if (kledingmaat is not null)
                {
                    kledingmaat = kledingmaat.Trim();
                    query += "AND maat = @Kledingmaat ";
                    sqlCommand.Parameters.AddWithValue("@Kledingmaat", $"{kledingmaat}");
                }
                if (versie is not null)
                {
                    query += "AND Versie = @Versie ";
                    sqlCommand.Parameters.AddWithValue("@Versie", $"{versie}");
                }
                if (thuis is not null)
                {
                    query += "AND Uit = @Thuis ";
                    sqlCommand.Parameters.AddWithValue("@Thuis", $"{(thuis == true ? 1 : 0)}");
                }
                if (prijs is not null)
                {
                    query += "AND Prijs LIKE @Prijs ";
                    sqlCommand.Parameters.AddWithValue("@Prijs", $"%{prijs}%");
                }

                sqlCommand.CommandText = query;

                IDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    int gevondenTruitjeId = (int)sqlDataReader["TruitjeId"];
                    double gevondenPrijs = (double)sqlDataReader["Prijs"];
                    string gevondenSeizoen = (string)sqlDataReader["Seizoen"];

                    KledingMaat gevondenMaat = Enum.Parse<KledingMaat>((string)sqlDataReader["Maat"]);
                    ClubSet gevondenClubSet = new((bool)sqlDataReader["Uit"], (int)sqlDataReader["Versie"]);
                    Club gevondenClub = new((string)sqlDataReader["Competitie"], (string)sqlDataReader["Ploegnaam"]);

                    truitjes.Add(new(gevondenPrijs, gevondenTruitjeId, gevondenSeizoen, gevondenClub, gevondenClubSet, gevondenMaat));
                }

                sqlDataReader.Close();

                return truitjes;
            }
            catch (Exception ex)
            {
                throw new TruitjeRepositoryException(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public void UpdateTruitje(Truitje truitje)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string queryInsert = "UPDATE truitje SET prijs=@prijs, " +
                "clubId = (SELECT clubId from Club WHERE ploegnaam=@ploegnaam AND seizoen=@seizoen), " +
                "clubsetId = (SELECT clubsetid FROM clubset WHERE uit=@uit AND versie=@versie), " +
                "maat=@kledingmaat WHERE truitjeId=@truitjeId";

            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = queryInsert;
                    cmd.Parameters.AddWithValue("@ploegnaam", truitje.Club.Ploegnaam);
                    cmd.Parameters.AddWithValue("@uit", truitje.ClubSet.Uit);
                    cmd.Parameters.AddWithValue("@versie", truitje.ClubSet.Versie);
                    cmd.Parameters.AddWithValue("@truitjeId", truitje.Id);
                    cmd.Parameters.AddWithValue("@prijs", truitje.Prijs);
                    cmd.Parameters.AddWithValue("@seizoen", truitje.Seizoen);
                    cmd.Parameters.AddWithValue("@kledingmaat", truitje.KledingMaat.ToString());
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
        public bool BestaatClubSet(ClubSet clubset)
        {
            string query = "SELECT count(*) FROM clubset WHERE uit=@uit AND versie=@versie;";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@uit", clubset.Uit);
                    cmd.Parameters.AddWithValue("@versie", clubset.Versie);
                    int n = (int)cmd.ExecuteScalar();
                    if (n > 0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new TruitjeRepositoryException("BestaatClubSet", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        
        public void VoegClubSetToe(ClubSet clubset)
        {
            string query = "INSERT INTO clubset(uit, versie) VALUES(@uit, @versie)";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@uit", clubset.Uit);
                    cmd.Parameters.AddWithValue("@versie", clubset.Versie);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new TruitjeRepositoryException("VoegClubSetToe", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
