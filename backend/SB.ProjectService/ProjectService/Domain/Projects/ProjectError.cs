using FluentResults;

namespace ProjectService.Domain.Projects;

public static class ProjectError
{
    public static Error NotFound
        => new Error("Project not found")
            .WithErrorCode("P404");

    public static Error InvalidDates
        => new Error("The start date must be before the end date")
            .WithErrorCode("P101");

    public static Error InvalidPriority
        => new Error("The priority should be from 1 to 10")
            .WithErrorCode("P102");

    public static Error ManagerCannotBeEmployee
        => new Error("The manager cannot be the project executor")
            .WithErrorCode("P103");

    public static Error EmployeeAlreadyInProject
        => new Error("The employee has already been added to the project.")
            .WithErrorCode("P104");

    public static Error EmployeeNotInProject
        => new Error("The employee was not found in the project.")
            .WithErrorCode("P105");
}
