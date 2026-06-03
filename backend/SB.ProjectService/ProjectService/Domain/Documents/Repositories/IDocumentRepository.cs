namespace ProjectService.Domain.Documents.Repositories;

public interface IDocumentRepository
{
    Task AddAsync(Document document);
    Task<Document?> GetByIdAsync(Guid id);
    Task<IEnumerable<Document>> GetByProjectIdAsync(Guid projectId);
    void Delete(Document document);
}
