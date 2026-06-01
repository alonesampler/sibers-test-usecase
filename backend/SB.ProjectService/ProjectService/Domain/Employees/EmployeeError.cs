using FluentResults;

namespace ProjectService.Domain.Employees;

public static class EmployeeError
{
    public static Error Manager
        => new Error($"Вы не можете удалить сотрудника если он руководит проектом")
            .WithErrorCode("E101");

    public static Error EmailExists
        => new Error("Email уже используется другим сотрудником")
            .WithErrorCode("E102");

    public static Error NotFound
        => new Error($"Работник не найден")
            .WithErrorCode("E404");
}
