namespace ProjectService.Domain.Employees.Repositories;

public interface IEmployeeRepository
{
    Task AddAsync(Employee employee);
    void Update(Employee entity);
    void Delete(Employee entity);
    Task<Employee[]> GetByIdsAsync(Guid[] ids);
    Task<Employee?> GetByIdAsync(Guid id);
    Task<Employee?> GetByEmailAsync(string email);
    Task<Employee[]> GetAllAsync();
    Task<Employee[]> SearchAsync(string? q = null);
}