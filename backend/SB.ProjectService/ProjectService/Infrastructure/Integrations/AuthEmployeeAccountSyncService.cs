using FluentResults;
using Microsoft.Extensions.Options;
using ProjectService.Application.Dependencies.Services;

namespace ProjectService.Infrastructure.Integrations;

public class AuthEmployeeAccountSyncService(
    HttpClient httpClient,
    IOptions<AuthServiceSettings> options) : IEmployeeAccountSyncService
{
    public async Task<Result> SyncEmailAsync(Guid employeeId, string email, CancellationToken ct = default)
    {
        var settings = options.Value;
        using var request = new HttpRequestMessage(HttpMethod.Put, "api/v1/auth/internal/sync-email");
        request.Headers.Add("X-Internal-Api-Key", settings.InternalApiKey);
        request.Content = JsonContent.Create(new { employeeId, email });

        HttpResponseMessage response;
        try
        {
            response = await httpClient.SendAsync(request, ct);
        }
        catch (Exception ex)
        {
            return Result.Fail($"Auth service is unavailable: {ex.Message}");
        }

        if (response.IsSuccessStatusCode)
            return Result.Ok();

        var body = await response.Content.ReadAsStringAsync(ct);
        return Result.Fail(string.IsNullOrWhiteSpace(body)
            ? $"Auth sync failed with status {(int)response.StatusCode}"
            : body);
    }
}