namespace SB.Auth.Api.Contract;

public record SyncEmployeeEmailRequest
{
    public Guid EmployeeId { get; init; }
    public string Email { get; init; }
}