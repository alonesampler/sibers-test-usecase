namespace ProjectService.Infrastructure.Integrations;

public class AuthServiceSettings
{
    public string BaseUrl { get; init; } = "http://localhost:5182";

    public string InternalApiKey { get; init; } = string.Empty;
}