namespace ProjectService.Domain.Projects.Repositories;

public interface IProjectRepository
{
    Task<Project?> GetByIdAsync(Guid id);
    Task<Project[]> GetAllAsync();
    Task<Project[]> SearchAsync(
    string? name = null,
    DateOnly? startDateFrom = null,
    DateOnly? startDateTo = null,
    int? priority = null);
    Task AddAsync(Project project);
    void Update(Project project);
    void Delete(Project project);
}
