using FluentResults;
using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Domain.Employees;

namespace ProjectService.Application.UseCases.Employees.Writing;

public sealed record UpdateEmployeeCommand
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public string? Patronymic { get; init; }
    public string Email { get; init; }
}

public class UpdateEmployeeUseCase(IUnitOfWork uow)
{
    public async ValueTask<Result> Handle(UpdateEmployeeCommand command, CancellationToken ct)
    {
        var employee = await uow.EmployeeRepository.GetByIdAsync(command.Id);
        if (employee is null)
            return Result.Fail(EmployeeError.NotFound);

        var emailOwner = await uow.EmployeeRepository.GetByEmailAsync(command.Email);
        if (emailOwner is not null && emailOwner.Id != command.Id)
            return Result.Fail(EmployeeError.EmailExists);

        var updated = employee.Update(
            new FullName(command.Name, command.Surname, command.Patronymic),
            command.Email);

        if (updated.IsFailed)
            return updated.ToResult();

        uow.EmployeeRepository.Update(updated.Value);
        await uow.SaveAsync(ct);
        return Result.Ok();
    }
}
