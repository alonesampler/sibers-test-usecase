using System.ComponentModel.DataAnnotations;

namespace ProjectService.Api.Contracts;

public class CreateEmployeeRequest : IValidatableObject
{
    public required string Email { get; init; }
    public required string Name { get; init; }
    public required string Surname { get; init; }
    public string? Patronymic { get; init; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!new EmailAddressAttribute().IsValid(Email))
            yield return new ValidationResult("Некорректный email", new[] { nameof(Email) });
    }
}
