using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SB.Auth.Application.Contracts;
using SB.Auth.Domain.User;

namespace SB.Auth.Application.UseCases;

public class GetUserAccountsUseCase(UserManager<ApplicationUser> userManager)
{
    public async Task<Result<IReadOnlyList<UserAccountDto>>> Handle(CancellationToken ct)
    {
        var users = await userManager.Users.AsNoTracking().ToListAsync(ct);
        var accounts = new List<UserAccountDto>(users.Count);

        foreach (var user in users)
        {
            var roles = await userManager.GetRolesAsync(user);
            accounts.Add(new UserAccountDto
            {
                EmployeeId = user.EmployeeId,
                Email = user.Email!,
                Roles = roles.ToArray()
            });
        }

        return Result.Ok<IReadOnlyList<UserAccountDto>>(accounts);
    }
}