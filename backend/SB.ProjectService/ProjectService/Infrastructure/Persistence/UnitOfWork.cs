using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Domain.Documents.Repositories;
using ProjectService.Domain.Employees.Repositories;
using ProjectService.Domain.Projects.Repositories;

namespace ProjectService.Infrastructure.Persistence;

public class UnitOfWork(
    DatabaseContext DbContext,
    IEmployeeRepository employeeRepository,
    IProjectRepository projectRepository,
    IDocumentRepository documentRepository,
    IProjectTaskRepository projectTaskRepository ) : IUnitOfWork
{
    public IEmployeeRepository EmployeeRepository => employeeRepository;
    public IProjectRepository ProjectRepository => projectRepository;
    public IDocumentRepository DocumentRepository => documentRepository;
    public IProjectTaskRepository ProjectTaskRepository => projectTaskRepository;

    public Task SaveAsync(CancellationToken ct = default)
        => DbContext.SaveChangesAsync(ct);
}