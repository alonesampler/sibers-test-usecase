using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SB.Auth.Domain.User;

namespace SB.Auth.Application.UseCases;

public record RegisterCommand
{
    public Guid EmployeeId { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }
    public string Role { get; init; }
}

public class RegisterUseCase(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
{
    public async Task<Result> Handle(RegisterCommand command, CancellationToken ct)
    {
        var existing = await userManager.FindByEmailAsync(command.Email);
        if (existing is not null)
            return Result.Fail("User with this email already exists");

        var linkedEmployee = await userManager.Users
            .AnyAsync(u => u.EmployeeId == command.EmployeeId, ct);
        if (linkedEmployee)
            return Result.Fail("This employee already has a login account");

        if (!AppRoles.All.Contains(command.Role))
            return Result.Fail($"Invalid role. Valid roles: {string.Join(", ", AppRoles.All)}");

        var user = new ApplicationUser
        {
            Id = Guid.CreateVersion7(),
            Email = command.Email,
            UserName = command.Email,
            EmployeeId = command.EmployeeId
        };

        var result = await userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
            return Result.Fail(result.Errors.Select(e => new Error(e.Description)));

        if (!await roleManager.RoleExistsAsync(command.Role))
            await roleManager.CreateAsync(new IdentityRole<Guid>(command.Role));

        await userManager.AddToRoleAsync(user, command.Role);
        return Result.Ok();
    }
}
