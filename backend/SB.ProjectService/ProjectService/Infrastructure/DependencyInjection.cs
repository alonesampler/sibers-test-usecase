using ProjectService.Infrastructure.Persistence;

namespace ProjectService.Infrastructure;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
            => services.AddPersistence(configuration);
    }
}
