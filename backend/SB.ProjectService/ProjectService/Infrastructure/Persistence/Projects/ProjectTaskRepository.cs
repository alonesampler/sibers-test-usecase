using Microsoft.EntityFrameworkCore;
using ProjectService.Domain.Projects;
using ProjectService.Domain.Projects.Repositories;

namespace ProjectService.Infrastructure.Persistence.Projects;

internal sealed class ProjectTaskRepository(DatabaseContext db) : IProjectTaskRepository
{
    public Task AddAsync(ProjectTask task)
        => db.ProjectTasks.AddAsync(task).AsTask();

    public Task<ProjectTask?> GetByIdAsync(Guid id)
        => db.ProjectTasks
            .Include(t => t.Author)
            .Include(t => t.Assignee)
            .FirstOrDefaultAsync(t => t.Id == id);

    public async Task<IEnumerable<ProjectTask>> GetAllAsync(Guid projectId, ProjectTaskFilter filter, TaskSort sort)
    {
        var query = db.ProjectTasks
            .Include(t => t.Author)
            .Include(t => t.Assignee)
            .AsNoTracking()
            .Where(t => t.ProjectId == projectId);

        if (filter.Status.HasValue)
            query = query.Where(t => t.Status == filter.Status.Value);

        if (filter.PriorityMin.HasValue)
            query = query.Where(t => t.Priority >= filter.PriorityMin.Value);

        if (filter.PriorityMax.HasValue)
            query = query.Where(t => t.Priority <= filter.PriorityMax.Value);

        if (filter.AssigneeId.HasValue)
            query = query.Where(t => t.AssigneeId == filter.AssigneeId.Value);

        if (filter.AuthorId.HasValue)
            query = query.Where(t => t.AuthorId == filter.AuthorId.Value);

        query = sort switch
        {
            TaskSort.NameAsc => query.OrderBy(t => t.Name),
            TaskSort.NameDesc => query.OrderByDescending(t => t.Name),
            TaskSort.PriorityAsc => query.OrderBy(t => t.Priority),
            TaskSort.PriorityDesc => query.OrderByDescending(t => t.Priority),
            TaskSort.StatusAsc => query.OrderBy(t => t.Status),
            TaskSort.StatusDesc => query.OrderByDescending(t => t.Status),
            _ => query.OrderBy(t => t.Name)
        };

        return await query.ToArrayAsync();
    }

    public void Update(ProjectTask task)
        => db.ProjectTasks.Update(task);

    public void Delete(ProjectTask task)
        => db.ProjectTasks.Remove(task);
}
