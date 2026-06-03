namespace ProjectService.Api.Endpoints.Projects;

public static class ProjectsEndpoints
{
    public static void MapProjectsEndpoints(this IEndpointRouteBuilder builder)
    {
        var v1Group = builder.MapGroup("api/v1/projects").WithTags("Projects");

        v1Group.MapGet("/", GetProjectsEndpoint.GetAll);
        v1Group.MapGet("/{id:guid}", GetProjectsEndpoint.GetById);
        v1Group.MapPost("/", CreateProjectEndpoint.Handle);
        v1Group.MapPut("/{id:guid}", UpdateDeleteProjectEndpoint.Update);
        v1Group.MapDelete("/{id:guid}", UpdateDeleteProjectEndpoint.Delete);
    }
}
