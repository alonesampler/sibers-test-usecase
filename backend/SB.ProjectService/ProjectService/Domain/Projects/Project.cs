using FluentResults;
using ProjectService.Domain.Employees;

namespace ProjectService.Domain.Projects;

public sealed class Project(Guid id) : Entity<Guid>(id)
{
    public string Name { get; private set; }
    public string CustomerCompanyName { get; private set; }
    public string ExecutorCompanyName { get; private set; }
    public DateOnly StartDate { get; private set; }
    public DateOnly EndDate { get; private set; }
    public int Priority { get; private set; }

    public Guid ManagerId { get; private set; }
    public Employee Manager { get; private set; } = null!;

    private readonly List<Employee> _employees = [];
    public IReadOnlyCollection<Employee> Employees => _employees;

    public static Result<Project> Create(
    string name,
    string customerCompanyName,
    string executorCompanyName,
    DateOnly startDate,
    DateOnly endDate,
    int priority,
    Employee manager,
    IEnumerable<Employee> employees)
    {
        if (startDate >= endDate)
            return Result.Fail(ProjectError.InvalidDates);

        if (priority < 1 ||  priority > 10)
            return Result.Fail(ProjectError.InvalidPriority);

        var project = new Project(Guid.CreateVersion7())
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

        project._employees.AddRange(employees);
        return project;
    }

    public Result Update(
        string name,
        string customerCompanyName,
        string executorCompanyName,
        DateOnly startDate,
        DateOnly endDate,
        int priority,
        Employee manager,
        IEnumerable<Employee> employees)
    {
        if (startDate >= endDate)
            return Result.Fail(ProjectError.InvalidDates);

        if (priority < 1 || priority > 10)
            return Result.Fail(ProjectError.InvalidPriority);

        Name = name;
        CustomerCompanyName = customerCompanyName;
        ExecutorCompanyName = executorCompanyName;
        StartDate = startDate;
        EndDate = endDate;
        Priority = priority;
        ManagerId = manager.Id;
        Manager = manager;

        _employees.Clear();
        _employees.AddRange(employees);
        return Result.Ok();
    }
}