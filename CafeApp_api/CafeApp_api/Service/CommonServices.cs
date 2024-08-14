using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CafeApp_api.Service
{
    public class CommonServices : ICommonServices
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public CommonServices(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("myconn");
        }
        public SqlCommand RunCommand(string query)
        {
            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand()
            {
                CommandText = query,
                CommandType = System.Data.CommandType.Text,
                Connection = conn
            };
            return cmd;
        }

        public void OpenConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
        }

        public void CloseConnection()
        {
            new SqlConnection(_connectionString).Close();
        }
    }
}
