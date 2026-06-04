using ProjectService.Api.Contracts;
using ProjectService.Application.UseCases.Tasks.Writing;

namespace ProjectService.Api.Endpoints.Tasks;

public static class CreateProjectTaskEndpoint
{
    public static async Task<IResult> Handle(
        Guid projectId,
        ParamsProjectTaskRequest request,
        CreateProjectTaskUseCase useCase,
        CancellationToken ct)
    {
        var command = new CreateProjectTaskCommand
        {
            ProjectId = projectId,
            Name = request.Name,
            Comment = request.Comment,
            Priority = request.Priority,
            AuthorId = request.AuthorId,
            AssigneeId = request.AssigneeId
        };

        var result = await useCase.Handle(command, ct);
        return result.IsSuccess
            ? Results.Created()
            : Results.BadRequest(result.Errors);
    }
}
