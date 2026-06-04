namespace ProjectService.Domain.Projects.Repositories;

public interface IProjectTaskRepository
{
    Task AddAsync(ProjectTask task);
    Task<ProjectTask?> GetByIdAsync(Guid id);
    Task<IEnumerable<ProjectTask>> GetAllAsync(Guid projectId, ProjectTaskFilter filter, TaskSort sort);
    void Update(ProjectTask task);
    void Delete(ProjectTask task);
}
