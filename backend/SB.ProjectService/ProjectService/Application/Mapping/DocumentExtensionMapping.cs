using ProjectService.Application.UseCases.Document.DTOs;
using ProjectService.Domain.Documents;

namespace ProjectService.Application.Mapping;

public static class DocumentExtensionMapping
{
    public static DocumentDto ToResponseDto(this Document document)
        => new()
        {
            Id = document.Id,
            ProjectId = document.ProjectId,
            FileName = document.FileName,
            ContentType = document.ContentType,
            UploadedAt = document.UploadedAt
        };

    public static DocumentDownloadDto ToDownloadDto(this Document document)
        => new()
        {
            FileName = document.FileName,
            ContentType = document.ContentType,
            Content = document.Content
        };

    public static IEnumerable<DocumentDto> ToResponseDtos(this IEnumerable<Document> documents)
        => documents.Select(ToResponseDto);
}
