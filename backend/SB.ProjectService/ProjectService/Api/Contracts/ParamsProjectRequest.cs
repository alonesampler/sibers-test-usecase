using System.ComponentModel.DataAnnotations;

namespace ProjectService.Api.Contracts;

public class ParamsProjectRequest : IValidatableObject
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
        if (string.IsNullOrWhiteSpace(Name))
            yield return new ValidationResult("Name is required", [nameof(Name)]);

        if (string.IsNullOrWhiteSpace(CustomerCompanyName))
            yield return new ValidationResult("Customer company name is required", [nameof(CustomerCompanyName)]);

        if (string.IsNullOrWhiteSpace(ExecutorCompanyName))
            yield return new ValidationResult("Executor company name is required", [nameof(ExecutorCompanyName)]);

        if (StartDate >= EndDate)
            yield return new ValidationResult("Start date must be earlier than end date", [nameof(StartDate), nameof(EndDate)]);

        if (Priority < 0 || Priority > 10)
            yield return new ValidationResult("Priority must be non-negative", [nameof(Priority)]);

        if (ManagerId == Guid.Empty)
            yield return new ValidationResult("Manager is required", [nameof(ManagerId)]);
    }
}
