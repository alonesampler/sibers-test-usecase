using ProjectService.Api.Contracts;
using ProjectService.Application.UseCases.Employees.Writing;

namespace ProjectService.Api.Endpoints.Employees;

public static class CreateEmployeeEndpoint
{
    public static async Task<IResult> Handle(
        ParamsEmployeeRequest request,
        CreateEmployeeUseCase useCase,
        CancellationToken ct)
    {
        var command = new CreateEmployee
        {
            Email = request.Email,
            Name = request.Name,
            Surname = request.Surname,
            Patronymic = request.Patronymic
        };

        var result = await useCase.Handle(command, ct);


        return result.IsSuccess
            ? Results.Created()
            : Results.BadRequest(result.Errors);
    }
}
