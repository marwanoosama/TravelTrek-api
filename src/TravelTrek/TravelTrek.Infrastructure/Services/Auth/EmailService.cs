using FluentEmail.Core;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TravelTrek.Application.Interfaces;
using TravelTrek.Infrastructure.Auth;

namespace TravelTrek.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IFluentEmail _fluentEmail;
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            IFluentEmail fluentEmail,
            IOptions<EmailSettings> emailSettings,
            ILogger<EmailService> logger)
        {
            _fluentEmail = fluentEmail;
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendConfirmationEmailAsync(string toEmail, string fullName, Guid userId, string token)
        {
            var encodedToken = Uri.EscapeDataString(token);
            var confirmationLink = $"{_emailSettings.BaseUrl}/api/auth/confirm-email?userId={userId}&token={encodedToken}";

            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                    <div style='max-width: 600px; margin: 0 auto; background: #ffffff; border-radius: 8px; padding: 40px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
                        <h1 style='color: #2c3e50; margin-bottom: 10px;'>Welcome to TravelTrek! ✈️</h1>
                        <p style='font-size: 16px; color: #555;'>Hi <strong>{fullName}</strong>,</p>
                        <p style='font-size: 16px; color: #555;'>Thank you for signing up. Please confirm your email address by clicking the button below:</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{confirmationLink}' 
                               style='background-color: #3498db; color: #ffffff; padding: 14px 32px; text-decoration: none; border-radius: 6px; font-size: 16px; font-weight: bold;'>
                                Confirm My Email
                            </a>
                        </div>
                        <p style='font-size: 14px; color: #888;'>If the button doesn't work, copy and paste this link into your browser:</p>
                        <p style='font-size: 13px; color: #3498db; word-break: break-all;'>{confirmationLink}</p>
                        <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;' />
                        <p style='font-size: 12px; color: #aaa;'>If you didn't create an account, you can safely ignore this email.</p>
                    </div>
                </body>
                </html>";

            try
            {
                var response = await _fluentEmail
                    .To(toEmail)
                    .Subject("Confirm your TravelTrek email")
                    .Body(body, isHtml: true)
                    .SendAsync();

                if (response.Successful)
                {
                    _logger.LogInformation("Confirmation email sent successfully. Email: {Email}, UserId: {UserId}", toEmail, userId);
                }
                else
                {
                    _logger.LogWarning("Failed to send confirmation email. Email: {Email}, Errors: {Errors}",
                        toEmail, string.Join(", ", response.ErrorMessages));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while sending confirmation email. Email: {Email}, UserId: {UserId}", toEmail, userId);
                throw;
            }
        }

        public async Task SendPasswordResetEmailAsync(string toEmail, string fullName, string resetToken)
        {
            var encodedToken = Uri.EscapeDataString(resetToken);
            var encodedEmail = Uri.EscapeDataString(toEmail);
            var resetLink = $"{_emailSettings.BaseUrl}/reset-password?email={encodedEmail}&token={encodedToken}";

            var body = $@"
                <html>
                <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 20px;'>
                    <div style='max-width: 600px; margin: 0 auto; background: #ffffff; border-radius: 8px; padding: 40px; box-shadow: 0 2px 8px rgba(0,0,0,0.1);'>
                        <h1 style='color: #2c3e50; margin-bottom: 10px;'>Reset Your Password 🔒</h1>
                        <p style='font-size: 16px; color: #555;'>Hi <strong>{fullName}</strong>,</p>
                        <p style='font-size: 16px; color: #555;'>We received a request to reset your password. Click the button below to set a new password:</p>
                        <div style='text-align: center; margin: 30px 0;'>
                            <a href='{resetLink}' 
                               style='background-color: #e74c3c; color: #ffffff; padding: 14px 32px; text-decoration: none; border-radius: 6px; font-size: 16px; font-weight: bold;'>
                                Reset My Password
                            </a>
                        </div>
                        <p style='font-size: 14px; color: #888;'>If the button doesn't work, copy and paste this link into your browser:</p>
                        <p style='font-size: 13px; color: #e74c3c; word-break: break-all;'>{resetLink}</p>
                        <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;' />
                        <p style='font-size: 12px; color: #aaa;'>If you didn't request a password reset, you can safely ignore this email. Your password will remain unchanged.</p>
                    </div>
                </body>
                </html>";

            try
            {
                var response = await _fluentEmail
                    .To(toEmail)
                    .Subject("Reset your TravelTrek password")
                    .Body(body, isHtml: true)
                    .SendAsync();

                if (response.Successful)
                {
                    _logger.LogInformation("Password reset email sent successfully. Email: {Email}", toEmail);
                }
                else
                {
                    _logger.LogWarning("Failed to send password reset email. Email: {Email}, Errors: {Errors}",
                        toEmail, string.Join(", ", response.ErrorMessages));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while sending password reset email. Email: {Email}", toEmail);
                throw;
            }
        }
    }
}
