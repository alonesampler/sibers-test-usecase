using FluentResults;
using ProjectService.Domain.Employees;

namespace ProjectService.Domain.Projects;

public sealed class Project(Guid id) : Entity<Guid>(id)
{
    private readonly List<Employee> _employees = new();

    public string Name { get; private set; }
    public string CustomerCompanyName { get; private set; }
    public string ExecutorCompanyName { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public int Priority { get; private set; }

    public Guid ManagerId { get; private set; }
    public Employee Manager { get; private set; } = null!;

    public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

    public static Result<Project> Create(
        string name,
        string customerCompanyName,
        string executorCompanyName,
        DateOnly startDate,
        DateOnly endDate,
        int priority,
        Employee manager)
    {
        if (startDate >= endDate)
            return Result.Fail(ProjectError.InvalidDates);

        return new Project(Guid.CreateVersion7())
        {
            Name = name,
            CustomerCompanyName = customerCompanyName,
            ExecutorCompanyName = executorCompanyName,
            StartDate = startDate,
            EndDate = endDate,
            Priority = priority,
            ManagerId = manager.Id,
            Manager = manager
        };
    }

    public void SetEmployees(IEnumerable<Employee> employees)
    {
        _employees.Clear();
        foreach (var employee in employees)
            _employees.Add(employee);
    }

    public Result Update(
        string name,
        string customerCompanyName,
        string executorCompanyName,
        DateOnly startDate,
        DateOnly endDate,
        int priority)
    {
        if (startDate >= endDate)
            return Result.Fail(ProjectError.InvalidDates);

        Name = name;
        CustomerCompanyName = customerCompanyName;
        ExecutorCompanyName = executorCompanyName;
        StartDate = startDate;
        EndDate = endDate;
        Priority = priority;
        return Result.Ok();
    }
}