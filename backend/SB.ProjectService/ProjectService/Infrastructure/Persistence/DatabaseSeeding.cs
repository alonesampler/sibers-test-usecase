using ProjectService.Application.Dependencies.UnitOfWork;
using ProjectService.Domain.Employees;

namespace ProjectService.Infrastructure.Persistence;

public static class DatabaseSeeding
{
    extension(IServiceProvider serviceProvider)
    {
        public async Task SeedAsync()
        {
            using var scope = serviceProvider.CreateScope();
            var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

            var existing = await uow.EmployeeRepository.GetByEmailAsync(SeedData.AdminEmail);
            if (existing is not null) return;

            var employee = Employee.CreateWithId(
                SeedData.AdminEmployeeId,
                new FullName("Admin", "System", null),
                SeedData.AdminEmail).Value;

            await uow.EmployeeRepository.AddAsync(employee);
            await uow.SaveAsync();
        }
    }
}