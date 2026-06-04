using System.ComponentModel.DataAnnotations;

namespace ProjectService.Api.Contracts;

public record ParamsProjectRequest : IValidatableObject
{
    public required string Name { get; init; }
    public required string CustomerCompanyName { get; init; }
    public required string ExecutorCompanyName { get; init; }
    public required DateOnly StartDate { get; init; }
    public required DateOnly EndDate { get; init; }
    public required int Priority { get; init; }
    public required Guid ManagerId { get; init; }
    public required Guid[] EmployeeIds { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (StartDate >= EndDate)
            yield return new ValidationResult("Start date must be earlier than end date", [nameof(StartDate), nameof(EndDate)]);

        if (Priority < 0 || Priority > 10)
            yield return new ValidationResult("Priority must be non-negative", [nameof(Priority)]);

        if (ManagerId == Guid.Empty)
            yield return new ValidationResult("Manager is required", [nameof(ManagerId)]);
    }
}
