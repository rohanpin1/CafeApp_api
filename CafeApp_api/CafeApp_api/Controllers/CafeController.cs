using CafeApp_api.DTO;
using CafeApp_api.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CafeApp_api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CafeController : ControllerBase
	{
        private readonly ICafeService _cafeService;
        public CafeController(ICafeService cafeService)
        {
              _cafeService = cafeService; 
        }

        [HttpGet("GetUsers")]
        public async Task<List<Users>> GetUsersAsync()
        {
            var users = await _cafeService.GetAllUsers();
            return users;
        }

        [HttpDelete("DeleteUser")]
        public void DeleteUser(int id)
        {
            _cafeService.DeleteRow(id);
        }

        [HttpGet("GetUser")]
		public async Task<Users> GetUser(int id)
        {
            return  await _cafeService.GetUser(id);
        }

        [HttpPost("UpdateUser")]
		public void UpdateUser([FromBody] Users user)
        {
            _cafeService.UpdateUser(user);
        }

        [HttpGet("GetUserByPhoneNumber")]
        public async Task<Users> GetUserByPhoneNumber(string phoneNumber)
        {
            return await _cafeService.GetUserByPhoneNumber(phoneNumber);
        }

        [HttpPost("RegisterLoginUser")]
        public async Task<bool> RegisterLoginUser([FromBody]AuthenticateUser user)
        {
            return await _cafeService.RegisterLoginUser(user);
        }
    }
}
