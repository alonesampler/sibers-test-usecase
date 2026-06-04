using FluentResults;
using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Domain.Employees;
using ProjectService.Domain.Projects;

namespace ProjectService.Application.UseCases.Tasks.Writing;

public sealed record CreateTaskCommand
{
    public Guid ProjectId { get; init; }
    public string Name { get; init; }
    public string? Comment { get; init; }
    public int Priority { get; init; }
    public Guid AuthorId { get; init; }
    public Guid? AssigneeId { get; init; }
}

public class CreateTaskUseCase(IUnitOfWork uow)
{
    public async ValueTask<Result> Handle(CreateTaskCommand command, CancellationToken ct)
    {
        var project = await uow.ProjectRepository.GetByIdAsync(command.ProjectId);
        if (project is null)
            return Result.Fail(ProjectError.NotFound);

        var author = await uow.EmployeeRepository.GetByIdAsync(command.AuthorId);
        if (author is null)
            return Result.Fail(EmployeeError.NotFound);

        Employee? assignee = null;
        if (command.AssigneeId.HasValue)
        {
            assignee = await uow.EmployeeRepository.GetByIdAsync(command.AssigneeId.Value);
            if (assignee is null)
                return Result.Fail(EmployeeError.NotFound);
        }

        var task = ProjectTask.Create(command.ProjectId, command.Name, command.Comment, command.Priority, author);

        if (assignee is not null)
            task.Update(task.Name, task.Comment, task.Priority, task.Status, assignee);

        await uow.ProjectTaskRepository.AddAsync(task);
        await uow.SaveAsync(ct);
        return Result.Ok();
    }
}
