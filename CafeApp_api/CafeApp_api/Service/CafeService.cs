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
        public async Task<List<Users>> GetAllUsers()
		{
			try
			{
				var users = new List<Users>();
				string connection = _configuration.GetConnectionString("myconn");
				var openCon = new SqlConnection(connection);
				
					var cmd = new SqlCommand()
					{
						CommandText = "select * from users",
						CommandType = System.Data.CommandType.Text,
						Connection = openCon
					};
				openCon.Open();
				var row = cmd.ExecuteReader();

				while (row.Read())
				{
					Users user = new Users();
					user.Name = (string)row["name"];
					user.Phone = (string)row["phone"];
					user.Email = (string)row["email"];
					user.City = (string)row["city"];
					users.Add(user);
				}
				
				openCon.Close();
				return users;
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}