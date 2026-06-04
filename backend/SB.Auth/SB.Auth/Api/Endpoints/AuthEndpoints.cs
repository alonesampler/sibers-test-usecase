using SB.Auth.Api.Contract;
using SB.Auth.Api.Filters;
using SB.Auth.Application.UseCases;
using SB.Auth.Domain;
using SB.Auth.Domain.User;

namespace SB.Auth.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder builder)
    {
        var group = builder.MapGroup("api/v1/auth").WithTags("Auth");

        group.MapPost("/register", Register)
            .RequireAuthorization(policy => policy.RequireRole(AppRoles.Director));

        group.MapPost("/login", Login);

        group.MapGet("/user-accounts", GetUserAccounts)
            .RequireAuthorization(policy => policy.RequireRole(AppRoles.Director));

        group.MapPut("/internal/sync-email", SyncEmployeeEmail)
            .AddEndpointFilter<InternalApiKeyFilter>();
    }

    private static async Task<IResult> Register(
        RegisterRequest request,
        RegisterUseCase useCase,
        CancellationToken ct)
    {
        var command = new RegisterCommand
        {
            EmployeeId = request.EmployeeId,
            Email = request.Email,
            Password = request.Password,
            Role = request.Role
        };

        var result = await useCase.Handle(command, ct);
        return result.IsSuccess
            ? Results.Created()
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> Login(
        LoginRequest request,
        LoginUseCase useCase)
    {
        var command = new LoginCommand
        {
            Email = request.Email,
            Password = request.Password
        };

        var result = await useCase.Handle(command);
        return result.IsSuccess
            ? Results.Ok(new { token = result.Value })
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> GetUserAccounts(
        GetUserAccountsUseCase useCase,
        CancellationToken ct)
    {
        var result = await useCase.Handle(ct);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }

    private static async Task<IResult> SyncEmployeeEmail(
        SyncEmployeeEmailRequest request,
        SyncEmployeeEmailUseCase useCase,
        CancellationToken ct)
    {
        var result = await useCase.Handle(
            new SyncEmployeeEmailCommand
            {
                EmployeeId = request.EmployeeId,
                Email = request.Email
            },
            ct);

        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Errors);
    }
}