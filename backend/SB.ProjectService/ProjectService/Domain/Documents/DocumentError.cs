using FluentResults;

namespace ProjectService.Domain.Documents;

public static class DocumentError
{
    public static Error InvalidFile
        => new Error("File is empty or invalid")
            .WithErrorCode("D400");

    public static Error NotFound
        => new Error("Document not found")
            .WithErrorCode("D404");
}
