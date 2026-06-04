using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connectionString));
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
                .AddScoped<LoginUseCase>()
                .AddScoped<GetUserAccountsUseCase>()
                .AddScoped<SyncEmployeeEmailUseCase>();
    }
}
