using FluentResults;
using ProjectService.Application.Dependencies.EmployeesServices;
using ProjectService.Application.Mapping;
using ProjectService.Application.UseCases.Employees.DTOs;
using ProjectService.Domain.Employees;
using ProjectService.Domain.Employees.Repositories;

namespace ProjectService.Application.UseCases.Employees.Getting;

public class GettingEmployeesService(IEmployeeRepository employeeRepository) : IGettingEmployeesService
{
    public async Task<Result<EmployeeDto>> GetByIdAsync(Guid id)
    {
        var employee = await employeeRepository.GetByIdAsync(id);

        if (employee is null)
            return Result.Fail(EmployeeError.NotFound);

        return Result.Ok(employee.ToResponseDto());
    }

    public async Task<Result<IEnumerable<EmployeeDto>>> GetAllAsync()
    {
        var employees = await employeeRepository.GetAllAsync();
        return Result.Ok(employees.ToResponseDtos());
    }

    public async Task<Result<IEnumerable<EmployeeDto>>> SearchAsync(string? query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await GetAllAsync();

        var employees = await employeeRepository.SearchAsync(query);
        return Result.Ok(employees.ToResponseDtos());
    }
}
