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

namespace DataLaag.ADO {
    public class CountryRepositoryADO : ICountryRepository {
        #region Properties
        private readonly string _connectionString;
        #endregion

        #region Constructors
        public CountryRepositoryADO(string connectionString)
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

        public List<Country> GeefLandenContinent(int id)
        {
            string sql =
                "SELECT co.ContinentId, co.Name AS ContinentName, co.Population AS ContinentPopulation, c.* FROM[dbo].[Country] c" +
                " INNER JOIN[dbo].[Continent] co ON c.ContinentId = co.ContinentId" +
                " WHERE c.ContinentId = @ContinentId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                List<Country> countries = new();
                Continent continent = null;
                command.Parameters.AddWithValue("@ContinentId", id);
                IDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //zolang hij de naam nogn iet heeft
                    if (continent == null)
                    {
                        continent = new((string)reader["ContinentName"], (int)reader["ContinentId"]);
                    }
                    Country country = new((int)reader["CountryId"], (string)reader["Name"], (int)reader["Population"], (decimal)reader["Surface"], continent);
                    countries.Add(country);
                }
                return countries;
            }
            catch (Exception ex)
            {
                throw new CountryRepositoryADOException("GeefLandenContinentADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool HeeftLanden(int continentId)
        {
            string sql = "SELECT COUNT(*) FROM [dbo].[Country] WHERE ContinentId = @ContinentId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@ContinentId", continentId);
                int n = (int)command.ExecuteScalar();
                if (n > 0) return true;
                return false;
            }
            catch (Exception ex)
            {
                throw new CountryRepositoryADOException("HeeftLandenADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public Country LandToevoegen(Country country)
        {
            string sql = "INSERT INTO [dbo].[Country] (Name, Population, Surface, ContinentId) OUTPUT Inserted.CountryId VALUES (@Name, @Population, @Surface, @ContinentId)";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            connection.Open();
            SqlTransaction sqlTransaction = connection.BeginTransaction();
            try
            {
                command.Transaction = sqlTransaction;
                command.Parameters.AddWithValue("@Name", country.Name);
                command.Parameters.AddWithValue("@Population", country.Population);
                command.Parameters.AddWithValue("@Surface", country.Surface);
                command.Parameters.AddWithValue("@ContinentId", country.Continent.Id);
                int id = (int)command.ExecuteScalar();
                country.ZetId(id);
                sqlTransaction.Commit();
                return country;
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                throw new CountryRepositoryADOException("LandToevoegenADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool BestaatLand(int countryId)
        {
            string sql = "SELECT COUNT(*) FROM [dbo].[Country] WHERE countryId = @countryId";
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
                throw new CountryRepositoryADOException("BestaatLandADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public Country LandWeergeven(int countryId)
        {
            string sql =
                "SELECT c.*, co.ContinentId, co.Name AS ContinentName FROM[dbo].[Country] c" +
                " INNER JOIN[dbo].[Continent] co ON c.ContinentId = co.ContinentId" +
                " WHERE c.CountryId = @CountryId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@CountryId", countryId);
                IDataReader reader = command.ExecuteReader();
                reader.Read();
                Continent continent = new((string)reader["ContinentName"], (int)reader["ContinentId"]);
                Country country = new(countryId, (string)reader["Name"], (int)reader["Population"], (decimal)reader["Surface"], continent);
                continent.AddCountry(country);
                reader.Close();
                return country;
            }
            catch (Exception ex)
            {
                throw new CountryRepositoryADOException("LandWeergevenADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public void LandVerwijderen(int countryId)
        {
            string sql = "DELETE FROM [dbo].[Country] WHERE countryId = @countryId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@countryId", countryId);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new CountryRepositoryADOException("LandVerwijderenADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public Country LandUpdaten(Country country)
        {
            //Country countryDB = LandWeergeven(country.Id);
            string sqlupdateBasics = "UPDATE [dbo].[Country] SET Name = @Name, Surface = @Surface, Population = @Population, ContinentId =@ContinentId WHERE CountryId = @CountryId";
            SqlConnection connection = GetConnection();
            {
                using (SqlCommand command = connection.CreateCommand())
                {
                    connection.Open();
                    try
                    {
                        command.Parameters.AddWithValue("@Name", country.Name);
                        command.Parameters.AddWithValue("@Surface", country.Surface);
                        command.Parameters.AddWithValue("@Population", country.Population);
                        command.Parameters.AddWithValue("@ContinentId", country.Continent.Id);
                        command.Parameters.AddWithValue("@CountryId", country.Id);
                        command.CommandText = sqlupdateBasics;
                        command.ExecuteNonQuery();
                        return country;
                    }
                    catch (Exception ex)
                    {
                        throw new CountryRepositoryADOException("Kon niet upgedate worden" + ex.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        public bool BestaatLand(string name, int id)
        {
            string sql = "SELECT COUNT(*) FROM [dbo].[Country] WHERE Name = @Name AND ContinentId = @ContinentId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@Name", name);
                command.Parameters.AddWithValue("@ContinentId", id);
                int n = (int)command.ExecuteScalar();
                if (n > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new CountryRepositoryADOException("BestaatLandADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool ZitLandInContinent(int continentId, int countryId)
        {
            string sql = "SELECT COUNT(*) FROM [dbo].[Country] WHERE CountryId = @CountryId AND ContinentId = @ContinentId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@CountryId", countryId);
                command.Parameters.AddWithValue("@ContinentId", continentId);
                int n = (int)command.ExecuteScalar();
                if (n > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new CountryRepositoryADOException("ZitLandInContinentADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion
    }
}
