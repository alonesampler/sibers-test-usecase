using ProjectService.Domain.Employees.Repositories;

namespace ProjectService.Application.Dependencies.UnitOfWork;

public interface IUnitOfWork
{
    IEmployeeRepository EmployeeRepository { get; }

    Task SaveAsync(CancellationToken ct = default);
}
