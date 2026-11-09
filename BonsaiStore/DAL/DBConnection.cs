
using System.Configuration;
using System.Data.SqlClient;

namespace BonsaiStore.DAL
{
    public class DBConnection
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["BonsaiConnection"].ConnectionString;

            return new SqlConnection(connectionString);
        }
    }
}
