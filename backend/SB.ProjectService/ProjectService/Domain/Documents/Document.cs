namespace ProjectService.Domain.Documents;

public sealed class Document(Guid id) : Entity<Guid>(id)
{
    public Guid ProjectId { get; private set; }
    public string FileName { get; private set; }
    public string ContentType { get; private set; }
    public byte[] Content { get; private set; }
    public DateTime UploadedAt { get; private set; }

    public static Document Create(Guid projectId, string fileName, string contentType, byte[] content)
        => new(Guid.CreateVersion7())
        {
            ProjectId = projectId,
            FileName = fileName,
            ContentType = contentType,
            Content = content,
            UploadedAt = DateTime.UtcNow
        };
}
