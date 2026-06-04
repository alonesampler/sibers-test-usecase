using ProjectService.Api.Policy;

namespace ProjectService.Api.Endpoints.Tasks;

public static class ProjectTasksEndpoints
{
    public static void MapTasksEndpoints(this IEndpointRouteBuilder builder)
    {
        var tasksGroup = builder.MapGroup("api/v1/projects/{projectId}/tasks").WithTags("Tasks");

        tasksGroup.MapGet("/", GetProjectTasksEndpoint.GetAll)
            .RequireAuthorization();
        tasksGroup.MapPost("/", CreateProjectTaskEndpoint.Handle)
            .RequireAuthorization(policy => policy.RequireRole(AppRoles.Director, AppRoles.ProjectManager));

        var v1Group = builder.MapGroup("api/v1/tasks").WithTags("Tasks");

        v1Group.MapGet("/{id:guid}", GetProjectTasksEndpoint.GetById)
            .RequireAuthorization();

        v1Group.MapPut("/{id:guid}", UpdateDeleteProjectTaskEndpoint.Update)
            .RequireAuthorization(policy => policy.RequireRole(AppRoles.Director, AppRoles.ProjectManager));

        v1Group.MapDelete("/{id:guid}", UpdateDeleteProjectTaskEndpoint.Delete)
            .RequireAuthorization(policy => policy.RequireRole(AppRoles.Director));
    }
}
