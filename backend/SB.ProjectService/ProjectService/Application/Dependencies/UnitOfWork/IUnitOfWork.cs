using ProjectService.Domain.Documents.Repositories;
using ProjectService.Domain.Employees.Repositories;
using ProjectService.Domain.Projects.Repositories;

namespace ProjectService.Application.Dependencies.UnitOfWork;

public interface IUnitOfWork
{
    IEmployeeRepository EmployeeRepository { get; }
    IProjectRepository ProjectRepository { get; }
    IDocumentRepository DocumentRepository { get; }
    IProjectTaskRepository ProjectTaskRepository { get; }

    Task SaveAsync(CancellationToken ct = default);
}
