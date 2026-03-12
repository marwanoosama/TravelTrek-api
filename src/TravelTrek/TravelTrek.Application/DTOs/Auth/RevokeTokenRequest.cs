namespace TravelTrek.Application.DTOs.Auth
{
    public class RevokeTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
