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
    public class ContinentRepositoryADO : IContinentRepository
    {
        #region Properties
        private readonly string _connectionString;
        #endregion

        #region Constructors
        public ContinentRepositoryADO(string connectionString)
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

        public Continent ContinentToevoegen(Continent continent)
        {
            string sql = "INSERT INTO [dbo].[Continent] (Name, Population) OUTPUT Inserted.ContinentId VALUES (@Name, @Population)";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            connection.Open();
            SqlTransaction sqlTransaction = connection.BeginTransaction();
            try
            {
                command.Transaction = sqlTransaction;
                command.Parameters.AddWithValue("@Name", continent.Name);
                command.Parameters.AddWithValue("@Population", continent.Population);
                int id = (int)command.ExecuteScalar();
                continent.ZetId(id);
                sqlTransaction.Commit();
                return continent;
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                throw new ContinentRepositoryADOException("ContinentToevoegenADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool BestaatContinent(int continentId)
        {
            string sql = "SELECT COUNT(*) FROM [dbo].[Continent] WHERE ContinentId = @ContinentId";
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
                throw new ContinentRepositoryADOException("BestaatContinentADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool BestaatContinent(string name)
        {
            string sql = "SELECT COUNT(*) FROM [dbo].[Continent] WHERE Name = @Name";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@Name", name);
                int n = (int)command.ExecuteScalar();
                if (n > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new ContinentRepositoryADOException("BestaatContinentADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public Continent ContinentWeergeven(int continentId)
        {
            Continent continent = null;
            string sql = "SELECT * FROM [dbo].[Continent] WHERE ContinentId = @ContinentId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@ContinentId", continentId);
                IDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    continent = new((int)reader["ContinentId"], (string)reader["Name"], (int)reader["Population"]);
                }
                reader.Close();
                return continent;
            }
            catch (Exception ex)
            {
                throw new ContinentRepositoryADOException("ContinentWeergevenADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public void ContinentVerwijderen(int continentId)
        {
            string sql = "DELETE FROM [dbo].[Continent] WHERE continentId = @continentId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            try
            {
                connection.Open();
                command.Parameters.AddWithValue("@continentId", continentId);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new ContinentRepositoryADOException("ContinentVerwijderenADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public void ContinentUpdaten(Continent continent)
        {
            string sql = "UPDATE [dbo].[Continent] SET Name = @Name WHERE ContinentId = @ContinentId";
            SqlConnection connection = GetConnection();
            using SqlCommand command = new(sql, connection);
            connection.Open();
            SqlTransaction sqlTransaction = connection.BeginTransaction();
            try
            {
                command.Transaction = sqlTransaction;
                command.Parameters.AddWithValue("@ContinentId", continent.Id);
                command.Parameters.AddWithValue("@Name", continent.Name);
                command.ExecuteNonQuery();
                sqlTransaction.Commit();
            }
            catch (Exception ex)
            {
                sqlTransaction.Rollback();
                throw new ContinentRepositoryADOException("ContinentUpdatenADO - error", ex);
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion
    }
}
