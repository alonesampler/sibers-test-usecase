using ProjectService.Application.UseCases.Employees.DTOs;
using ProjectService.Domain.Employees;

namespace ProjectService.Application.Mapping;

public static class EmployeeExtensionMapping
{
    public static EmployeeDto ToResponseDto(this Employee employee)
        => new()
        {
            Id =employee.Id,
            Email = employee.Email,
            Name = employee.FullName.Name,
            Surname = employee.FullName.Surname,
            Patronymic =employee.FullName.Patronymic
        };

    public static IEnumerable<EmployeeDto> ToResponseDtos(this IEnumerable<Employee> employees)
        => employees.Select(ToResponseDto);
}
