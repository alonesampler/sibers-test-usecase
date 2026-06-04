using ProjectService.Application.Dependencies.Services;
using ProjectService.Domain.Projects;

namespace ProjectService.Api.Endpoints.Tasks;

public static class GetProjectTasksEndpoint
{
    public static async Task<IResult> GetAll(
        Guid projectId,
        [AsParameters] ProjectTaskFilter filter,
        TaskSort sort,
        IGettingProjectTasksService service,
        CancellationToken ct)
    {
        var result = await service.GetAllAsync(projectId, filter, sort);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }

    public static async Task<IResult> GetById(
        Guid id,
        IGettingProjectTasksService service,
        CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.Errors);
    }
}
