using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerkoopTruithesBL.Interfaces;
using VerkoopTruitjesDL.Exceptions;

namespace VerkoopTruitjesDL.Repositories
{
    public class ClubRepository : IClubRepository
    {
        private string connectionString;
        private string huidigSeizoen;

        public ClubRepository(string connectionString, string huidigSeizoen)
        {
            this.connectionString = connectionString;
            this.huidigSeizoen = huidigSeizoen;
        }

        public bool BestaatCompetitie(string competitie)
        {
            string query = "select count(*) from club where seizoen=@seizoen and competitie=@competitie";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@seizoen", huidigSeizoen);
                    cmd.Parameters.AddWithValue("@competitie", competitie);
                    int n=(int)cmd.ExecuteScalar();
                    if (n>0) return true; else return false;
                }
                catch (Exception ex)
                {
                    throw new ClubRepositoryException("GeefCompetities", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }    

        public IReadOnlyList<string> GeefClubs(string competitie)
        {
            List<string> clubs = new List<string>();
            string query = "SELECT Ploegnaam FROM Club where seizoen=@seizoen and competitie=@competitie";
            SqlConnection connection = new SqlConnection(connectionString);
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@seizoen",huidigSeizoen);
                    cmd.Parameters.AddWithValue("@competitie",competitie);
                    SqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string club= (string)reader["ploegnaam"];
                        clubs.Add(club);
                    }
                    reader.Close();
                    return clubs;
                }
                catch (Exception ex)
                {
                    throw new ClubRepositoryException("GeefClubs", ex);
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        public IReadOnlyList<string> GeefCompetities()
        {
            List<string> competities = new List<string>();
            string query = "select distinct competitie from club where seizoen=@seizoen";
            SqlConnection connection=new SqlConnection(connectionString);
            using(SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.Add(new SqlParameter("@seizoen",SqlDbType.VarChar));
                    cmd.Parameters["@seizoen"].Value = huidigSeizoen;
                    SqlDataReader reader=cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string competitie = (string) reader["competitie"];
                        competities.Add(competitie);
                    }
                    reader.Close();
                    return competities;
                }
                catch (Exception ex)
                {
                    throw new ClubRepositoryException("GeefCompetities", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}
