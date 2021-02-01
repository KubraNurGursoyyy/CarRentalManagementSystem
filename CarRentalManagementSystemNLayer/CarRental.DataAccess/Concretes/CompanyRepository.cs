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
    public class CompanyRepository : IRepository<Companies>, IDisposable
    {
        private string _connectionString;
        private string _dbProviderName;
        private DbProviderFactory _dbProviderFactory;
        private int _rowsAffected, _errorCode;
        private bool _bDisposed;

        public CompanyRepository()
        {
            _connectionString = DBHelper.GetConnectionString();
            _dbProviderName = DBHelper.GetConnectionProvider();
            _dbProviderFactory = DbProviderFactories.GetFactory(_dbProviderName);
        }
        public bool Insert(Companies entity)
        {
            _rowsAffected = 0;
            _errorCode = 0;
            try
            {
                var query = new StringBuilder();
                query.Append("INSERT [dbo].[tbl_Companies] ");
                query.Append("([CompanyEmail],[CompanyPassword],[CompanyName],[CompanyCity],[CompanyAdress]) ");
                query.Append("VALUES ");
                query.Append(
                    "( @CompanyEmail, @CompanyPassword, @CompanyName, @CompanyCity, @CompanyAdress) ");
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
                            throw new ArgumentNullException("dbCommand" + " The db Insert command for entity [tbl_Companies] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Params
                        DBHelper.AddParameter(dbCommand, "@CompanyEmail", CsType.String, ParameterDirection.Input, entity.CompanyEmail);
                        DBHelper.AddParameter(dbCommand, "@CompanyPassword", CsType.String, ParameterDirection.Input, entity.CompanyPassword);
                        DBHelper.AddParameter(dbCommand, "@CompanyName", CsType.String, ParameterDirection.Input, entity.CompanyName);
                        DBHelper.AddParameter(dbCommand, "@CompanyCity", CsType.String, ParameterDirection.Input, entity.CompanyCity);
                        DBHelper.AddParameter(dbCommand, "@CompanyAdress", CsType.String, ParameterDirection.Input, entity.CompanyAdress);
                       
                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Inserting Error for entity [tbl_Companies] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes: " + entity.GetType().ToString() + "::Insert:Error occured.", ex);
            }
        }

        public bool DeleteByID(int id)
        {
            _rowsAffected = 0;
            _errorCode = 0;
            try
            {
                var query = new StringBuilder();
                query.Append("DELETE ");
                query.Append("FROM [dbo].[tbl_Companies] ");
                query.Append("WHERE ");
                query.Append("[CompanyID] = @id ");
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
                            throw new ArgumentNullException("dbCommand" + " The db Delete command for entity [tbl_Companies] can't be null. ");

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
                            throw new Exception("Deleting Error for entity [tbl_Companies] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes:CompanyRepository::DeleteByID:Error occured.", ex);
            }
        }

        public IList<Companies> GetAll()
        {
            _rowsAffected = 0;
            _errorCode = 0;

            IList<Companies> companies = new List<Companies>();

            try
            {
                var query = new StringBuilder();
                query.Append("SELECT [CompanyID],[CompanyEmail],[CompanyPassword],[CompanyName],[CompanyCity],[CompanyAdress] FROM [dbo].[tbl_Companies] SELECT @intErrorCode=@@ERROR ");
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
                            throw new ArgumentNullException("dbCommand" + " The db GetAll command for entity [tbl_Companies] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

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
                                    var entity = new Companies();
                                    entity.CompanyId = reader.GetInt32(0);
                                    entity.CompanyEmail = reader.GetString(1);
                                    entity.CompanyPassword = reader.GetString(2);
                                    entity.CompanyName = reader.GetString(3);
                                    entity.CompanyCity = reader.GetString(4);
                                    entity.CompanyAdress = reader.GetString(5);
                                    companies.Add(entity);
                                }
                            }
                        }

                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Getting All Error for entity [tbl_Companies] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                return companies;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes:CompanyRepository:GetAll::Error occured.", ex);
            }
        }

        public Companies GetByID(int id)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            Companies companies = null;

            try
            {
                var query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("[CompanyID],[CompanyEmail],[CompanyPassword],[CompanyName],[CompanyCity],[CompanyAdress]");
                query.Append("FROM [dbo].[tbl_Companies] ");
                query.Append("WHERE ");
                query.Append("[CompanyID] = @id ");
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
                            throw new ArgumentNullException("dbCommand" + " The db GetByID command for entity [tbl_Companies] can't be null. ");

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
                                    var entity = new Companies();
                                    entity.CompanyId = reader.GetInt32(0);
                                    entity.CompanyEmail = reader.GetString(1);
                                    entity.CompanyPassword = reader.GetString(2);
                                    entity.CompanyName = reader.GetString(3);
                                    entity.CompanyCity = reader.GetString(4);
                                    entity.CompanyAdress = reader.GetString(5);
                                   
                                    companies = entity;
                                    break;
                                }
                            }
                        }

                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("GetByID Error for entity [tbl_Companies] reported the Database ErrorCode: " + _errorCode);
                    }
                }

                companies.Vehicles = new VehicleRepository().GetAll().Where(x => x.VehiclesCompanyId.Equals(companies.CompanyId)).ToList();
                companies.Employees = new EmployeeRepository().GetAll().Where(x => x.EmployeesCompanyId.Equals(companies.CompanyId)).ToList();
                companies.RentalRequests = new RentalRequestRepository().GetAll().Where(x => x.RequestedSupplierCompanyId.Equals(companies.CompanyId)).ToList();
                companies.RentedVehicles = new RentedVehicleRepository().GetAll().Where(x => x.SupplierCompanyId.Equals(companies.CompanyId)).ToList();
                return companies;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes:CompanyRepository:GetByID::Insert:Error occured.", ex);
            }
        }

        public bool Update(Companies entity)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            try
            {
                var query = new StringBuilder();
                query.Append("UPDATE [dbo].[tbl_Companies] ");
                query.Append("(SET [CompanyEmail] = @CompanyEmail,[CompanyPassword] = @CompanyPassword ,[CompanyName] = @CompanyName," +
                    "[CompanyCity] = @CompanyCity," +
                    "[CompanyAdress] = @CompanyAdress," );
                query.Append("WHERE ");
                query.Append("[CompanyID] = @CompanyID ");
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
                            throw new ArgumentNullException("dbCommand" + " The db Update command for entity [tbl_Companies] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Params
                        DBHelper.AddParameter(dbCommand, "@CompanyID", CsType.Int, ParameterDirection.Input, entity.CompanyId);
                        DBHelper.AddParameter(dbCommand, "@CompanyEmail", CsType.String, ParameterDirection.Input, entity.CompanyEmail);
                        DBHelper.AddParameter(dbCommand, "@CompanyPassword", CsType.String, ParameterDirection.Input, entity.CompanyPassword);
                        DBHelper.AddParameter(dbCommand, "@CompanyName", CsType.String, ParameterDirection.Input, entity.CompanyName);
                        DBHelper.AddParameter(dbCommand, "@CompanyCity", CsType.String, ParameterDirection.Input, entity.CompanyCity);
                        DBHelper.AddParameter(dbCommand, "@CompanyAdress", CsType.String, ParameterDirection.Input, entity.CompanyAdress);

                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Updating Error for entity [tbl_Companies] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes: " + entity.GetType().ToString() + "::Insert:Error occured.", ex);
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
