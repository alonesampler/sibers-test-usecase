using FluentResults;
using ProjectService.Application.Dependencies.Services;
using ProjectService.Application.Mapping;
using ProjectService.Application.UseCases.Tasks.DTOs;
using ProjectService.Domain.Projects;
using ProjectService.Domain.Projects.Repositories;

namespace ProjectService.Application.UseCases.Tasks.Getting;

public class GettingProjectTasksService(IProjectTaskRepository taskRepository) : IGettingProjectTasksService
{
    public async Task<Result<IEnumerable<ProjectTaskDto>>> GetAllAsync(Guid projectId, ProjectTaskFilter filter, TaskSort sort)
    {
        var tasks = await taskRepository.GetAllAsync(projectId, filter, sort);
        return Result.Ok(tasks.ToResponseDtos());
    }

    public async Task<Result<ProjectTaskDto>> GetByIdAsync(Guid id)
    {
        var task = await taskRepository.GetByIdAsync(id);
        if (task is null)
            return Result.Fail(ProjectTaskError.NotFound);

        return Result.Ok(task.ToResponseDto());
    }
}