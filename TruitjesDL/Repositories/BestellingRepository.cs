using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TruitjesDL.Exceptions;
using VerkoopTruithesBL.Interfaces;
using VerkoopTruithesBL.Model;

namespace VerkoopTruitjesDL.Repositories
{
    public class BestellingRepository : IBestellingRepository
    {
        private string connectionString;
        public BestellingRepository(string connectionString)
        {
            this.connectionString = connectionString;
            // truitjeRepository = new TruitjeRepository(connectionString);
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
            string queryBestelling = "INSERT INTO bestelling(prijs, klantNr, betaald, tijdstip) output INSERTED.bestellingNr VALUES(@prijs, @klantNr, @betaald, @tijdstip )";
            string queryDetail = "INSERT INTO bestellingdetail(bestellingNr, truitjeId, aantal) VALUES(@bestellingNr, @truitjeId, @aantal)";
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = queryBestelling;
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
                }
                catch (Exception ex)
                {
                    throw new BestellingRepositoryException("VoegBestellingToe", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        //_____NOG FIXEN_____

        public void UpdateBestelling(Bestelling bestelling)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "UPDATE bestelling SET prijs=@prijs, klantNr=@klantNr, betaald=@betaald, tijdstip=@tijdstip WHERE bestellingNr=@bestellingNr";
            // hier moet insert/update
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@prijs", bestelling.Prijs);
                    cmd.Parameters.AddWithValue("@klantNr", bestelling.Klant.KlantNr);
                    cmd.Parameters.AddWithValue("@betaald", bestelling.Betaald);
                    cmd.Parameters.AddWithValue("@tijdstip", bestelling.Tijdstip);
                    cmd.Parameters.AddWithValue("@bestellingNr", bestelling.BestellingNr);

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
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
            string query = "DELETE FROM bestelling WHERE bestellingNr=@bestellingNr";
            string query2 = "DELETE from bestellingdetail WHERE bestellingNr=@bestellingNr";
            using (SqlCommand cmd = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@bestellingNr", bestelling.BestellingNr);
                    cmd.ExecuteNonQuery();

                    cmd.CommandText = query2;
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    throw new BestellingRepositoryException("VerwijderBestelling", ex);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public Bestelling GeefBestelling(int bestellingNr)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Bestelling> GeefBestellingen(int? klantId, DateTime? startDatum, DateTime? eindDatum)
        {
            throw new NotImplementedException();
        }
    }
}
