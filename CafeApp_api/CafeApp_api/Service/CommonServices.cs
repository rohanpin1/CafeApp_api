using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CafeApp_api.Service
{
    public static class CommonServices
    {
        public static SqlCommand RunCommand(string query,SqlConnection conn)
        {
            var cmd = new SqlCommand()
            {
                CommandText = query,
                CommandType = System.Data.CommandType.Text,
                Connection = conn
            };
            return cmd;
        }
    }
}
