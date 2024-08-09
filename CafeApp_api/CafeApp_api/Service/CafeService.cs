using CafeApp_api.DTO;
using Microsoft.Data.SqlClient;


namespace CafeApp_api.Service
{
	public class CafeService : ICafeService
	{
		private readonly IConfiguration _configuration;
		private readonly string _connectionString;
        public CafeService(IConfiguration configuration)
        {
            _configuration = configuration;
			_connectionString = _configuration.GetConnectionString("myconn");

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
					user.Id = (int)row["id"];
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

		public void DeleteRow(int id)
		{
			try
			{
				var openCon = new SqlConnection(_connectionString);
				var cmd = new SqlCommand()
				{
					CommandText = $"delete from users where id = {id}",
					CommandType = System.Data.CommandType.Text,
					Connection = openCon
				};
				openCon.Open();
				var row = cmd.ExecuteNonQuery();
				openCon.Close();
			}
			catch (Exception)
			{

				throw;
			}
		}
	}
}