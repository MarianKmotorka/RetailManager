using RM.WebApi.Library.Internal.DataAccess;
using RM.WebApi.Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RM.WebApi.Library.DataAccess
{
    public class UserData
    {
        public UserModel GetUserById(string id)
        {
            SqlDataAccess sql = new SqlDataAccess();

            var parameters = new { @Id = id };
            var output = sql.LoadData<UserModel, dynamic>("dbo.spUserLookup", parameters, "RM.Database").Single();
            return output;
        }
    }
}
