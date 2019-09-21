using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.WebApi.Library.Internal.DataAccess
{
    internal class SqlDataAccess
    {
        public string GetConnectionString(string name) 
            => ConfigurationManager.ConnectionStrings[name].ConnectionString;

        public List<TResult> LoadData<TResult, TParameters>(string storedProcedureName, TParameters parameters, string connectionStringName)
        {
            var cnnString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(cnnString))
            {
                List<TResult> rows = connection.Query<TResult>(storedProcedureName, parameters,
                    commandType: CommandType.StoredProcedure).ToList();

                return rows;
            }
        }

        public void SaveData<TParameters>(string storedProcedureName, TParameters parameters, string connectionStringName)
        {
            var cnnString = GetConnectionString(connectionStringName);

            using (IDbConnection connection = new SqlConnection(cnnString))
            {
                connection.Execute(storedProcedureName, parameters,
                    commandType: CommandType.StoredProcedure);
            }
        }
    }
}
