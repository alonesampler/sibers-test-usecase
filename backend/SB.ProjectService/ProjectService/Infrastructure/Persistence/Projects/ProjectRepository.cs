using Microsoft.EntityFrameworkCore;
using ProjectService.Application.UseCases.Projects.DTOs;
using ProjectService.Domain.Projects;
using ProjectService.Domain.Projects.Repositories;

namespace ProjectService.Infrastructure.Persistence.Projects;

internal sealed class ProjectRepository(DatabaseContext db) : IProjectRepository
{
    public Task AddAsync(Project project)
        => db.Projects.AddAsync(project).AsTask();

    public void Delete(Project project)
        => db.Projects.Remove(project);

    public void Update(Project project)
        => db.Projects.Update(project);

    public Task<Project?> GetByIdAsync(Guid id)
        => db.Projects
            .Include(p => p.Manager)
            .Include(p => p.Employees)
            .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Project>> GetAllAsync(ProjectFilterDto filter, ProjectSort sort)
    {
        var query = db.Projects
            .Include(p => p.Manager)
            .Include(p => p.Employees)
            .AsNoTracking()
            .AsQueryable();

        if (filter.StartDateFrom.HasValue)
            query = query.Where(p => p.StartDate >= filter.StartDateFrom.Value);

        if (filter.StartDateTo.HasValue)
            query = query.Where(p => p.StartDate <= filter.StartDateTo.Value);

        if (filter.PriorityMin.HasValue)
            query = query.Where(p => p.Priority >= filter.PriorityMin.Value);

        if (filter.PriorityMax.HasValue)
            query = query.Where(p => p.Priority <= filter.PriorityMax.Value);

        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(p => p.Name.Contains(filter.Name));

        if (!string.IsNullOrWhiteSpace(filter.CustomerCompanyName))
            query = query.Where(p => p.CustomerCompanyName.Contains(filter.CustomerCompanyName));

        if (!string.IsNullOrWhiteSpace(filter.ExecutorCompanyName))
            query = query.Where(p => p.ExecutorCompanyName.Contains(filter.ExecutorCompanyName));

        query = sort switch
        {
            ProjectSort.NameAsc => query.OrderBy(p => p.Name),
            ProjectSort.NameDesc => query.OrderByDescending(p => p.Name),
            ProjectSort.StartDateAsc => query.OrderBy(p => p.StartDate),
            ProjectSort.StartDateDesc => query.OrderByDescending(p => p.StartDate),
            ProjectSort.EndDateAsc => query.OrderBy(p => p.EndDate),
            ProjectSort.EndDateDesc => query.OrderByDescending(p => p.EndDate),
            ProjectSort.PriorityAsc => query.OrderBy(p => p.Priority),
            ProjectSort.PriorityDesc => query.OrderByDescending(p => p.Priority),
            _ => query.OrderBy(p => p.Name)
        };

        return await query.ToArrayAsync();
    }
}
