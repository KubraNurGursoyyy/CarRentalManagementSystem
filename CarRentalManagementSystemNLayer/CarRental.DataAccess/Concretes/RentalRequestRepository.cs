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
using System.Threading.Tasks;

namespace CarRental.DataAccess.Concretes
{
    public class RentalRequestRepository : IRepository<RentalRequests>, IDisposable
    {
        private string _connectionString;
        private string _dbProviderName;
        private DbProviderFactory _dbProviderFactory;
        private int _rowsAffected, _errorCode;
        private bool _bDisposed;
        public RentalRequestRepository()
        {
            _connectionString = DBHelper.GetConnectionString();
            _dbProviderName = DBHelper.GetConnectionProvider();
            _dbProviderFactory = DbProviderFactories.GetFactory(_dbProviderName);
        }
        public bool Insert(RentalRequests entity)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            try
            {
                var query = new StringBuilder();
                query.Append("INSERT [dbo].[tbl_RentalRequests] ");
                query.Append("([RentalRequestCustomerID],[RequestedSupplierCompanyID],[RequestedVehicleID],[RequestedPickUpDate], [RequestedDropOffDate])");
                query.Append("VALUES ");
                query.Append("(@RentalRequestCustomerID ,@RequestedSupplierCompanyID ,@RequestedVehicleID ,@RequestedPickUpDate, @RequestedDropOffDate)");
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
                            throw new ArgumentNullException("dbCommand" + " The db Insert command for entity [tbl_RentalRequests] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Params
                        DBHelper.AddParameter(dbCommand, "@RentalRequestCustomerID", CsType.Int, ParameterDirection.Input, entity.RentalRequestCustomerId);
                        DBHelper.AddParameter(dbCommand, "@RequestedSupplierCompanyID", CsType.Int, ParameterDirection.Input, entity.RequestedSupplierCompanyId);
                        DBHelper.AddParameter(dbCommand, "@RequestedVehicleID", CsType.Int, ParameterDirection.Input, entity.RequestedVehicleId);
                        DBHelper.AddParameter(dbCommand, "@RequestedPickUpDate", CsType.DateTime, ParameterDirection.Input, entity.RequestedPickUpDate);
                        DBHelper.AddParameter(dbCommand, "@RequestedDropOffDate", CsType.DateTime, ParameterDirection.Input, entity.RequestedDropOffDate);

                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        //Open Connection
                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        //Execute query
                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Inserting Error for entity [tbl_RentalRequests] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                //Return the results of query/ies
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes::RentalRequestRepository::Insert:Error occured.", ex);
            }
        }

        public bool DeleteByID(int id)
        {
            _errorCode = 0;
            _rowsAffected = 0;

            try
            {
                var query = new StringBuilder();
                query.Append("DELETE ");
                query.Append("FROM [dbo].[tbl_RentalRequests] ");
                query.Append("WHERE ");
                query.Append("[RentalRequestID] = @id ");
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
                                "dbCommand" + " The db DeleteById command for entity [tbl_RentalRequests] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Parameters
                        DBHelper.AddParameter(dbCommand, "@id", CsType.Int, ParameterDirection.Input, id);

                        //Output Parameters
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        //Open Connection
                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();
                        //Execute query
                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception(
                                "Deleting Error for entity [tbl_RentalRequests] reported the Database ErrorCode: " +
                                _errorCode);
                    }
                }
                //Return the results of query/ies
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes::RentalRequestRepository::Delete:Error occured.", ex);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IList<RentalRequests> GetAll()
        {
            _errorCode = 0;
            _rowsAffected = 0;

            IList<RentalRequests> Rentalrequested = new List<RentalRequests>();

            try
            {
                var query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("[RentalRequestID], [RentalRequestCustomerID], [RequestedSupplierCompanyID], [RequestedVehicleID], [RequestedPickUpDate], [RequestedDropOffDate]");
                query.Append("FROM [dbo].[tbl_RentalRequests] ");
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
                                "dbCommand" + " The db GetAll command for entity [tbl_RentalRequests] can't be null. ");

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
                                    var entity = new RentalRequests();
                                    entity.RentalRequestId = reader.GetInt32(0);
                                    entity.RentalRequestCustomerId = reader.GetInt32(1);
                                    entity.RequestedSupplierCompanyId = reader.GetInt32(2);
                                    entity.RequestedVehicleId = reader.GetInt32(3);
                                    entity.RequestedPickUpDate = reader.GetDateTime(4);
                                    entity.RequestedDropOffDate = reader.GetDateTime(5);
                                    Rentalrequested.Add(entity);
                                }
                            }

                        }

                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                        {
                            // Throw error.
                            throw new Exception("Getting All Error for entity [tbl_Vehicles] reported the Database ErrorCode: " + _errorCode);

                        }
                    }
                }
                // Return list
                return Rentalrequested;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes::RentalRequestRepository::GetAll:Error occured.", ex);
            }
        }

        public RentalRequests GetByID(int id)
        {
            _errorCode = 0;
            _rowsAffected = 0;

            RentalRequests rentalRequest = null;

            try
            {
                var query = new StringBuilder();
                query.Append("SELECT ");
                query.Append(
                    "[RentalRequestID], [RentalRequestCustomerID], [RequestedSupplierCompanyID], [RequestedVehicleID], [RequestedPickUpDate], [RequestedDropOffDate]");
                query.Append("FROM [dbo].[tbl_RentalRequests] ");
                query.Append("WHERE ");
                query.Append("[RentalRequestID] = @id ");
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
                                "dbCommand" + " The db GetById command for entity [tbl_RentalRequests] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Parameters
                        DBHelper.AddParameter(dbCommand, "@id", CsType.Int, ParameterDirection.Input, id);

                        //Output Parameters
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

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
                                    var entity = new RentalRequests();
                                    entity.RentalRequestId = reader.GetInt32(0);
                                    entity.RentalRequestCustomerId = reader.GetInt32(1);
                                    entity.RequestedSupplierCompanyId = reader.GetInt32(2);
                                    entity.RequestedVehicleId = reader.GetInt32(3);
                                    entity.RequestedPickUpDate = reader.GetDateTime(4);
                                    entity.RequestedDropOffDate = reader.GetDateTime(5);
                                    rentalRequest = entity;
                                    break;
                                }
                            }
                        }

                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                        {
                            // Throw error.
                            throw new Exception("Getting Error for entity [tbl_RentalRequests] reported the Database ErrorCode: " + _errorCode);
                        }
                    }
                }
                return rentalRequest;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes::RentalRequestRepository::GetByID:Error occured.", ex);
            }
        }

        public bool Update(RentalRequests entity)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            try
            {
                var query = new StringBuilder();
                query.Append(" UPDATE [dbo].[tbl_RentalRequests] ");
                query.Append(" SET [RentalRequestCustomerID] = @RentalRequestCustomerID , [RequestedSupplierCompanyID] = @RequestedSupplierCompanyID, [RequestedVehicleID] = @RequestedVehicleID ,[RequestedPickUpDate] = @RequestedPickUpDate, [RequestedDropOffDate] = @RequestedDropOffDate");
                query.Append(" WHERE ");
                query.Append(" [RentalRequestID] = @RentalRequestID ");
                query.Append(" SELECT @intErrorCode = @@ERROR; ");

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
                            throw new ArgumentNullException("dbCommand" + " The db Insert command for entity [tbl_RentalRequests] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Params
                        DBHelper.AddParameter(dbCommand, "@RentalRequestID", CsType.Int, ParameterDirection.Input, entity.RentalRequestId);
                        DBHelper.AddParameter(dbCommand, "@RentalRequestCustomerID", CsType.DateTime, ParameterDirection.Input, entity.RentalRequestCustomerId);
                        DBHelper.AddParameter(dbCommand, "@RequestedSupplierCompanyID", CsType.DateTime, ParameterDirection.Input, entity.RequestedSupplierCompanyId);
                        DBHelper.AddParameter(dbCommand, "@RequestedVehicleID", CsType.DateTime, ParameterDirection.Input, entity.RequestedVehicleId);
                        DBHelper.AddParameter(dbCommand, "@RequestedPickUpDate", CsType.DateTime, ParameterDirection.Input, entity.RequestedPickUpDate);
                        DBHelper.AddParameter(dbCommand, "@RequestedDropOffDate", CsType.DateTime, ParameterDirection.Input, entity.RequestedDropOffDate);


                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        //Open Connection
                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        //Execute query
                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Updating Error for entity [tbl_RentalRequestID] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                //Return the results of query/ies
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes::RentalRequestRepository::Update:Error occured.", ex);
            }
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
