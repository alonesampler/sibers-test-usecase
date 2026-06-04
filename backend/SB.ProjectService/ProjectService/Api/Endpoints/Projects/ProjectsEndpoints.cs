using ProjectService.Api.Policy;

namespace ProjectService.Api.Endpoints.Projects;

public static class ProjectsEndpoints
{
    public static void MapProjectsEndpoints(this IEndpointRouteBuilder builder)
    {
        var v1Group = builder.MapGroup("api/v1/projects").WithTags("Projects");

        v1Group.MapGet("/", GetProjectsEndpoint.GetAll)
            .RequireAuthorization();

        v1Group.MapGet("/{id:guid}", GetProjectsEndpoint.GetById)
            .RequireAuthorization();

        v1Group.MapPost("/", CreateProjectEndpoint.Handle)
            .RequireAuthorization(policy => policy.RequireRole(AppRoles.Director, AppRoles.ProjectManager));

        v1Group.MapPut("/{id:guid}", UpdateDeleteProjectEndpoint.Update)
            .RequireAuthorization(policy => policy.RequireRole(AppRoles.Director, AppRoles.ProjectManager));

        v1Group.MapDelete("/{id:guid}", UpdateDeleteProjectEndpoint.Delete)
            .RequireAuthorization(policy => policy.RequireRole(AppRoles.Director));
    }
}
