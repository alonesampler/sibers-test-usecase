using ProjectService.Application.Dependencies.Services;

namespace ProjectService.Api.Endpoints.Employees;

public static class GetEmployeesEndpoint
{
    public static async Task<IResult> GetAll(
        IGettingEmployeesService service,
        CancellationToken ct)
    {
        var result = await service.GetAllAsync();
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }

    public static async Task<IResult> Search(
        string? query,
        IGettingEmployeesService service,
        CancellationToken ct)
    {
        var result = await service.SearchAsync(query);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }

    public static async Task<IResult> GetById(
        Guid id,
        IGettingEmployeesService service,
        CancellationToken ct)
    {
        var result = await service.GetByIdAsync(id);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.NotFound(result.Errors);
    }
}
