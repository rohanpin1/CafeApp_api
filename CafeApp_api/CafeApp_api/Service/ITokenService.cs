using CafeApp_api.DTO;

namespace CafeApp_api.Service
{
    public interface ITokenService
    {
        Task<string> GetToken(AuthenticateUser user);
    }
}
