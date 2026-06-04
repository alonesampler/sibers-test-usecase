namespace ProjectService.Domain.Projects;

public record ProjectFilter
{
    public DateOnly? StartDateFrom { get; init; }
    public DateOnly? StartDateTo { get; init; }
    public int? PriorityMin { get; init; }
    public int? PriorityMax { get; init; }
    public string? Name { get; init; } 
    public string? CustomerCompanyName { get; init; }
    public string? ExecutorCompanyName { get; init; }
}

public enum ProjectSort
{
    NameAsc,
    NameDesc,
    StartDateAsc,
    StartDateDesc,
    EndDateAsc,
    EndDateDesc,
    PriorityAsc,
    PriorityDesc
}
