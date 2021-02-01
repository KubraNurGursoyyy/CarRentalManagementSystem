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
    public class VehicleRepository : IRepository<Vehicles>, IDisposable
    {
        private string _connectionString;
        private string _dbProviderName;
        private DbProviderFactory _dbProviderFactory;
        private int _rowsAffected, _errorCode;
        private bool _bDisposed;
        public VehicleRepository()
        {
            _connectionString = DBHelper.GetConnectionString();
            _dbProviderName = DBHelper.GetConnectionProvider();
            _dbProviderFactory = DbProviderFactories.GetFactory(_dbProviderName);
        }
        public bool Insert(Vehicles entity)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            try
            {
                var query = new StringBuilder();
                query.Append("INSERT [dbo].[tbl_Vehicles] ");
                query.Append("([VehiclesCompanyID],[VehicleName], [VehicleModel], [VehiclesInstantKm], [HasAirbag], [TrunkVolume], [SeatingCapacity], [DailyRentalPrice], [AgeLimitForDrivingThisCar], [KmLimitPerDay], [RequiredDrivingLicenseAge]) ");
                query.Append("VALUES ");
                query.Append(
                    "( @VehiclesCompanyID ,@VehicleName, @VehicleModel, @VehiclesInstantKm, @HasAirbag, @TrunkVolume, @SeatingCapacity, @DailyRentalPrice, @AgeLimitForDrivingThisCar, @KmLimitPerDay, @RequiredDrivingLicenseAge ) ");
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
                            throw new ArgumentNullException("dbCommand" + " The db Insert command for entity [tbl_Vehicles] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Params
                        DBHelper.AddParameter(dbCommand, "@VehiclesCompanyID", CsType.Int, ParameterDirection.Input, entity.VehiclesCompanyId);
                        DBHelper.AddParameter(dbCommand, "@VehicleName", CsType.String, ParameterDirection.Input, entity.VehicleName);
                        DBHelper.AddParameter(dbCommand, "@VehicleModel", CsType.String, ParameterDirection.Input, entity.VehicleModel);
                        DBHelper.AddParameter(dbCommand, "@VehiclesInstantKm", CsType.Int, ParameterDirection.Input, entity.VehiclesInstantKm);
                        DBHelper.AddParameter(dbCommand, "@HasAirbag", CsType.Boolean, ParameterDirection.Input, entity.HasAirbag);
                        DBHelper.AddParameter(dbCommand, "@TrunkVolume", CsType.Int, ParameterDirection.Input, entity.TrunkVolume);
                        DBHelper.AddParameter(dbCommand, "@SeatingCapacity", CsType.Int, ParameterDirection.Input, entity.SeatingCapacity);
                        DBHelper.AddParameter(dbCommand, "@DailyRentalPrice", CsType.Decimal, ParameterDirection.Input, entity.DailyRentalPrice);
                        DBHelper.AddParameter(dbCommand, "@AgeLimitForDrivingThisCar", CsType.Int, ParameterDirection.Input, entity.AgeLimitForDrivingThisCar);
                        DBHelper.AddParameter(dbCommand, "@KmLimitPerDay", CsType.Int, ParameterDirection.Input, entity.KmLimitPerDay);
                        DBHelper.AddParameter(dbCommand, "@RequiredDrivingLicenseAge", CsType.Int, ParameterDirection.Input, entity.RequiredDrivingLicenseAge);

                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        //Open Connection
                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        //Execute query
                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Inserting Error for entity [tbl_Vehicles] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                //Return the results of query/ies
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes::VehiclesRepository::Insert:Error occured.", ex);
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
                query.Append("FROM [dbo].[tbl_Vehicles] ");
                query.Append("WHERE ");
                query.Append("[VehicleID] = @id ");
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
                                "dbCommand" + " The db DeleteById command for entity [tbl_Vehicles] can't be null. ");

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
                                "Deleting Error for entity [tbl_Vehicles] reported the Database ErrorCode: " +
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

        public IList<Vehicles> GetAll()
        {
            _errorCode = 0;
            _rowsAffected = 0;

            IList<Vehicles> vehicles = new List<Vehicles>();

            try
            {
                var query = new StringBuilder();
                query.Append("SELECT ");
                query.Append(" [VehiclesCompanyID], [VehicleID], [VehicleName], [VehicleModel], [VehiclesInstantKm], [HasAirbag], [TrunkVolume], [SeatingCapacity], [DailyRentalPrice], [AgeLimitForDrivingThisCar], [KmLimitPerDay], [RequiredDrivingLicenseAge] ");
                query.Append("FROM [dbo].[tbl_Vehicles] ");
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
                                "dbCommand" + " The db GetAll command for entity [tbl_Vehicles] can't be null. ");

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
                                    var entity = new Vehicles();
                                    entity.VehiclesCompanyId = reader.GetInt32(0);
                                    entity.VehicleId = reader.GetInt32(1);
                                    entity.VehicleName = reader.GetString(2);
                                    entity.VehicleModel = reader.GetString(3);
                                    entity.VehiclesInstantKm = reader.GetInt32(4);
                                    entity.HasAirbag = reader.GetBoolean(5);
                                    entity.TrunkVolume = reader.GetInt32(6);
                                    entity.SeatingCapacity = reader.GetInt32(7);
                                    entity.DailyRentalPrice = reader.GetDecimal(8);
                                    entity.AgeLimitForDrivingThisCar = reader.GetInt32(9);
                                    entity.KmLimitPerDay = reader.GetInt32(10);
                                    entity.RequiredDrivingLicenseAge = reader.GetInt32(11);
                                    vehicles.Add(entity);
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
                return vehicles;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes::RentedVehiclesRepository::GetAll:Error occured.", ex);
            }
        }

        public Vehicles GetByID(int id)
        {
            _errorCode = 0;
            _rowsAffected = 0;

            Vehicles vehicle = null;

            try
            {
                var query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("[VehiclesCompanyID],[VehicleID], [VehicleName], [VehicleModel], [VehiclesInstantKm], [HasAirbag], [TrunkVolume], [SeatingCapacity], [DailyRentalPrice], [AgeLimitForDrivingThisCar], [KmLimitPerDay], [RequiredDrivingLicenseAge]");
                query.Append("FROM [dbo].[tbl_Vehicles] ");
                query.Append("WHERE ");
                query.Append("[VehicleID] = @id ");
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
                                "dbCommand" + " The db GetById command for entity [tbl_Vehicles] can't be null. ");

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
                                    var entity = new Vehicles();
                                    entity.VehicleId = reader.GetInt32(1);
                                    entity.VehiclesCompanyId = reader.GetInt32(0);
                                    entity.VehicleName = reader.GetString(2);
                                    entity.VehicleModel = reader.GetString(3);
                                    entity.VehiclesInstantKm = reader.GetInt32(4);
                                    entity.HasAirbag = reader.GetBoolean(5);
                                    entity.TrunkVolume = reader.GetInt32(6);
                                    entity.SeatingCapacity = reader.GetInt32(7);
                                    entity.DailyRentalPrice = reader.GetDecimal(8);
                                    entity.AgeLimitForDrivingThisCar = reader.GetInt32(9);
                                    entity.KmLimitPerDay = reader.GetInt32(10);
                                    entity.RequiredDrivingLicenseAge = reader.GetInt32(11);
                                    vehicle = entity;
                                    break;
                                }
                            }
                        }

                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                        {
                            // Throw error.
                            throw new Exception("Getting By ID Error for entity [tbl_Vehicles] reported the Database ErrorCode: " + _errorCode);
                        }
                    }
                }

                return vehicle;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes::RentedVehiclesRepository::GetByID:Error occured.", ex);
            }
        }

        public bool Update(Vehicles entity)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            try
            {
                var query = new StringBuilder();
                query.Append(" UPDATE [dbo].[tbl_Vehicle] ");
                query.Append(" SET [VehiclesCompanyID] = @VehiclesCompanyID ,[VehicleName] = @VehicleName, [VehicleModel] = @VehicleModel, [VehiclesInstantKm] =  @VehiclesInstantKm, [HasAirbag] = @HasAirbag, [TrunkVolume] = @TrunkVolume, [SeatingCapacity] = @SeatingCapacity, [DailyRentalPrice] = @DailyRentalPrice, [AgeLimitForDrivingThisCar] = @AgeLimitForDrivingThisCar, [KmLimitPerDay] = @KmLimitPerDay, [RequiredDrivingLicenseAge] = @RequiredDrivingLicenseAge");
                query.Append(" WHERE ");
                query.Append(" [VehicleID] = @VehicleID ");
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
                            throw new ArgumentNullException("dbCommand" + " The db Insert command for entity [tbl_Vehicles] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Params
                        DBHelper.AddParameter(dbCommand, "@VehicleID", CsType.Int, ParameterDirection.Input, entity.VehicleId);
                        DBHelper.AddParameter(dbCommand, "@VehiclesCompanyID", CsType.Int, ParameterDirection.Input, entity.VehiclesCompanyId);
                        DBHelper.AddParameter(dbCommand, "@VehicleName", CsType.String, ParameterDirection.Input, entity.VehicleName);
                        DBHelper.AddParameter(dbCommand, "@VehicleModel", CsType.String, ParameterDirection.Input, entity.VehicleModel);
                        DBHelper.AddParameter(dbCommand, "@VehiclesInstantKm", CsType.Int, ParameterDirection.Input, entity.VehiclesInstantKm);
                        DBHelper.AddParameter(dbCommand, "@HasAirbag", CsType.Boolean, ParameterDirection.Input, entity.HasAirbag);
                        DBHelper.AddParameter(dbCommand, "@TrunkVolume", CsType.Int, ParameterDirection.Input, entity.TrunkVolume);
                        DBHelper.AddParameter(dbCommand, "@SeatingCapacity", CsType.Int, ParameterDirection.Input, entity.SeatingCapacity);
                        DBHelper.AddParameter(dbCommand, "@DailyRentalPrice", CsType.Decimal, ParameterDirection.Input, entity.DailyRentalPrice);
                        DBHelper.AddParameter(dbCommand, "@AgeLimitForDrivingThisCar", CsType.Int, ParameterDirection.Input, entity.AgeLimitForDrivingThisCar);
                        DBHelper.AddParameter(dbCommand, "@KmLimitPerDay", CsType.Int, ParameterDirection.Input, entity.KmLimitPerDay);
                        DBHelper.AddParameter(dbCommand, "@RequiredDrivingLicenseAge", CsType.Int, ParameterDirection.Input, entity.RequiredDrivingLicenseAge);


                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        //Open Connection
                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        //Execute query
                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Updating Error for entity [tbl_Vehicles] reported the Database ErrorCode: " + _errorCode);
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
