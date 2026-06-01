using FluentResults;
using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Domain.Employees;

namespace ProjectService.Application.UseCases.Employees.Writing;


public sealed record CreateEmployee
{
    public string Email { get; init; }

    public string Name { get; init; }

    public string Surname { get; init; }

    public string? Patronymic { get; init; }
}

public class CreateEmployeeUseCase(IUnitOfWork uow)
{
    public async ValueTask<Result> Handle(
        CreateEmployee command,
        CancellationToken cancellationToken)
    {
        var existing = await uow.EmployeeRepository.GetByEmailAsync(command.Email);
        if (existing is not null)
            return Result.Fail(EmployeeError.EmailExists);

        var fullName = new FullName(command.Name, command.Surname, command.Patronymic);
        var employeeResult = Employee.Create(fullName, command.Email);

        if (employeeResult.IsFailed)
            return employeeResult.ToResult();

        var employee = employeeResult.Value;
        await uow.EmployeeRepository.AddAsync(employee);
        await uow.SaveAsync(cancellationToken);
        return Result.Ok();
    }
}
