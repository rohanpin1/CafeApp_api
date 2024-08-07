using CafeApp_api.DTO;

namespace CafeApp_api.Service
{
	public interface ICafeService
	{
		Task<Users> GetAllUsers();
	}
}
