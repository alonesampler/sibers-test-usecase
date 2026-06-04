using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SB.Auth.Api.Filters;
using SB.Auth.Domain;
using SB.Auth.Infrastructire.Persistence;
using System.Text;

namespace SB.Auth.Infrastructire;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
            => services
                .AddPersistence(configuration)
                .AddJwt(configuration)
                .AddInternalApi(configuration);

        private IServiceCollection AddJwt(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>()!;
            services.AddSingleton(jwtSettings);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
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

        private IServiceCollection AddInternalApi(IConfiguration configuration)
        {
            services.Configure<InternalApiSettings>(configuration.GetSection("InternalApi"));
            services.AddScoped<InternalApiKeyFilter>();
            return services;
        }
    }
}
