namespace TravelTrek.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string toEmail, string fullName, Guid userId, string token);
        Task SendPasswordResetEmailAsync(string toEmail, string fullName, string resetToken);
    }
}
