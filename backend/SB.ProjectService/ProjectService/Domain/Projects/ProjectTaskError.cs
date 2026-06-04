using FluentResults;

namespace ProjectService.Domain.Projects;

public static class ProjectTaskError
{
    public static Error NotFound
        => new Error("Task not found")
            .WithErrorCode("T404");

    public static Error InvalidPriority
        => new Error("The priority should be from 1 to 10")
            .WithErrorCode("T400");
}