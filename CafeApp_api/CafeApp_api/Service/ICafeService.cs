using CafeApp_api.DTO;

namespace CafeApp_api.Service
{
	public interface ICafeService
	{
		Task<List<Users>> GetAllUsers();
		void DeleteRow(int id);
		Task<Users> GetUser(int id);
		void UpdateUser(Users user);	

		Task<Users> GetUserByPhoneNumber(string phoneNumber);
		
	}
}
