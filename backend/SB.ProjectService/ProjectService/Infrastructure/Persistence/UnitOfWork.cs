using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Domain.Employees.Repositories;

namespace ProjectService.Infrastructure.Persistence;

public class UnitOfWork(
    DatabaseContext DbContext,
    IEmployeeRepository employeeRepository) : IUnitOfWork
{
    public IEmployeeRepository EmployeeRepository => employeeRepository;

    public Task SaveAsync(CancellationToken ct = default)
        => DbContext.SaveChangesAsync(ct);
}