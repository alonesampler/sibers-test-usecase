using FluentResults;
using ProjectService.Application.UseCases.Projects.DTOs;

namespace ProjectService.Application.Dependencies.Services;

public interface IGettingProjectsService
{
    Task<Result<ProjectDto>> GetByIdAsync(Guid id);
    Task<Result<IEnumerable<ProjectDto>>> GetAllAsync(ProjectFilterDto filter, ProjectSort sort);
}
