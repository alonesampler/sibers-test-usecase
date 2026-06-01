using ProjectService.Api.Contracts;
using ProjectService.Application.UseCases.Employees.Writing;

namespace ProjectService.Api.Endpoints.Employees;

public static class UpdateDeleteEmployeeEndpoint
{
    public static async Task<IResult> Update(
        Guid id,
        ParamsEmployeeRequest request,
        UpdateEmployeeUseCase useCase,
        CancellationToken ct)
    {
        var command = new UpdateEmployee
        {
            Id = id,
            Email = request.Email,
            Name = request.Name,
            Surname = request.Surname,
            Patronymic = request.Patronymic
        };

        var result = await useCase.Handle(command, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : Results.BadRequest(result.Errors);
    }

    public static async Task<IResult> Delete(
        Guid id,
        DeleteEmployeeUseCase useCase,
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
