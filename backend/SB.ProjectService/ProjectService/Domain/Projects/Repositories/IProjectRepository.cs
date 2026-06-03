using ProjectService.Application.UseCases.Projects.DTOs;

namespace ProjectService.Domain.Projects.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id);
    Task<IEnumerable<Project>> GetAllAsync(ProjectFilterDto filter, ProjectSort sort);
    Task AddAsync(Project project);
    void Update(Project project);
    void Delete(Project project);
}
