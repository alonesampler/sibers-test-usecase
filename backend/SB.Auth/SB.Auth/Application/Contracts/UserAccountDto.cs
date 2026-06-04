namespace SB.Auth.Application.Contracts;

public sealed record UserAccountDto
{
    public Guid EmployeeId { get; init; }
    public string Email { get; init; }
    public string[] Roles { get; init; } = [];
}