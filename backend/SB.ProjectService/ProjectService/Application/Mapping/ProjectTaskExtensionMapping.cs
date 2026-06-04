using ProjectService.Application.UseCases.Tasks.DTOs;
using ProjectService.Domain.Projects;

namespace ProjectService.Application.Mapping;

public static class ProjectTaskExtensionMapping
{
    public static ProjectTaskDto ToResponseDto(this ProjectTask task)
        => new()
        {
            Id = task.Id,
            ProjectId = task.ProjectId,
            Name = task.Name,
            Comment = task.Comment,
            Priority = task.Priority,
            Status = task.Status,
            Author = task.Author.ToResponseDto(),
            Assignee = task.Assignee?.ToResponseDto()
        };

    public static IEnumerable<ProjectTaskDto> ToResponseDtos(this IEnumerable<ProjectTask> tasks)
        => tasks.Select(ToResponseDto);
}
