namespace ProjectService.Application.UseCases.Employees.DTOs;

public sealed record EmployeeDto
{
    public Guid Id { get; init; }

    public string Email { get; init; }

    public string Name { get; init; }

    public string Surname { get; init; }

    public string? Patronymic { get; init; }
}