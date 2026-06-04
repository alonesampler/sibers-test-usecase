namespace SB.Auth.Api.Contract;

public record RegisterRequest
{
    public Guid EmployeeId { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string Role { get; init; }
}
