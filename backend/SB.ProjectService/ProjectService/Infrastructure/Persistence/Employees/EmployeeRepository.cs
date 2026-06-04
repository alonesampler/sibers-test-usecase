using Microsoft.EntityFrameworkCore;
using ProjectService.Domain.Employees;
using ProjectService.Domain.Employees.Repositories;

namespace ProjectService.Infrastructure.Persistence.Employees;

internal sealed class EmployeeRepository(DatabaseContext db) : IEmployeeRepository
{
    public Task AddAsync(Employee employee)
        => db.Employees.AddAsync(employee).AsTask();
    public void Delete(Employee employee)
        => db.Employees.Remove(employee);

    public Task<Employee[]> GetByIdsAsync(Guid[] ids)
        => db.Employees
            .Where(e => ids.Contains(e.Id))
            .ToArrayAsync();

    public Task<Employee[]> GetAllAsync() =>
        db.Employees
            .AsNoTracking()
            .ToArrayAsync();

    public Task<Employee?> GetByEmailAsync(string email)
        => db.Employees.FirstOrDefaultAsync(e => e.Email == email);

    public Task<Employee?> GetByIdAsync(Guid id)
        => db.Employees.FirstOrDefaultAsync(e => e.Id == id);

    public Task<Employee[]> SearchAsync(string? query = null)
    {
        if (string.IsNullOrWhiteSpace(query))
            return GetAllAsync();

        var q = query.Trim();

        return db.Employees
            .Where(e =>
                e.FullName.Name.Contains(q) ||
                e.FullName.Surname.Contains(q) ||
                e.FullName.Patronymic!.Contains(q) ||
                e.Email.Contains(q)
            )
            .ToArrayAsync();
    }

    public void Update(Employee employee)
    {
        db.Employees.Update(employee);
    }
}