using CarRental.Commons.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;
using CarRental.Commons.Concretes.Helper;
using System.Data;
using CarRental.Commons.Concretes.Data;
using System.Data.SqlClient;

namespace CarRental.Commons.Concretes.Logger
{
    internal class DBLogger : LogBase
    {
        string connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
        
        public override void Log(string message, bool isError)
        {
            lock (lockObj)
            {
                if (isError)
                {
                    SqlConnection con = new SqlConnection(connectionString);
                    SqlCommand command = new SqlCommand("spInsertLog", con);
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter param = new SqlParameter("@ExceptionMessage", message);
                    command.Parameters.Add(param);
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
}
