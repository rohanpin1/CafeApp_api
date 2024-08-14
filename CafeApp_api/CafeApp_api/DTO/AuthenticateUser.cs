namespace CafeApp_api.DTO
{
    public class AuthenticateUser
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool Is2FAEnabled { get; set; }
    }
}
