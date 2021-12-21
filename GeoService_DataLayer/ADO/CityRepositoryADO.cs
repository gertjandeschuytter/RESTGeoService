using DataLaag.Exceptions;
using DomeinLaag.Interfaces;
using GeoService_BusinessLayer.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLaag.ADO
{
    public class CityRepositoryADO : ICityRepository
    {
        #region Properties
        private readonly string _connectionString;
        #endregion

        #region Constructors
        public CityRepositoryADO(string connectionString)
        {
            _connectionString = connectionString;
        }
        #endregion

        #region Methods
        private SqlConnection GetConnection()
        {
            SqlConnection connection = new(_connectionString);
            return connection;
        }

        public List<City> GeefStedenLand(int id)
        {
            string sql = "SELECT c.Id AS ContinentId, c.Naam AS ContinentNaam, c.Bevolkingsaantal AS ContinentBevolkingsaantal, l.Id AS LandId, l.Naam AS LandNaam, l.Bevolkingsaantal AS LandBevolkingsaantal, l.Oppervlakte, l.ContinentId, s.* FROM [dbo].[Stad] s " +
                "INNER JOIN [dbo].[Land] l ON s.LandId = l.Id " +
                "INNER JOIN [dbo].[Continent] c ON l.ContinentId = c.Id WHERE l.Id = @Id;";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                Continent continent = null;
                Country land = null;
                List<City> steden = new();
                command.Parameters.AddWithValue("@Id", id);
                IDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (continent == null)
                    {
                        continent = new((int)reader["ContinentId"], (string)reader["ContinentName"], (int)reader["Bevolkingsaantal"]);
                    }
                    if (land == null)
                    {
                        land = new((int)reader["LandId"], (string)reader["LandNaam"], (int)reader["LandBevolkingsaantal"], (decimal)reader["Oppervlakte"], continent);
                    }
                    City stad = new((int)reader["Id"], (string)reader["Naam"], (int)reader["Bevolkingsaantal"], (bool)reader["IsHoofdstad"], land);
                    steden.Add(stad);
                }
                reader.Close();
                return steden;
            }
            catch (Exception ex)
            {
                throw new CityRepositoryADOException("GeefStedenLandADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool HeeftSteden(int landId)
        {
            string sql = "SELECT COUNT(*) FROM [dbo].[Stad] WHERE LandId = @LandId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@LandId", landId);
                int n = (int)command.ExecuteScalar();
                if (n > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new CityRepositoryADOException("HeeftStedenADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public City StadToevoegen(City stad)
        {
            string sql = "INSERT INTO [dbo].[Stad] (Naam, Bevolkingsaantal, IsHoofdStad, LandId) VALUES (@Naam, @Bevolkingsaantal, @IsHoofdStad, @LandId)";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;
                command.Parameters.AddWithValue("@Naam", stad.Name);
                command.Parameters.AddWithValue("@Bevolkingsaantal", stad.Population);
                command.Parameters.AddWithValue("@IsHoofdStad", stad.IsCapital);
                command.Parameters.AddWithValue("@LandId", stad.Country.Id);
                command.ExecuteNonQuery();
                transaction.Commit();
                return stad;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new CityRepositoryADOException("StadToevoegenADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool BestaatStad(int id)
        {
            string sql = "SELECT COUNT(*) FROM [dbo].[Stad] WHERE Id = @Id";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@Id", id);
                int n = (int)command.ExecuteScalar();
                if (n > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new CityRepositoryADOException("BestaatStadADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public City StadWeergeven(int stadId)
        {
            string sql = "SELECT c.Id AS ContinentId, c.Naam AS ContinentNaam, c.Bevolkingsaantal AS ContinentBevolkingsaantal, l.Id AS LandId, l.Naam AS LandNaam, l.Bevolkingsaantal AS LandBevolkingsaantal, l.Oppervlakte, l.ContinentId, s.* FROM [dbo].[Stad] s " +
                "INNER JOIN [dbo].[Land] l ON s.LandId = l.Id " +
                "INNER JOIN [dbo].[Continent] c ON l.ContinentId = c.Id WHERE s.Id = @Id;";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                Continent continent = null;
                Country country = null;
                command.Parameters.AddWithValue("@Id", stadId);
                IDataReader reader = command.ExecuteReader();
                reader.Read();
                if (continent == null)
                {
                    continent = new((int)reader["ContinentId"], (string)reader["ContinentNaam"], (int)reader["Bevolkingsaantal"]);
                }
                if (country == null)
                {
                    country = new((int)reader["LandId"], (string)reader["LandNaam"], (int)reader["LandBevolkingsaantal"], (decimal)reader["Oppervlakte"], continent);
                }
                City city = new((int)reader["Id"], (string)reader["Naam"], (int)reader["Bevolkingsaantal"], (bool)reader["IsHoofdstad"], country);
                reader.Close();
                return city;
            }
            catch (Exception ex)
            {
                throw new CityRepositoryADOException("StadWeergevenADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public void StadVerwijderen(int stadId)
        {
            string sql = "DELETE FROM [dbo].[Stad] WHERE Id = @Id";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@Id", stadId);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new CityRepositoryADOException("BestaatStadADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public City StadUpdaten(City city)
        {
            string sql = "UPDATE [dbo].[Stad] SET Naam = @Naam, Bevolkingsaantal = @Bevolkingsaantal, IsHoofdStad = @IsHoofdStad, LandId = @LandId WHERE Idd = @Id";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;
                command.Parameters.AddWithValue("@Id", city.Id);
                command.Parameters.AddWithValue("@Naam", city.Name);
                command.Parameters.AddWithValue("@Bevolkingsaantal", city.Population);
                command.Parameters.AddWithValue("@IsHoofdStad", city.IsCapital);
                command.Parameters.AddWithValue("@LandId", city.Country.Id);
                command.ExecuteNonQuery();
                transaction.Commit();
                return city;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new CityRepositoryADOException("StadUpdatenADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool ControleerBevolkingsaantal(int landId, int bevolkingsaantal)
        {
            string sql = "SELECT c.Id AS ContinentId, c.Naam AS ContinentNaam, c.Bevolkingsaantal AS ContinentBevolkingsaantal, l.Id AS LandId, l.Naam AS LandNaam, l.Bevolkingsaantal AS LandBevolkingsaantal, l.Oppervlakte, l.ContinentId, s.* FROM [dbo].[Stad] s " +
                "INNER JOIN [dbo].[Land] l ON s.LandId = l.Id " +
                "INNER JOIN [dbo].[Continent] c ON l.ContinentId = c.Id WHERE l.Id = @Id;";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            List<City> cities = new();
            Continent continent = null;
            Country country = null;
            int totaalBevolking = 0;
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@Id", landId);
                IDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (continent == null)
                    {
                        continent = new((int)reader["ContinentId"], (string)reader["ContinentNaam"], (int)reader["Bevolkingsaantal"]);
                    }
                    if (country == null)
                    {
                        country = new((int)reader["LandId"], (string)reader["LandNaam"], (int)reader["LandBevolkingsaantal"], (decimal)reader["Oppervlakte"], continent);
                    }
                    City city = new((int)reader["Id"], (string)reader["Naam"], (int)reader["Bevolkingsaantal"], (bool)reader["IsHoofdstad"], country);
                    cities.Add(city);
                }
                reader.Close();
                if (cities.Count > 0)
                {
                    foreach (City stad in cities)
                    {
                        totaalBevolking += stad.Population;
                    }
                    if ((totaalBevolking + bevolkingsaantal) > country.Population)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new CityRepositoryADOException("ControleerBevolkingsaantalADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool ZitStadInLandInContinent(int continentId, int countryId, int cityId)
        {
            string sql = 
                "SELECT COUNT(*) FROM [dbo].[City] c " +
                "INNER JOIN [dbo].[Country] c ON s.LandId = c.Id " +
                "INNER JOIN [dbo].[Continent] co ON c.ContinentId = co.Id " +
                "WHERE co.Id = @ContinentId " +
                "AND c.Id = @countryId AND s.Id = @cityId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@ContinentId", continentId);
                command.Parameters.AddWithValue("@cityId", cityId);
                command.Parameters.AddWithValue("@countryId", countryId);
                int n = (int)command.ExecuteScalar();
                if (n > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new CityRepositoryADOException("ZitStadInLandInContinentADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion
    }
}
