using CarRental.Commons.Concretes.Data;
using CarRental.Commons.Concretes.Helper;
using CarRental.Commons.Concretes.Logger;
using CarRental.DataAccess.Abstracts;
using CarRental.Models.Concretes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace CarRental.DataAccess.Concretes
{
    public class CustomerRepository : IRepository<Customers>, IDisposable
    {
        private string _connectionString;
        private string _dbProviderName;
        private DbProviderFactory _dbProviderFactory;
        private int _rowsAffected, _errorCode;
        private bool _bDisposed;

        public CustomerRepository()
        {
            _connectionString = DBHelper.GetConnectionString();
            _dbProviderName = DBHelper.GetConnectionProvider();
            _dbProviderFactory = DbProviderFactories.GetFactory(_dbProviderName);
        }

        public bool DeleteByID(int id)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            try
            {
                var query = new StringBuilder();
                query.Append("DELETE ");
                query.Append("FROM [dbo].[tbl_Customers] ");
                query.Append("WHERE ");
                query.Append("[CustomerID] = @id ");
                query.Append("SELECT @intErrorCode=@@ERROR; ");


                var commandText = query.ToString();
                query.Clear();

                using (var dbConnection = _dbProviderFactory.CreateConnection())
                {
                    if (dbConnection == null)
                        throw new ArgumentNullException("dbConnection", "The db connection can't be null.");

                    dbConnection.ConnectionString = _connectionString;

                    using (var dbCommand = _dbProviderFactory.CreateCommand())
                    {
                        if (dbCommand == null)
                            throw new ArgumentNullException("dbCommand" + " The db Delete command for entity [tbl_Customers] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Params
                        DBHelper.AddParameter(dbCommand, "@id", CsType.Int, ParameterDirection.Input, id);

                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Deleting Error for entity [tbl_Customers] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes:CustomerRepository::DeleteByID:Error occured.", ex);
            }
        }

        public IList<Customers> GetAll()
        {
            _errorCode = 0;
            _rowsAffected = 0;

            IList<Customers> customers = new List<Customers>();

            try
            {
                var query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("[CustomerID],[CustomerName],[CustomerSurname],[CustomerEmail],[CustomerPassword],[CustomerPhoneNumber] ,[CustomerAddress] ,[SecondPhoneNumber] ,[NationalIDCard] ");
                query.Append("FROM [dbo].[tbl_Customers] ");
                query.Append("SELECT @intErrorCode=@@ERROR; ");

                var commandText = query.ToString();
                query.Clear();

                using (var dbConnection = _dbProviderFactory.CreateConnection())
                {
                    if (dbConnection == null)
                        throw new ArgumentNullException("dbConnection", "The db connection can't be null.");

                    dbConnection.ConnectionString = _connectionString;

                    using (var dbCommand = _dbProviderFactory.CreateCommand())
                    {
                        if (dbCommand == null)
                            throw new ArgumentNullException(
                                "dbCommand" + " The db GetAll command for entity [tbl_Customers] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Parameters - None

                        //Output Parameters
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int,
                        ParameterDirection.Output, null);

                        //Open Connection
                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        //Execute query.
                        using (var reader = dbCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var entity = new Customers();
                                    entity.CustomerId = Convert.ToInt32(reader.GetValue(0));
                                    entity.CustomerName = reader.GetValue(1).ToString();
                                    entity.CustomerSurname = reader.GetValue(2).ToString();
                                    entity.CustomerEmail = reader.GetValue(3).ToString();
                                    entity.CustomerPassword = reader.GetValue(4).ToString();
                                    entity.CustomerPhoneNumber = reader.GetValue(5).ToString();
                                    entity.CustomerAddress = reader.GetValue(6).ToString();
                                    entity.SecondPhoneNumber = reader.GetValue(7).ToString();
                                    entity.NationalIdcard = reader.GetValue(8).ToString();
                                   
                                    customers.Add(entity);
                                }
                            }

                        }

                       _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                        {
                       //  Throw error.
                         throw new Exception("Getting All Error for entity [tbl_Customer] reported the Database ErrorCode: " + _errorCode);
                        }
                    }
                }
                // Return list
                return customers;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes:CustomerRepository::GetAll:Error occured.", ex);
            }
        }

        public Customers GetByID(int id)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            Customers customers = null;

            try
            {
                var query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("([CustomerName],[CustomerSurname],[CustomerEmail],[CustomerPassword],[CustomerPhoneNumber] ,[CustomerAddress] ,[SecondPhoneNumber] ,[NationalIDCard]) ");
                query.Append("FROM [dbo].[tbl_Customers] ");
                query.Append("WHERE ");
                query.Append("[CustomerID] = @id ");
                query.Append("SELECT @intErrorCode=@@ERROR;");

                var commandText = query.ToString();
                query.Clear();

                using (var dbConnection = _dbProviderFactory.CreateConnection())
                {
                    if (dbConnection == null)
                        throw new ArgumentNullException("dbConnection", "The db connection can't be null.");

                    dbConnection.ConnectionString = _connectionString;

                    using (var dbCommand = _dbProviderFactory.CreateCommand())
                    {
                        if (dbCommand == null)
                            throw new ArgumentNullException("dbCommand" + " The db GetByID command for entity [tbl_Customers] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Parameters
                        DBHelper.AddParameter(dbCommand, "@id", CsType.Int, ParameterDirection.Input, id);

                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        //Execute query.
                        using (var reader = dbCommand.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    var entity = new Customers();
                                 
                                    entity.CustomerName = reader.GetString(1);
                                    entity.CustomerSurname = reader.GetString(2);
                                    entity.CustomerEmail = reader.GetString(3);
                                    entity.CustomerPassword = reader.GetString(4);
                                    entity.CustomerPhoneNumber = reader.GetString(5);
                                    entity.CustomerAddress = reader.GetString(6);
                                    entity.SecondPhoneNumber = reader.GetString(7);
                                    entity.NationalIdcard = reader.GetString(8);   
                                    entity.CustomerId = reader.GetInt32(0);
                                    customers = entity;
                                    break;
                                }
                            }
                        }

                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("GetByID Error for entity [tbl_Customers] reported the Database ErrorCode: " + _errorCode);
                    }
                }

                return customers;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes:CustomerRepository:GetByID::Error occured.", ex);
            }
        }

        public bool Insert(Customers entity)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            try
            {
                var query = new StringBuilder();
                query.Append("INSERT [dbo].[tbl_Customers] ");
                query.Append("( [CustomerName],[CustomerSurname],[CustomerEmail],[CustomerPassword],[CustomerPhoneNumber] ,[CustomerAddress] ,[SecondPhoneNumber] ,[NationalIDCard]) ");
                query.Append("VALUES ");
                query.Append("( @CustomerName, @CustomerSurname, @CustomerEmail, @CustomerPassword, @CustomerPhoneNumber, @CustomerAddress, @SecondPhoneNumber , @NationalIDCard ) ");
                query.Append("SELECT @intErrorCode=@@ERROR;");

                var commandText = query.ToString();
                query.Clear();

                using (var dbConnection = _dbProviderFactory.CreateConnection())
                {
                    if (dbConnection == null)
                        throw new ArgumentNullException("dbConnection", "The db connection can't be null.");

                    dbConnection.ConnectionString = _connectionString;

                    using (var dbCommand = _dbProviderFactory.CreateCommand())
                    {
                        if (dbCommand == null)
                            throw new ArgumentNullException("dbCommand" + " The db Insert command for entity [tbl_Customers] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Params
                        DBHelper.AddParameter(dbCommand, "@CustomerName", CsType.String, ParameterDirection.Input, entity.CustomerName);
                        DBHelper.AddParameter(dbCommand, "@CustomerSurname", CsType.String, ParameterDirection.Input, entity.CustomerSurname);
                        DBHelper.AddParameter(dbCommand, "@CustomerEmail", CsType.String, ParameterDirection.Input, entity.CustomerEmail);
                        DBHelper.AddParameter(dbCommand, "@CustomerPassword", CsType.String, ParameterDirection.Input, entity.CustomerPassword);
                        DBHelper.AddParameter(dbCommand, "@CustomerPhoneNumber", CsType.String, ParameterDirection.Input, entity.CustomerPhoneNumber);
                        DBHelper.AddParameter(dbCommand, "@CustomerAddress", CsType.String, ParameterDirection.Input, entity.CustomerAddress);
                        DBHelper.AddParameter(dbCommand, "@SecondPhoneNumber", CsType.String, ParameterDirection.Input, entity.SecondPhoneNumber);
                        DBHelper.AddParameter(dbCommand, "@NationalIDCard", CsType.String, ParameterDirection.Input, entity.NationalIdcard);


                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        //Open Connection
                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        //Execute query
                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Inserting Error for entity [tbl_Customer] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes:CustomerRepository:Insert::Error occured.", ex);
            }
        }

        public bool Update(Customers entity)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            try
            {
                var query = new StringBuilder();
                query.Append("UPDATE [dbo].[tbl_Customers] ");
                query.Append("(SET [CustomerName] = @CustomerName," +
                "[CustomerSurname] = @CustomerSurname ," +
                "[CustomerEmail] = @CustomerEmail," +
                    "[CustomerPassword] = @CustomerPassword," +
                    "[CustomerPhoneNumber] = @CustomerPhoneNumber," +
                    "[CustomerAddress] = @CustomerAddress," +
                    "[SecondPhoneNumber] = @SecondPhoneNumber," +
                    "[NationalIDCard] = @NationalIDCard) ");
                query.Append("WHERE ");
                query.Append("[CustomerID] = @CustomerID ");
                query.Append("SELECT @intErrorCode=@@ERROR;");

                var commandText = query.ToString();
                query.Clear();

                using (var dbConnection = _dbProviderFactory.CreateConnection())
                {
                    if (dbConnection == null)
                        throw new ArgumentNullException("dbConnection", "The db connection can't be null.");

                    dbConnection.ConnectionString = _connectionString;

                    using (var dbCommand = _dbProviderFactory.CreateCommand())
                    {
                        if (dbCommand == null)
                            throw new ArgumentNullException("dbCommand" + " The db Update command for entity [tbl_Customers] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Params
                        DBHelper.AddParameter(dbCommand, "@CustomerID", CsType.Int, ParameterDirection.Input, entity.CustomerId);
                        DBHelper.AddParameter(dbCommand, "@CustomerName", CsType.String, ParameterDirection.Input, entity.CustomerName);
                        DBHelper.AddParameter(dbCommand, "@CustomerSurname", CsType.String, ParameterDirection.Input, entity.CustomerSurname);
                        DBHelper.AddParameter(dbCommand, "@CustomerEmail", CsType.String, ParameterDirection.Input, entity.CustomerEmail);
                        DBHelper.AddParameter(dbCommand, "@CustomerPassword", CsType.String, ParameterDirection.Input, entity.CustomerPassword);
                        DBHelper.AddParameter(dbCommand, "@CustomerPhoneNumber", CsType.String, ParameterDirection.Input, entity.CustomerPhoneNumber);
                        DBHelper.AddParameter(dbCommand, "@CustomerAddress", CsType.String, ParameterDirection.Input, entity.CustomerAddress);
                        DBHelper.AddParameter(dbCommand, "@SecondPhoneNumber", CsType.String, ParameterDirection.Input, entity.SecondPhoneNumber);
                        DBHelper.AddParameter(dbCommand, "@NationalIDCard", CsType.String, ParameterDirection.Input, entity.NationalIdcard);

                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Updating Error for entity [tbl_Customers] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes: " + entity.GetType().ToString() + "::Update:Error occured.", ex);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool bDisposing)
        {
            // Check the Dispose method called before.
            if (!_bDisposed)
            {
                if (bDisposing)
                {
                    // Clean the resources used.
                    _dbProviderFactory = null;
                }

                _bDisposed = true;
            }
        }
    }
}
