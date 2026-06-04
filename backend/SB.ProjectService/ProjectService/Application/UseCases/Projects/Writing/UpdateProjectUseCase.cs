using FluentResults;
using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Domain.Employees;
using ProjectService.Domain.Projects;

namespace ProjectService.Application.UseCases.Projects.Writing;

public sealed record UpdateProjectCommand
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string CustomerCompanyName { get; init; }
    public string ExecutorCompanyName { get; init; }
    public DateOnly StartDate { get; init; }
    public DateOnly EndDate { get; init; }
    public int Priority { get; init; }
    public Guid ManagerId { get; init; }
    public Guid[] EmployeeIds { get; init; }
}

public class UpdateProjectUseCase(IUnitOfWork uow)
{
    public async ValueTask<Result> Handle(UpdateProjectCommand command, CancellationToken ct)
    {
        var project = await uow.ProjectRepository.GetByIdAsync(command.Id);
        if (project is null)
            return Result.Fail(ProjectError.NotFound);

        var manager = await uow.EmployeeRepository.GetByIdAsync(command.ManagerId);
        if (manager is null)
            return Result.Fail(EmployeeError.NotFound);

        var employees = await uow.EmployeeRepository.GetByIdsAsync(command.EmployeeIds);

        var result = project.Update(
            command.Name,
            command.CustomerCompanyName,
            command.ExecutorCompanyName,
            command.StartDate,
            command.EndDate,
            command.Priority,
            manager,
            employees);

        if (result.IsFailed)
            return result;

        await uow.SaveAsync(ct);
        return Result.Ok();
    }
}
