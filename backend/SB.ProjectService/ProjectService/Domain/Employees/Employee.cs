using FluentResults;

namespace ProjectService.Domain.Employees;

public sealed class Employee(Guid id) : Entity<Guid>(id)
{
    public FullName FullName { get;  private set; }

    public string Email { get; private set; }

    public static Result<Employee> Create(FullName fullName, string email)
        => CreateWithId(Guid.CreateVersion7(), fullName, email);

    public static Result<Employee> CreateWithId(Guid id, FullName fullName, string email)
    {
        var employee = new Employee(id)
        {
            FullName = fullName,
            Email = email
        };

        return employee;
    }

    public Result<Employee> Update(FullName fullName, string email)
    {
        FullName = fullName;
        Email = email;
        return this;
    }
}
