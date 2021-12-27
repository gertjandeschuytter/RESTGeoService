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

        public List<City> GeefStedenLand(int countryId)
        {
            string sql =
                "SELECT c.ContinentId, c.Name AS ContinentName, Country.CountryId, Country.Name AS CountryName," +
                " Country.Population AS CountryPopulation, Country.Surface, Country.ContinentId, city.* FROM [dbo].[City] city " +
                "INNER JOIN [dbo].[Country] Country ON city.CountryId = Country.CountryId " +
                "INNER JOIN [dbo].[Continent] c ON Country.ContinentId = c.ContinentId " +
                "WHERE Country.countryId = @countryId;";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                Continent continent = null;
                Country country = null;
                List<City> steden = new();
                command.Parameters.AddWithValue("@countryId", countryId);
                IDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (continent == null)
                    {
                        continent = new((string)reader["ContinentName"],(int)reader["ContinentId"]);
                    }
                    if (country == null)
                    {
                        country = new((int)reader["CountryId"], (string)reader["CountryName"], (int)reader["CountryPopulation"], (decimal)reader["Surface"], continent);
                        continent.AddCountry(country);
                    }
                    City stad = new((int)reader["Id"], (string)reader["Name"], (int)reader["Population"], (bool)reader["IsCapital"], country);
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

        public bool HeeftSteden(int countryId)
        {
            string sql = "SELECT COUNT(*) FROM [dbo].[City] WHERE CountryId = @countryId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@countryId", countryId);
                int n = (int)command.ExecuteScalar();
                if (n > 0) return true;
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
            string sql = "INSERT INTO [dbo].[City] (Name, Population, IsCapital, CountryId) OUTPUT INSERTED.Id VALUES (@Name, @Population, @IsCapital, @CountryId)";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;
                command.Parameters.AddWithValue("@Name", stad.Name);
                command.Parameters.AddWithValue("@Population", stad.Population);
                command.Parameters.AddWithValue("@IsCapital", stad.IsCapital);
                command.Parameters.AddWithValue("@CountryId", stad.Country.Id);
                var id = (int)command.ExecuteScalar();
                transaction.Commit();
                stad.ZetId(id);
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

        public bool BestaatStad(int cityId)
        {
            string sql = "SELECT COUNT(*) FROM [dbo].[City] WHERE Id = @cityId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@cityId", cityId);
                int n = (int)command.ExecuteScalar();
                if (n > 0) return true;
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

        public City StadWeergeven(int cityId)
        {
            string sql =
                "SELECT Continent.ContinentId, Continent.Name AS ContinentName, Country.CountryId, Country.Name AS CountryName, " +
                "Country.Population AS CountryPopulation, Country.Surface, Country.ContinentId, city.* FROM [dbo].[City] city " +
                "INNER JOIN [dbo].[Country] Country ON city.CountryId = Country.CountryId " +
                "INNER JOIN [dbo].[Continent] Continent ON Country.ContinentId = Continent.ContinentId " +
                "WHERE city.Id = @cityId;";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                Continent continent = null;
                Country country = null;
                command.Parameters.AddWithValue("@cityId", cityId);
                IDataReader reader = command.ExecuteReader();
                reader.Read();
                if (continent == null)
                {
                    continent = new((string)reader["ContinentName"],(int)reader["ContinentId"]);
                }
                if (country == null)
                {
                    country = new((int)reader["CountryId"], (string)reader["CountryName"], (int)reader["CountryPopulation"], (decimal)reader["Surface"], continent);
                }
                City city = new((int)reader["Id"], (string)reader["Name"], (int)reader["Population"], (bool)reader["IsCapital"], country);
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

        public void StadVerwijderen(int cityId)
        {
            string sql = "DELETE FROM [dbo].[City] WHERE Id = @cityId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@cityId", cityId);
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
            string sql = "UPDATE [dbo].[City] SET Name = @Name, Population = @Population, IsCapital = @IsCapital, CountryId = @CountryId WHERE Id = @CityId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            try
            {
                command.Transaction = transaction;
                command.Parameters.AddWithValue("@CityId", city.Id);
                command.Parameters.AddWithValue("@Name", city.Name);
                command.Parameters.AddWithValue("@Population", city.Population);
                command.Parameters.AddWithValue("@IsCapital", city.IsCapital);
                command.Parameters.AddWithValue("@CountryId", city.Country.Id);
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

        public bool ControleerBevolkingsaantal(int ContinentId, int Population)
        {
            string sql =
            "SELECT c.ContinentId, c.Name AS ContinentName, Country.CountryId, Country.Name AS Countryname," +
            " Country.Population AS CountryPopulation, Country.Surface, Country.ContinentId, city.* FROM[dbo].[City] city" +
            " INNER JOIN[dbo].[Country] Country ON city.CountryId = Country.CountryId" +
            " INNER JOIN[dbo].[Continent] c ON Country.ContinentId = c.ContinentId" +
            " WHERE Country.ContinentId = @ContinentId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            List<City> cities = new();
            Continent continent = null;
            Country country = null;
            int totaalBevolking = 0;
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@ContinentId", ContinentId);
                IDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (continent == null)
                    {
                        continent = new((string)reader["ContinentName"],(int)reader["ContinentId"]);
                    }
                    if (country == null)
                    {
                        country = new((int)reader["CountryId"], (string)reader["Countryname"], (int)reader["CountryPopulation"], (decimal)reader["Surface"], continent);
                    }
                    City city = new((int)reader["CountryId"], (string)reader["Name"], (int)reader["Population"], (bool)reader["IsCapital"], country);
                    cities.Add(city);
                }
                reader.Close();
                if (cities.Count > 0)
                {
                    foreach (City stad in cities)
                    {
                        totaalBevolking += stad.Population;
                    }
                    if ((totaalBevolking + Population ) > country.Population)
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
                "SELECT COUNT(*) FROM [dbo].[City] city " +
                "INNER JOIN [dbo].[Country] Country ON city.CountryId = Country.CountryId " +
                "INNER JOIN [dbo].[Continent] Continent ON Country.ContinentId = Continent.ContinentId " +
                "WHERE Continent.ContinentId = @ContinentId " +
                "AND Country.CountryId = @CountryId " +
                "AND city.Id = @CityId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@ContinentId", continentId);
                command.Parameters.AddWithValue("@CityId", cityId);
                command.Parameters.AddWithValue("@CountryId", countryId);
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
