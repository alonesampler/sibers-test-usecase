using Microsoft.AspNetCore.Identity;
using SB.Auth.Domain.User;
using SB.Auth.Infrastructure.Persistence;

namespace SB.Auth.Infrastructire.Persistence;

public static class DatabaseSeeding
{
    extension(IServiceProvider serviceProvider)
    {
        public async Task SeedAsync()
        {
            using var scope = serviceProvider.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            if (await userManager.FindByEmailAsync(SeedData.AdminEmail) is not null)
                return;

            foreach (var role in AppRoles.All)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }

            var user = new ApplicationUser
            {
                Id = Guid.CreateVersion7(),
                Email = SeedData.AdminEmail,
                UserName = SeedData.AdminEmail,
                EmployeeId = SeedData.AdminEmployeeId
            };

            var result = await userManager.CreateAsync(user, SeedData.AdminPassword);
            if (!result.Succeeded)
            {
                var errors = string.Join("; ", result.Errors.Select(e => e.Description));
                throw new InvalidOperationException($"Failed to seed admin user: {errors}");
            }

            await userManager.AddToRoleAsync(user, AppRoles.Director);
        }
    }
}