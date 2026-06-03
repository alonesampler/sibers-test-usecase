using FluentResults;
using ProjectService.Application.UseCases.Document.DTOs;

namespace ProjectService.Application.Dependencies.Services;

public interface IDocumentsService
{
    Task<Result> DeleteAsync(Guid id);
    Task<Result<DocumentDownloadDto>> DownloadAsync(Guid id);
    Task<Result<IEnumerable<DocumentDto>>> GetByProjectIdAsync(Guid projectId);
    Task<Result> UploadAsync(Guid projectId, string fileName, string contentType, byte[] content);
}