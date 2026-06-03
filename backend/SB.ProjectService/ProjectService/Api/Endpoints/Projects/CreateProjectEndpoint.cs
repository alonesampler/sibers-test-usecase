using ProjectService.Api.Contracts;
using ProjectService.Application.UseCases.Projects.Writing;

namespace ProjectService.Api.Endpoints.Projects;

public static class CreateProjectEndpoint
{
    public static async Task<IResult> Handle(
        ParamsProjectRequest request,
        CreateProjectUseCase useCase,
        CancellationToken ct)
    {
        var command = new CreateProjectCommand
        {
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
            ? Results.Created()
            : Results.BadRequest(result.Errors);
    }
}
