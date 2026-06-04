namespace ProjectService.Api.Endpoints.Tasks;

public static class ProjectTasksEndpoints
{
    public static void MapTasksEndpoints(this IEndpointRouteBuilder builder)
    {
        var v1Group = builder.MapGroup("api/v1/tasks").WithTags("Tasks");

        v1Group.MapGet("/", GetProjectTasksEndpoint.GetAll);
        v1Group.MapGet("/{id:guid}", GetProjectTasksEndpoint.GetById);
        v1Group.MapPost("/", CreateProjectTaskEndpoint.Handle);
        v1Group.MapPut("/{id:guid}", UpdateDeleteProjectTaskEndpoint.Update);
        v1Group.MapDelete("/{id:guid}", UpdateDeleteProjectTaskEndpoint.Delete);
    }
}
