namespace ProjectService.Domain.Employees;

public sealed class Employee(Guid id) : Entity<Guid>(id)
{
    public FullName FullName { get;  init; }
    public string Email { get; init; }
}
