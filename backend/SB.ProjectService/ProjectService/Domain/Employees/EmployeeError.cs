using FluentResults;

namespace ProjectService.Domain.Employees;

public static class EmployeeError
{
    public static Error Manager
        => new Error($"You cannot remove an employee if he is managing a project")
            .WithErrorCode("E101");

    public static Error EmailExists
        => new Error("The email is already in use by another employee.")
            .WithErrorCode("E102");

    public static Error NotFound
        => new Error($"Employee not found")
            .WithErrorCode("E404");
}
