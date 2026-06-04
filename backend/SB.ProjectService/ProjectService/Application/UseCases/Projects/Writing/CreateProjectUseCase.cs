using FluentResults;
using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Domain.Employees;
using ProjectService.Domain.Projects;

namespace ProjectService.Application.UseCases.Projects.Writing;

public sealed record CreateProjectCommand
{
    public string Name { get; init; }
    public string CustomerCompanyName { get; init; }
    public string ExecutorCompanyName { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public int Priority { get; init; }
    public Guid ManagerId { get; init; }
    public Guid[] EmployeeIds { get; init; }
}

public class CreateProjectUseCase(IUnitOfWork uow)
{
    public async ValueTask<Result> Handle(CreateProjectCommand command, CancellationToken ct)
    {
        var manager = await uow.EmployeeRepository.GetByIdAsync(command.ManagerId);
        if (manager is null)
            return Result.Fail(EmployeeError.NotFound);

        var employees = await uow.EmployeeRepository.GetByIdsAsync(command.EmployeeIds);

        var projectResult = Project.Create(
            command.Name,
            command.CustomerCompanyName,
            command.ExecutorCompanyName,
            command.StartDate,
            command.EndDate,
            command.Priority,
            manager,
            employees);

        if (projectResult.IsFailed)
            return projectResult.ToResult();

        await uow.ProjectRepository.AddAsync(projectResult.Value);
        await uow.SaveAsync(ct);
        return Result.Ok();
    }
}
