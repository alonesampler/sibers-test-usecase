namespace SB.Auth.Api.Contract;

public record LoginRequest
{
    public string Email { get; init; }
    public string Password { get; init; }
}
