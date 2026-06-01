using FluentResults;
using ProjectService.Application.UseCases.Employees.DTOs;

namespace ProjectService.Application.Dependencies.EmployeesServices;

public interface IGettingEmployeesService
{
    public Task<Result<EmployeeDto>> GetByIdAsync(Guid id);

    public Task<Result<IEnumerable<EmployeeDto>>> GetAllAsync();

    public Task<Result<IEnumerable<EmployeeDto>>> SearchAsync(string? query);
}
