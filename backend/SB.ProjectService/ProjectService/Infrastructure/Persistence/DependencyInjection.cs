using ProjectService.Application.Dependencies.EmployeesServices;
using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Application.UseCases.Employees.Getting;
using ProjectService.Application.UseCases.Employees.Writing;
using ProjectService.Domain.Employees.Repositories;
using ProjectService.Infrastructure.Persistence.Employees;

namespace ProjectService.Infrastructure.Persistence;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddPersistence(IConfiguration configuration)
            => services.AddEfCore(configuration).AddRepositories().AddServices();

        private IServiceCollection AddEfCore(IConfiguration configuration)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            return services.AddNpgsql<DatabaseContext>(
                configuration.GetConnectionString("DefaultConnection"),
                _ => { });
        }

        private IServiceCollection AddServices()
            => services
                .AddScoped<IGettingEmployeesService, GettingEmployeesService>()
                .AddScoped<CreateEmployeeUseCase>();

        private IServiceCollection AddRepositories()
            => services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IEmployeeRepository, EmployeeRepository>();
    }
}
