using FluentResults;
using ProjectService.Application.UseCases.Tasks.DTOs;
using ProjectService.Domain.Projects;

namespace ProjectService.Application.Dependencies.Services;

public interface IGettingProjectTasksService
{
    Task<Result<IEnumerable<ProjectTaskDto>>> GetAllAsync(Guid projectId, ProjectTaskFilter filter, TaskSort sort);
    Task<Result<ProjectTaskDto>> GetByIdAsync(Guid id);
}