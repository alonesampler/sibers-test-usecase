using ProjectService.Application.UseCases.Projects.DTOs;
using ProjectService.Domain.Projects;

namespace ProjectService.Application.Mapping;

public static class ProjectExtensionMapping
{
    public static ProjectDto ToResponseDto(this Project project)
        => new()
        {
            Id = project.Id,
            Name = project.Name,
            CustomerCompanyName = project.CustomerCompanyName,
            ExecutorCompanyName = project.ExecutorCompanyName,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            Priority = project.Priority,
            Manager = project.Manager.ToResponseDto(),
            Employees = project.Employees.ToResponseDtos().ToArray()
        };

    public static IEnumerable<ProjectDto> ToResponseDtos(this IEnumerable<Project> projects)
        => projects.Select(ToResponseDto);
}
