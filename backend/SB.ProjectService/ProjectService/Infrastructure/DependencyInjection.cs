using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using ProjectService.Application.Dependencies.Services;
using ProjectService.Infrastructure.Integrations;
using ProjectService.Infrastructure.Persistence;
using System.Text;

namespace ProjectService.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
            => services
                .AddPersistence(configuration)
                .AddAuthIntegration(configuration)
                .AddJwt(configuration);

        private IServiceCollection AddAuthIntegration(IConfiguration configuration)
        {
            services.Configure<AuthServiceSettings>(configuration.GetSection("Auth"));
            services.AddHttpClient<IEmployeeAccountSyncService, AuthEmployeeAccountSyncService>((sp, client) =>
            {
                var settings = sp.GetRequiredService<IOptions<AuthServiceSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl.TrimEnd('/') + "/");
            });
            return services;
        }

        private IServiceCollection AddJwt(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()!;

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Secret))
                    };
                });

            services.AddAuthorization();
            return services;
        }
    }
}
