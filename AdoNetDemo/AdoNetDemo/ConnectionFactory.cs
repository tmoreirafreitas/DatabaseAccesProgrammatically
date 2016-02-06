using System;
using System.Data.SqlClient;
using System.Data;
using System.Text;

namespace AdoNetDemo
{
    /// <summary>
    /// 
    /// </summary>
    public class ConnectionFactory : IDisposable
    {
        private static SqlConnection _connection;

        /// <summary>
        /// 
        /// </summary>
        /// <returns>SqlConnection</returns>
        public static SqlConnection CreateConnection()
        {
            const string server = @"Data Source=DESKTOP-49LVODE;";
            const string database = @"Initial Catalog=SVDB;";
            const string user = @"User Id=SVAdminUser;";
            const string password = @"Password=root@*741258";

            var sb = new StringBuilder();
            sb.Append(server);
            sb.Append(database);
            sb.Append(user);
            sb.Append(password);
            sb.Append(@";MultipleActiveResultSets=true;");

            _connection = new SqlConnection(sb.ToString());

            if (_connection.State == ConnectionState.Closed)
                _connection.Open();

            return _connection;
        }

        public static void Fechar()
        {
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
        }

        public void Dispose()
        {
            Fechar();
            GC.SuppressFinalize(this);
        }
    }
}
