namespace ProjectService.Application.UseCases.Document.DTOs;

public sealed record DocumentDto
{
    public Guid Id { get; init; }
    public Guid ProjectId { get; init; }
    public string FileName { get; init; }
    public string ContentType { get; init; }
    public DateTime UploadedAt { get; init; }
}
