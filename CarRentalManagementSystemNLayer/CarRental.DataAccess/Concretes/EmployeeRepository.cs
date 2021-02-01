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
    public class EmployeeRepository : IRepository<Employees>, IDisposable
    {
        private string _connectionString;
        private string _dbProviderName;
        private DbProviderFactory _dbProviderFactory;
        private int _rowsAffected, _errorCode;
        private bool _bDisposed;

        public EmployeeRepository()
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
                query.Append("FROM [dbo].[tbl_Employees] ");
                query.Append("WHERE ");
                query.Append("[EmployeesID] = @id ");
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
                            throw new ArgumentNullException("dbCommand" + " The db Delete command for entity [tbl_Employees] can't be null. ");

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
                            throw new Exception("Deleting Error for entity [tbl_Employees] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes:EmployeeRepository::DeleteByID:Error occured.", ex);
            }
        }

        public IList<Employees> GetAll()
        {
            _rowsAffected = 0;
            _errorCode = 0;

            IList<Employees> employees = new List<Employees>();

            try
            {
                var query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("[EmployeesID], [EmployeesCompanyID],[EmployeesEmail],[EmployessPassword],[EmployeesPhoneNumber],[EmployeesAddress] ");
                query.Append("FROM [dbo].[tbl_Employees] ");
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
                            throw new ArgumentNullException("dbCommand" + " The db GetAll command for entity [tbl_Employees] can't be null. ");

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
                                    var entity = new Employees();
                                    entity.EmployeesId = reader.GetInt32(0);
                                    entity.EmployeesCompanyId = reader.GetInt32(1);
                                    entity.EmployeesEmail = reader.GetString(2);
                                    entity.EmployessPassword = reader.GetString(3);
                                    entity.EmployeesPhoneNumber = reader.GetString(4);
                                    entity.EmployeesAddress = reader.GetString(5);
                                    employees.Add(entity);
                                }
                            }
                        }

                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Getting All Error for entity [tbl_Employees] reported the Database ErrorCode: " + _errorCode);
                    }
                }
                return employees;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes:EmployeeRepository:GetAll::Error occured.", ex);
            }
        }

        public Employees GetByID(int id)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            Employees employees = null;

            try
            {
                var query = new StringBuilder();
                query.Append("SELECT ");
                query.Append("[EmployeesCompanyID],[EmployeesEmail],[EmployessPassword],[EmployeesPhoneNumber],[EmployeesAddress] ");
                query.Append("FROM [dbo].[tbl_Employees] ");
                query.Append("WHERE ");
                query.Append("[EmployeesID] = @id ");
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
                            throw new ArgumentNullException("dbCommand" + " The db GetByID command for entity [tbl_Employees] can't be null. ");

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
                                    var entity = new Employees();
                                    entity.EmployeesCompanyId = reader.GetInt32(0);
                                    entity.EmployeesEmail = reader.GetString(1);
                                    entity.EmployessPassword = reader.GetString(2);
                                    entity.EmployeesPhoneNumber = reader.GetString(3);
                                    entity.EmployeesAddress = reader.GetString(4);
                                    employees = entity;
                                    break;
                                }
                            }
                        }

                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("GetByID Error for entity [tbl_Employees] reported the Database ErrorCode: " + _errorCode);
                    }
                }

                return employees;
            }
            catch (Exception ex)
            {
                LogHelper.Log(LogTarget.File, ExceptionHelper.ExceptionToString(ex), true);
                LogHelper.Log(LogTarget.Database, ExceptionHelper.ExceptionToString(ex), true);
                throw new Exception("CarRental.DataAccess.Concretes:EmployeeRepository:GetByID::Error occured.", ex);
            }
        }

        public bool Insert(Employees entity)
        {
            _rowsAffected = 0;
            _errorCode = 0;
            try
            {
                var query = new StringBuilder();
                query.Append("INSERT [dbo].[tbl_Employees] ");
                query.Append("([EmployeesCompanyID], [EmployeesEmail],[EmployessPassword],[EmployeesPhoneNumber],[EmployeesAddress]) ");
                query.Append("VALUES ");
                query.Append("( @EmployeesCompanyID, @EmployeesEmail, @EmployessPassword, @EmployeesPhoneNumber, @EmployeesAddress ) ");
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
                            throw new ArgumentNullException("dbCommand" + " The db Insert command for entity [tbl_Employees] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Params
                        DBHelper.AddParameter(dbCommand, "@EmployeesCompanyID", CsType.String, ParameterDirection.Input, entity.EmployeesCompanyId);
                        DBHelper.AddParameter(dbCommand, "@EmployeesEmail", CsType.String, ParameterDirection.Input, entity.EmployeesEmail);
                        DBHelper.AddParameter(dbCommand, "@EmployessPassword", CsType.String, ParameterDirection.Input, entity.EmployessPassword);
                        DBHelper.AddParameter(dbCommand, "@EmployeesPhoneNumber", CsType.String, ParameterDirection.Input, entity.EmployeesPhoneNumber);
                        DBHelper.AddParameter(dbCommand, "@EmployeesAddress", CsType.String, ParameterDirection.Input, entity.EmployeesAddress);

                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Inserting Error for entity [tbl_Employees] reported the Database ErrorCode: " + _errorCode);
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

        public bool Update(Employees entity)
        {
            _rowsAffected = 0;
            _errorCode = 0;

            try
            {
                var query = new StringBuilder();
                query.Append("UPDATE [dbo].[tbl_Companies] ");
                query.Append("(SET [EmployeesCompanyID] = @EmployeesCompanyID,[EmployeesEmail] = @EmployeesEmail ,[EmployessPassword] = @EmployessPassword," +
                    "[EmployeesPhoneNumber] = @EmployeesPhoneNumber," +
                    "[EmployeesAddress] = @EmployeesAddress) ");
                query.Append("WHERE ");
                query.Append("[EmployeesID] = @EmployeesID ");
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
                            throw new ArgumentNullException("dbCommand" + " The db Update command for entity [tbl_Emplyees] can't be null. ");

                        dbCommand.Connection = dbConnection;
                        dbCommand.CommandText = commandText;

                        //Input Params
                        DBHelper.AddParameter(dbCommand, "@EmployeesID", CsType.Int, ParameterDirection.Input, entity.EmployeesId);
                        DBHelper.AddParameter(dbCommand, "@EmployeesCompanyID", CsType.String, ParameterDirection.Input, entity.EmployeesCompanyId);
                        DBHelper.AddParameter(dbCommand, "@EmployeesEmail", CsType.String, ParameterDirection.Input, entity.EmployeesEmail);
                        DBHelper.AddParameter(dbCommand, "@EmployessPassword", CsType.String, ParameterDirection.Input, entity.EmployessPassword);
                        DBHelper.AddParameter(dbCommand, "@EmployeesPhoneNumber", CsType.String, ParameterDirection.Input, entity.EmployeesPhoneNumber);
                        DBHelper.AddParameter(dbCommand, "@EmployeesAddress", CsType.String, ParameterDirection.Input, entity.EmployeesAddress);

                        //Output Params
                        DBHelper.AddParameter(dbCommand, "@intErrorCode", CsType.Int, ParameterDirection.Output, null);

                        if (dbConnection.State != ConnectionState.Open)
                            dbConnection.Open();

                        _rowsAffected = dbCommand.ExecuteNonQuery();
                        _errorCode = int.Parse(dbCommand.Parameters["@intErrorCode"].Value.ToString());

                        if (_errorCode != 0)
                            throw new Exception("Updating Error for entity [tbl_Employees] reported the Database ErrorCode: " + _errorCode);
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
