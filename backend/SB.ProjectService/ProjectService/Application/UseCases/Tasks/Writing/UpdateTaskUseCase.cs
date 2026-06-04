using FluentResults;
using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Domain.Employees;
using ProjectService.Domain.Projects;

namespace ProjectService.Application.UseCases.Tasks.Writing;

public sealed record UpdateTaskCommand
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string? Comment { get; init; }
    public int Priority { get; init; }
    public ProjectTaskStatus Status { get; init; }
    public Guid? AssigneeId { get; init; }
}

public class UpdateTaskUseCase(IUnitOfWork uow)
{
    public async ValueTask<Result> Handle(UpdateTaskCommand command, CancellationToken ct)
    {
        var task = await uow.ProjectTaskRepository.GetByIdAsync(command.Id);
        if (task is null)
            return Result.Fail(ProjectTaskError.NotFound);

        Employee? assignee = null;
        if (command.AssigneeId.HasValue)
        {
            assignee = await uow.EmployeeRepository.GetByIdAsync(command.AssigneeId.Value);
            if (assignee is null)
                return Result.Fail(EmployeeError.NotFound);
        }

        var result = task.Update(command.Name, command.Comment, command.Priority, command.Status, assignee);
        if (result.IsFailed)
            return result;

        await uow.SaveAsync(ct);
        return Result.Ok();
    }
}
