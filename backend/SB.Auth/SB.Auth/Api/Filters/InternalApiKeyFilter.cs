using Microsoft.Extensions.Options;
using SB.Auth.Domain;

namespace SB.Auth.Api.Filters;

public sealed class InternalApiKeyFilter(IOptions<InternalApiSettings> options) : IEndpointFilter
{
    private readonly InternalApiSettings _settings = options.Value;

    public const string HeaderName = "X-Internal-Api-Key";

    public ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (string.IsNullOrWhiteSpace(_settings.ApiKey))
            return new ValueTask<object?>(Results.Problem("Internal API key is not configured", statusCode: 503));

        if (!context.HttpContext.Request.Headers.TryGetValue(HeaderName, out var key)
            || key != _settings.ApiKey)
            return new ValueTask<object?>(Results.Unauthorized());

        return next(context);
    }
}