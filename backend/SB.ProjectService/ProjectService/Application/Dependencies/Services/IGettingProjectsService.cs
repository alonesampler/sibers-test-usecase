using FluentResults;
using ProjectService.Application.UseCases.Projects.DTOs;
using ProjectService.Domain.Projects;

namespace ProjectService.Application.Dependencies.Services;

public interface IGettingProjectsService
{
    Task<Result<ProjectDto>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<ProjectDto>>> GetAllAsync(ProjectFilter filter, ProjectSort sort);
}
