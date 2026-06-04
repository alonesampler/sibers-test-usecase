using ProjectService.Application.Dependencies.Services;
using ProjectService.Application.UseCases.Document;

namespace ProjectService.Api.Endpoints.Documents;

public static class DocumentsEndpoints
{
    public static void MapDocumentsEndpoints(this IEndpointRouteBuilder builder)
    {
        var v1Group = builder.MapGroup("api/v1/documents").WithTags("Documents");

        v1Group.MapPost("/", DocumentEndpoints.Upload).DisableAntiforgery();
        v1Group.MapGet("/", DocumentEndpoints.GetByProject);
        v1Group.MapGet("/{id:guid}/download", DocumentEndpoints.Download);
        v1Group.MapDelete("/{id:guid}", DocumentEndpoints.Delete);
    }
}

public static class DocumentEndpoints
{
    public static async Task<IResult> Upload(
        Guid projectId,
        IFormFile file,
        IDocumentsService service,
        CancellationToken ct)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms, ct);

        var result = await service.UploadAsync(
            projectId,
            file.FileName,
            file.ContentType,
            ms.ToArray(),
            ct);

        return result.IsSuccess
            ? Results.Created()
            : Results.BadRequest(result.Errors);
    }

    public static async Task<IResult> GetByProject(
        Guid projectId,
        IDocumentsService service)
    {
        var result = await service.GetByProjectIdAsync(projectId);
        return result.IsSuccess
            ? Results.Ok(result.Value)
            : Results.BadRequest(result.Errors);
    }

    public static async Task<IResult> Download(
        Guid id,
        IDocumentsService service)
    {
        var result = await service.DownloadAsync(id);
        return result.IsSuccess
            ? Results.File(result.Value.Content, result.Value.ContentType, result.Value.FileName)
            : Results.NotFound(result.Errors);
    }

    public static async Task<IResult> Delete(
        Guid id,
        IDocumentsService service,
        CancellationToken ct)
    {
        var result = await service.DeleteAsync(id, ct);
        return result.IsSuccess
            ? Results.NoContent()
            : result.Errors.Any(e => e.Metadata.TryGetValue("ErrorCode", out var c) && c is "E404")
                ? Results.NotFound(result.Errors)
                : Results.BadRequest(result.Errors);
    }
}
