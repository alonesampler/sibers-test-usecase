using FluentResults;

namespace ProjectService.Domain.Employees;

public sealed class Employee(Guid id) : Entity<Guid>(id)
{
    public FullName FullName { get;  init; }

    public string Email { get; init; }

    public bool IsManager { get; init; }

    public static Result<Employee> Create(FullName fullName, string email)
    {
        var employee = new Employee(Guid.CreateVersion7())
        {
            FullName = fullName,
            Email = email,
            IsManager = false
        };

        return employee;
    }

    public Result<Employee> Update(FullName fullName, string email)
    {
        var updated = new Employee(Id)
        {
            FullName = fullName,
            Email = email,
            IsManager = IsManager
        };
        return updated;
    }
}
