using FluentResults;
using ProjectService.Application.Dependencies.Services;
using ProjectService.Application.Mapping;
using ProjectService.Application.UseCases.Projects.DTOs;
using ProjectService.Domain.Projects;
using ProjectService.Domain.Projects.Repositories;

namespace ProjectService.Application.UseCases.Projects.Getting;

public class GettingProjectsService(IProjectRepository projectRepository) : IGettingProjectsService
{
    public async Task<Result<IEnumerable<ProjectDto>>> GetAllAsync(ProjectFilter filter, ProjectSort sort)
    {
        var projects = await projectRepository.GetAllAsync(filter, sort);
        return Result.Ok(projects.ToResponseDtos());
    }

    public async Task<Result<ProjectDto>> GetByIdAsync(Guid id)
    {
        var project = await projectRepository.GetByIdAsync(id);
        if (project is null)
            return Result.Fail(ProjectError.NotFound);

        return Result.Ok(project.ToResponseDto());
    }
}
