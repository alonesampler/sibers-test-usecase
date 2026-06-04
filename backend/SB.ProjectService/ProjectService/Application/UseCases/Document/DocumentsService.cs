using FluentResults;
using ProjectService.Application.Dependencies.Services;
using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Application.Mapping;
using ProjectService.Application.UseCases.Document.DTOs;
using ProjectService.Domain.Documents;
using ProjectService.Domain.Projects;

namespace ProjectService.Application.UseCases.Document;

public class DocumentsService(IUnitOfWork uow) : IDocumentsService
{
    public async Task<Result<IEnumerable<DocumentDto>>> GetByProjectIdAsync(Guid projectId)
    {
        var documents = await uow.DocumentRepository.GetByProjectIdAsync(projectId);
        return Result.Ok(documents.ToResponseDtos());
    }

    public async Task<Result<DocumentDownloadDto>> DownloadAsync(Guid id)
    {
        var document = await uow.DocumentRepository.GetByIdAsync(id);
        if (document is null)
            return Result.Fail(DocumentError.NotFound);

        return document.ToDownloadDto();
    }

    public async Task<Result> UploadAsync(Guid projectId, string fileName, string contentType, byte[] content, CancellationToken ct)
    {
        if (content is null || content.Length == 0)
            return Result.Fail(DocumentError.InvalidFile);

        var project = await uow.ProjectRepository.GetByIdAsync(projectId);
        if (project is null)
            return Result.Fail(ProjectError.NotFound);

        var document = Domain.Documents.Document.Create(projectId, fileName, contentType, content);
        await uow.DocumentRepository.AddAsync(document);
        await uow.SaveAsync(ct);
        return Result.Ok();
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken ct)
    {
        var document = await uow.DocumentRepository.GetByIdAsync(id);
        if (document is null)
            return Result.Fail(DocumentError.NotFound);

        uow.DocumentRepository.Delete(document);
        await uow.SaveAsync(ct);
        return Result.Ok();
    }
}
