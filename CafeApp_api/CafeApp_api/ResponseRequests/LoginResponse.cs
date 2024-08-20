namespace CafeApp_api.ResponseRequests
{
    public class LoginResponse
    {
        public bool Is2FAEnabled { get; set; }
        public string? Token { get; set; }
        public int FlagCode { get; set; }
    }
}
