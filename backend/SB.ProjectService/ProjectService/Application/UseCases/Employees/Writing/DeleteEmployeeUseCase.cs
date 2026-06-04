using FluentResults;
using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Domain.Employees;

namespace ProjectService.Application.UseCases.Employees.Writing;

public class DeleteEmployeeUseCase(IUnitOfWork uow)
{
    public async ValueTask<Result> Handle(Guid id, CancellationToken ct)
    {
        var employee = await uow.EmployeeRepository.GetByIdAsync(id);
        if (employee is null)
            return Result.Fail(EmployeeError.NotFound);

        uow.EmployeeRepository.Delete(employee);
        await uow.SaveAsync(ct);
        return Result.Ok();
    }
}
