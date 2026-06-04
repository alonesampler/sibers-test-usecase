using FluentResults;
using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Domain.Projects;

namespace ProjectService.Application.UseCases.Tasks.Writing;

public class DeleteTaskUseCase(IUnitOfWork uow)
{
    public async ValueTask<Result> Handle(Guid id, CancellationToken ct)
    {
        var task = await uow.ProjectTaskRepository.GetByIdAsync(id);
        if (task is null)
            return Result.Fail(ProjectTaskError.NotFound);

        uow.ProjectTaskRepository.Delete(task);
        await uow.SaveAsync(ct);
        return Result.Ok();
    }
}
