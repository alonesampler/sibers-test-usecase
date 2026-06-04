using ProjectService.Domain.Projects;
using System.ComponentModel.DataAnnotations;

namespace ProjectService.Api.Contracts;

public record ParamsProjectTaskRequest : IValidatableObject
{
    public required Guid ProjectId { get; init; }
    public required string Name { get; init; }
    public string? Comment { get; init; }
    public required int Priority { get; init; }
    public required Guid AuthorId { get; init; }
    public Guid? AssigneeId { get; init; }
    public ProjectTaskStatus Status { get; init; } = ProjectTaskStatus.ToDo;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Priority < 1 || Priority > 10)
            yield return new ValidationResult("Priority must be between 1 and 10", [nameof(Priority)]);

        if (ProjectId == Guid.Empty)
            yield return new ValidationResult("ProjectId is required", [nameof(ProjectId)]);

        if (AuthorId == Guid.Empty)
            yield return new ValidationResult("AuthorId is required", [nameof(AuthorId)]);
    }
}
