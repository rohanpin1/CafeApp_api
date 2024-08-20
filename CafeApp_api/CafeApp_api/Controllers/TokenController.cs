using CafeApp_api.DTO;
using CafeApp_api.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CafeApp_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet("GetToken")]
        public async Task<string> GetToken(AuthenticateUser user)
        {
            return await _tokenService.GetToken(user);
        }
    }
}
