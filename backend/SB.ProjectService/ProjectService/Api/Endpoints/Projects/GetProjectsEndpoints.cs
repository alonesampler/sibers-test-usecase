using ProjectService.Application.Dependencies.Services;
using ProjectService.Application.UseCases.Projects.DTOs;

namespace ProjectService.Api.Endpoints.Projects;

public static class GetProjectsEndpoint
{
    public static async Task<IResult> GetAll(
        [AsParameters] ProjectFilterDto filter,
        ProjectSort sort,
        IGettingProjectsService service,
        CancellationToken ct)
    {
        var result = await service.GetAllAsync(filter, sort);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }

    public static async Task<IResult> GetById(
        Guid id,
        IGettingProjectsService service,
        CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.Errors);
    }
}
