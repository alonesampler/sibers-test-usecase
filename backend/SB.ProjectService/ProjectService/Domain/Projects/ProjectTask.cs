using FluentResults;
using ProjectService.Domain.Employees;

namespace ProjectService.Domain.Projects;

public sealed class ProjectTask(Guid id) : Entity<Guid>(id)
{
    public Guid ProjectId { get; private set; }
    public string Name { get; private set; }
    public string? Comment { get; private set; }
    public int Priority { get; private set; }
    public ProjectTaskStatus Status { get; private set; }
    public Guid AuthorId { get; private set; }
    public Employee Author { get; private set; } = null!;
    public Guid? AssigneeId { get; private set; }
    public Employee? Assignee { get; private set; }

    public static ProjectTask Create(
        Guid projectId,
        string name,
        string? comment,
        int priority,
        Employee author)
        => new(Guid.CreateVersion7())
        {
            ProjectId = projectId,
            Name = name,
            Comment = comment,
            Priority = priority,
            Status = ProjectTaskStatus.ToDo,
            AuthorId = author.Id,
            Author = author
        };

    public Result Update(
        string name,
        string? comment,
        int priority,
        ProjectTaskStatus status,
        Employee? assignee)
    {
        Name = name;
        Comment = comment;
        Priority = priority;
        Status = status;
        AssigneeId = assignee?.Id;
        Assignee = assignee;
        return Result.Ok();
    }
}
