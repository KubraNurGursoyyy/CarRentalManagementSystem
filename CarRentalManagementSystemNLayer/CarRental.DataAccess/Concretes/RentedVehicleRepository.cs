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
    public class RentedVehicleRepository : IRepository<RentedVehicles>, IDisposable
    {
        private string _connectionString;
        private string _dbProviderName;
        private DbProviderFactory _dbProviderFactory;
        private int _rowsAffected, _errorCode;
        private bool _bDisposed;

        public RentedVehicleRepository()
        {
            _connectionString = DBHelper.GetConnectionString();
            _dbProviderName = DBHelper.GetConnectionProvider();
            _dbProviderFactory = DbProviderFactories.GetFactory(_dbProviderName);
        }
        public bool Insert(RentedVehicles entity)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            try
            {
                var query = new StringBuilder();
                query.Append("INSERT [dbo].[tbl_RentedVehicles] ");
                query.Append("([RentedVehicleID],[SupplierCompanyID],[DriverCustomerID],[PickUpDate], [DropOffDate], [VehiclesPickUpKm], [VehiclesDropOffKm], [RentalPrice])");
                query.Append("VALUES ");
                query.Append("( @RentedVehicleID ,@SupplierCompanyID ,@DriverCustomerID ,@PickUpDate, @DropOffDate, @VehiclesPickUpKm, @VehiclesDropOffKm, @RentalPrice)");
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
                            throw new ArgumentNullException("dbCommand" + " The db Insert command for entity [tbl_RentedVehicles] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Params
                        DBHelper.AddParameter(dbCommand, "@RentedVehicleID", CsType.Int, ParameterDirection.Input, entity.RentedVehicleId);
                        DBHelper.AddParameter(dbCommand, "@SupplierCompanyID", CsType.Int, ParameterDirection.Input, entity.SupplierCompanyId);
                        DBHelper.AddParameter(dbCommand, "@DriverCustomerID", CsType.Int, ParameterDirection.Input, entity.DriverCustomerId);
                        DBHelper.AddParameter(dbCommand, "@PickUpDate", CsType.DateTime, ParameterDirection.Input, entity.PickUpDate);
                        DBHelper.AddParameter(dbCommand, "@DropOffDate", CsType.DateTime, ParameterDirection.Input, entity.DropOffDate);
                        DBHelper.AddParameter(dbCommand, "@VehiclesPickUpKm", CsType.Int, ParameterDirection.Input, entity.VehiclesPickUpKm);
                        DBHelper.AddParameter(dbCommand, "@VehiclesDropOffKm", CsType.Int, ParameterDirection.Input, entity.DropOffDate);
                        DBHelper.AddParameter(dbCommand, "@RentalPrice", CsType.Decimal, ParameterDirection.Input, entity.RentalPrice);

                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        //Open Connection
                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        //Execute query
                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Inserting Error for entity [tbl_RentedVehicles] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                //Return the results of query/ies
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes::RentedVehiclesRepository::Insert:Error occured.", ex);
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
                query.Append("FROM [dbo].[tbl_RentedVehicles] ");
                query.Append("WHERE ");
                query.Append("[RentID] = @id ");
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
                                "dbCommand" + " The db DeleteByID command for entity [tbl_RentedVehicles] can't be null. ");

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
                                "Deleting Error for entity [tbl_RentedVehicles] reported the Database ErrorCode: " +
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
                throw new Exception("CarRental.DataAccess.Concretes::RentedVehiclesRepository::Delete:Error occured.", ex);
            }

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public IList<RentedVehicles> GetAll()
        {
            _errorCode = 0;
            _rowsAffected = 0;

            IList<RentedVehicles> rentedvehicles = new List<RentedVehicles>();

            try
            {
                var query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("[RentID], [RentedVehicleID],[SupplierCompanyID],[DriverCustomerID], [PickUpDate], [DropOffDate], [VehiclesPickUpKm], [VehiclesDropOffKm], [RentalPrice]");
                query.Append("FROM [dbo].[tbl_RentedVehicles] ");
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
                                "dbCommand" + " The db GetAll command for entity [tbl_RentedVehicles] can't be null. ");

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
                                    var entity = new RentedVehicles();
                                    entity.RentId = reader.GetInt32(0);
                                    entity.RentedVehicleId = reader.GetInt32(1);
                                    entity.SupplierCompanyId = reader.GetInt32(2);
                                    entity.DriverCustomerId = reader.GetInt32(3);
                                    entity.PickUpDate = reader.GetDateTime(4);
                                    entity.DropOffDate = reader.GetDateTime(5);
                                    entity.VehiclesPickUpKm = reader.GetInt32(6);
                                    entity.VehiclesDropOffKm = reader.GetInt32(7);
                                    entity.RentalPrice = reader.GetDecimal(8);
                                    rentedvehicles.Add(entity);
                                }
                            }

                        }

                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                        {
                            // Throw error.
                            throw new Exception("Getting All Error for entity [tbl_RentedVehicles] reported the Database ErrorCode: " + _errorCode);

                        }
                    }
                }
                // Return list
                return rentedvehicles;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes::RentedVehiclesRepository::GetAll:Error occured.", ex);
            }
        }

        public RentedVehicles GetByID(int id)
        {
            _errorCode = 0;
            _rowsAffected = 0;

            RentedVehicles rentedvehicle = null;

            try
            {
                var query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("[RentedVehicleID],[SupplierCompanyID],[DriverCustomerID], [PickUpDate], [DropOffDate], [VehiclesPickUpKm], [VehiclesDropOffKm], [RentalPrice]");
                query.Append("FROM [dbo].[tbl_RentedVehicles] ");
                query.Append("WHERE ");
                query.Append("[RentID] = @id ");
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
                                "dbCommand" + " The db GetByID command for entity [tbl_RentedVehicles] can't be null. ");

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
                                    var entity = new RentedVehicles();
                                    entity.RentId = reader.GetInt32(0);
                                    entity.RentedVehicleId = reader.GetInt32(1);
                                    entity.SupplierCompanyId = reader.GetInt32(2);
                                    entity.DriverCustomerId = reader.GetInt32(3);
                                    entity.PickUpDate = reader.GetDateTime(4);
                                    entity.DropOffDate = reader.GetDateTime(5);
                                    entity.VehiclesPickUpKm = reader.GetInt32(6);
                                    entity.VehiclesDropOffKm = reader.GetInt32(7);
                                    entity.RentalPrice = reader.GetDecimal(8);
                                    rentedvehicle = entity;
                                    break;
                                }
                            }
                        }

                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                        {
                            // Throw error.
                            throw new Exception("Geeting by ID Error for entity [tbl_RentedVehicles] reported the Database ErrorCode: " + _errorCode);
                        }
                    }
                }
                return rentedvehicle;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes::RentedVehiclesRepository::GetByID:Error occured.", ex);
            }

        }

        public bool Update(RentedVehicles entity)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            try
            {
                var query = new StringBuilder();
                query.Append(" UPDATE [dbo].[tbl_Vehicle] ");
                query.Append(" SET [RentedVehicleID] = @RentedVehicleID ,[SupplierCompanyID] = @SupplierCompanyID,[DriverCustomerID] = @DriverCustomerID, [PickUpDate] = @PickUpDate, [DropOffDate] = @DropOffDate, [VehiclesPickUpKm] =  @VehiclesPickUpKm, [VehiclesDropOffKm] = @VehiclesDropOffKm, [RentalPrice] = @RentalPrice");
                query.Append(" WHERE ");
                query.Append(" [RentID] = @RentID ");
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
                            throw new ArgumentNullException("dbCommand" + " The db Update command for entity [tbl_RentedVehicles] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Params
                        DBHelper.AddParameter(dbCommand, "@RentID", CsType.Int, ParameterDirection.Input, entity.RentId);
                        DBHelper.AddParameter(dbCommand, "@RentedVehicleID", CsType.Int, ParameterDirection.Input, entity.RentedVehicleId);
                        DBHelper.AddParameter(dbCommand, "@SupplierCompanyID", CsType.Int, ParameterDirection.Input, entity.SupplierCompanyId);
                        DBHelper.AddParameter(dbCommand, "@DriverCustomerID", CsType.Int, ParameterDirection.Input, entity.DriverCustomerId);
                        DBHelper.AddParameter(dbCommand, "@PickUpDate", CsType.DateTime, ParameterDirection.Input, entity.PickUpDate);
                        DBHelper.AddParameter(dbCommand, "@DropOffDate", CsType.DateTime, ParameterDirection.Input, entity.DropOffDate);
                        DBHelper.AddParameter(dbCommand, "@VehiclesPickUpKm", CsType.Int, ParameterDirection.Input, entity.VehiclesPickUpKm);
                        DBHelper.AddParameter(dbCommand, "@VehiclesDropOffKm", CsType.Int, ParameterDirection.Input, entity.DropOffDate);
                        DBHelper.AddParameter(dbCommand, "@RentalPrice", CsType.Decimal, ParameterDirection.Input, entity.RentalPrice);



                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        //Open Connection
                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        //Execute query
                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Updating Error for entity [tbl_RentedVehicles] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                //Return the results of query/ies
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes::RentedVehiclesRepository::Update:Error occured.", ex);
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
