using Microsoft.AspNetCore.Identity;
using SB.Auth.Application.UseCases;
using SB.Auth.Domain.User;

namespace SB.Auth.Infrastructire.Persistence;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence(IConfiguration configuration)
            => services.AddEfCore(configuration).AddIdentityStore().AddUseCases();

        private IServiceCollection AddEfCore(IConfiguration configuration)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddNpgsql<DatabaseContext>(
                configuration.GetConnectionString("DefaultConnection"),
                _ => { });
            return services;
        }

        private IServiceCollection AddIdentityStore()
        {
            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();
            return services;
        }

        private IServiceCollection AddUseCases()
            => services
                .AddScoped<RegisterUseCase>()
                .AddScoped<LoginUseCase>();
    }
}
