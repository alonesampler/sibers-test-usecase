using FluentResults;
using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Domain.Projects;

namespace ProjectService.Application.UseCases.Projects.Writing;

public class DeleteProjectUseCase(IUnitOfWork uow)
{
    public async ValueTask<Result> Handle(Guid id, CancellationToken ct)
    {
        var project = await uow.ProjectRepository.GetByIdAsync(id);
        if (project is null)
            return Result.Fail(ProjectError.NotFound);

        uow.ProjectRepository.Delete(project);
        await uow.SaveAsync(ct);
        return Result.Ok();
    }
}
