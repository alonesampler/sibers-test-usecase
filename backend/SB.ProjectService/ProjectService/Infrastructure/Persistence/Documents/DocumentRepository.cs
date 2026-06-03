using Microsoft.EntityFrameworkCore;
using ProjectService.Domain.Documents;
using ProjectService.Domain.Documents.Repositories;
using ProjectService.Infrastructure;
using ProjectService.Infrastructure.Persistence;

namespace ProjectService.Infrastructure.Persistence.Documents;

internal sealed class DocumentRepository(DatabaseContext db) : IDocumentRepository
{
    public Task AddAsync(Document document)
        => db.Documents.AddAsync(document).AsTask();

    public Task<Document?> GetByIdAsync(Guid id)
        => db.Documents.FirstOrDefaultAsync(d => d.Id == id);

    public Task<IEnumerable<Document>> GetByProjectIdAsync(Guid projectId)
        => Task.FromResult(
            db.Documents
                .AsNoTracking()
                .Where(d => d.ProjectId == projectId)
                .AsEnumerable());

    public void Delete(Document document)
        => db.Documents.Remove(document);
}
