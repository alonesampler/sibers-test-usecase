using ProjectService.Api.Contracts;
using ProjectService.Application.UseCases.Projects.Writing;
using static ProjectService.Application.UseCases.Projects.Writing.CreateProjectUseCase;

namespace ProjectService.Api.Endpoints.Projects;

public static class UpdateDeleteProjectEndpoint
{
    public static async Task<IResult> Update(
        Guid id,
        ParamsProjectRequest request,
        UpdateProjectUseCase useCase,
        CancellationToken ct)
    {
        var command = new UpdateProjectCommand
        {
            Id = id,
            Name = request.Name,
            CustomerCompanyName = request.CustomerCompanyName,
            ExecutorCompanyName = request.ExecutorCompanyName,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            Priority = request.Priority,
            ManagerId = request.ManagerId,
            EmployeeIds = request.EmployeeIds
        };

        var result = await useCase.Handle(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Errors);
    }

    public static async Task<IResult> Delete(
        Guid id,
        DeleteProjectUseCase useCase,
        CancellationToken ct)
    {
        var result = await useCase.Handle(id, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : result.Errors.Any(e => e.Metadata.TryGetValue("ErrorCode", out var c) && c is "E404")
                ? Results.NotFound(result.Errors)
                : Results.BadRequest(result.Errors);
    }
}
