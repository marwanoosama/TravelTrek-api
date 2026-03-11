using System.ComponentModel.DataAnnotations;

namespace TravelTrek.Application.DTOs.Auth
{
    public class SignupWithGoogleRequest
    {
        [Required]
        public string IdToken { get; set; } = string.Empty;
    }
}
