using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetDemo
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns>SqlConnection</returns>
        public static SqlConnection CreateConnection()
        {
            string server = @"Data Source=DESKTOP-49LVODE;";
            string database = @"Initial Catalog=SVDB;";
            string user = @"User ID=SVAdminUser;";
            string password = @"Password=root@*741258";

            StringBuilder sb = new StringBuilder();
            sb.Append(server);
            sb.Append(database);
            sb.Append(user);
            sb.Append(password);
            sb.Append(@";MultipleActiveResultSets=true;");

            return new SqlConnection(sb.ToString());
        }
    }
}
