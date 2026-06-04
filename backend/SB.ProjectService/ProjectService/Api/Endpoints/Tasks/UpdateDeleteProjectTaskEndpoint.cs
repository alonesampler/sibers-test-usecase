using ProjectService.Api.Contracts;
using ProjectService.Application.UseCases.Tasks.Writing;

namespace ProjectService.Api.Endpoints.Tasks;

public static class UpdateDeleteProjectTaskEndpoint
{
    public static async Task<IResult> Update(
        Guid id,
        ParamsProjectTaskRequest request,
        UpdateProjectTaskUseCase useCase,
        CancellationToken ct)
    {
        var command = new UpdateProjectTaskCommand
        {
            Id = id,
            Name = request.Name,
            Comment = request.Comment,
            Priority = request.Priority,
            Status = request.Status,
            AssigneeId = request.AssigneeId
        };

        var result = await useCase.Handle(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Errors);
    }

    public static async Task<IResult> Delete(
        Guid id,
        DeleteProjectTaskUseCase useCase,
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
