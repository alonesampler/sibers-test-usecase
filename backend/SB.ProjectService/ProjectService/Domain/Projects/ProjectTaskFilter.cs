namespace ProjectService.Domain.Projects;

public record ProjectTaskFilter
{
    public ProjectTaskStatus? Status { get; init; }
    public int? PriorityMin { get; init; }
    public int? PriorityMax { get; init; }
    public Guid? AssigneeId { get; init; }
    public Guid? AuthorId { get; init; }
}

public enum TaskSort
{
    NameAsc, NameDesc,
    PriorityAsc, PriorityDesc,
    StatusAsc, StatusDesc
}
