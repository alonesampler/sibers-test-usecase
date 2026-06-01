namespace ProjectService.Domain.Employees.Repositories;

public interface IEmployeeRepository
{
    Task AddAsync(Employee employee);
    void UpdateAsync(Employee entity);
    void DeleteAsync(Employee entity);
    Task<Employee?> GetByIdAsync(Guid id);
    Task<Employee?> GetByEmailAsync(string email);
    Task<Employee[]> GetAllAsync();
    Task<Employee[]> SearchAsync(string? q = null);
}