using ProjectService.Application.UseCases.Employees.DTOs;
using ProjectService.Domain.Projects;

namespace ProjectService.Application.UseCases.Tasks.DTOs;

public sealed record ProjectTaskDto
{
    public Guid Id { get; init; }
    public Guid ProjectId { get; init; }
    public string Name { get; init; }
    public string? Comment { get; init; }
    public int Priority { get; init; }
    public ProjectTaskStatus Status { get; init; }
    public EmployeeDto Author { get; init; }
    public EmployeeDto? Assignee { get; init; }
}
