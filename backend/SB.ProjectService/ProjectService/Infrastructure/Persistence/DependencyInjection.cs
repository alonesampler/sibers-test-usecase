using ProjectService.Application.Dependencies.Services;
using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Application.UseCases.Document;
using ProjectService.Application.UseCases.Employees.Getting;
using ProjectService.Application.UseCases.Employees.Writing;
using ProjectService.Application.UseCases.Projects.Getting;
using ProjectService.Application.UseCases.Projects.Writing;
using ProjectService.Application.UseCases.Tasks.Getting;
using ProjectService.Application.UseCases.Tasks.Writing;
using ProjectService.Domain.Documents.Repositories;
using ProjectService.Domain.Employees.Repositories;
using ProjectService.Domain.Projects.Repositories;
using ProjectService.Infrastructure.Persistence.Documents;
using ProjectService.Infrastructure.Persistence.Employees;
using ProjectService.Infrastructure.Persistence.Projects;

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
                .AddScoped<IDocumentsService, DocumentsService>()
                .AddScoped<IGettingProjectTasksService, GettingProjectTasksService>()
                .AddScoped<IGettingProjectsService, GettingProjectsService>()
                .AddScoped<IGettingEmployeesService, GettingEmployeesService>()
                .AddScoped<CreateProjectTaskUseCase>()
                .AddScoped<UpdateProjectTaskUseCase>()
                .AddScoped<DeleteProjectTaskUseCase>()
                .AddScoped<CreateProjectUseCase>()
                .AddScoped<UpdateProjectUseCase>()
                .AddScoped<DeleteProjectUseCase>()
                .AddScoped<CreateEmployeeUseCase>()
                .AddScoped<UpdateEmployeeUseCase>()
                .AddScoped<DeleteEmployeeUseCase>();


        private IServiceCollection AddRepositories()
            => services
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IProjectTaskRepository, ProjectTaskRepository>()
                .AddScoped<IDocumentRepository, DocumentRepository>()
                .AddScoped<IProjectRepository, ProjectRepository>()
                .AddScoped<IEmployeeRepository, EmployeeRepository>();
    }
}
