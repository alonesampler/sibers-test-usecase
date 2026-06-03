namespace ProjectService.Application.UseCases.Document.DTOs;

public sealed record DocumentDownloadDto
{
    public string FileName { get; init; }
    public string ContentType { get; init; }
    public byte[] Content { get; init; }
}
