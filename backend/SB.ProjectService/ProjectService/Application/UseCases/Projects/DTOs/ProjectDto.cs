using ProjectService.Application.UseCases.Employees.DTOs;

namespace ProjectService.Application.UseCases.Projects.DTOs;

public record ProjectDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string CustomerCompanyName { get; init; }
    public string ExecutorCompanyName { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public int Priority { get; init; }
    public EmployeeDto Manager { get; init; }
    public EmployeeDto[] Employees { get; init; }
}
