using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TravelTrek.Application.Interfaces;
using TravelTrek.Domain.Entities;
using TravelTrek.Domain.Interfaces;
using TravelTrek.Infrastructure.Auth;
using TravelTrek.Infrastructure.Data;
using TravelTrek.Infrastructure.Data.Configurations;
using TravelTrek.Infrastructure.Repositories;
using TravelTrek.Infrastructure.Repositories.User;
using TravelTrek.Infrastructure.Services;
using TravelTrek.Infrastructure.Services.OpenTrip;
using TravelTrek.Infrastructure.Services.Weather;

namespace TravelTrek.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = true;

                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
                options.Lockout.AllowedForNewUsers = true;
            })
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var secret = configuration["JwtSettings:Secret"]!;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.Configure<GoogleSettings>(configuration.GetSection("Google"));
            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            // FluentEmail configuration
            var emailSettings = configuration.GetSection("EmailSettings").Get<EmailSettings>()
                ?? throw new InvalidOperationException("EmailSettings configuration is missing.");

            services
                .AddFluentEmail(emailSettings.SenderEmail, emailSettings.SenderName)
                .AddSmtpSender(new SmtpClient(emailSettings.SmtpServer)
                {
                    Port = emailSettings.SmtpPort,
                    Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password),
                    EnableSsl = emailSettings.EnableSsl
                });

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IEmailService, EmailService>();

            // OpenTripMap API (typed HttpClient registered in Program.cs)
            services.Configure<OpenTripMapApiOptions>(configuration.GetSection("OpenTripMapAPI"));

            // OpenWeather API
            services.AddOptionsWithValidateOnStart<OpenWeatherApiOptions>()
                .Bind(configuration.GetSection(OpenWeatherApiOptions.SectionName))
                .ValidateDataAnnotations();

            services.AddHttpClient<IOpenWeatherService, OpenWeatherService>(client =>
            {
                client.BaseAddress = new Uri(configuration[$"{OpenWeatherApiOptions.SectionName}:BaseUrl"]!);
                client.Timeout = TimeSpan.FromSeconds(15);
            })
            .AddStandardResilienceHandler();

            return services;
        }
    }
}