using CafeApp_api.DTO;
using Microsoft.Data.SqlClient;

namespace CafeApp_api.Service
{
	public class CafeService : ICafeService
	{
		private readonly IConfiguration _configuration;
        public CafeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<Users> GetAllUsers()
		{
			try
			{
				string connection = _configuration.GetConnectionString("myconn");
				var openCon = new SqlConnection(connection);
				
					var cmd = new SqlCommand()
					{
						CommandText = "select * from users",
						CommandType = System.Data.CommandType.Text,
						Connection = openCon
					};
				openCon.Open();
				var wantedRow = cmd.ExecuteReader();
				openCon.Close();
				return new();
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}
