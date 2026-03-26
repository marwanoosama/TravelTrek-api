using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Application.DTOs.Auth
{
    public class RevokeTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
