using FluentResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SB.Auth.Domain.User;

namespace SB.Auth.Application.UseCases;

public sealed record SyncEmployeeEmailCommand
{
    public Guid EmployeeId { get; init; }
    public string Email { get; init; }
}

public class SyncEmployeeEmailUseCase(UserManager<ApplicationUser> userManager)
{
    public async Task<Result> Handle(SyncEmployeeEmailCommand command, CancellationToken ct)
    {
        var targetEmail = command.Email.Trim();
        var employeeUsers = await userManager.Users
            .Where(u => u.EmployeeId == command.EmployeeId)
            .ToListAsync(ct);

        if (employeeUsers.Count == 0)
            return Result.Ok();

        var withTargetEmail = employeeUsers.FirstOrDefault(u =>
            string.Equals(u.Email, targetEmail, StringComparison.OrdinalIgnoreCase));

        if (withTargetEmail is not null)
        {
            var removeDuplicates = await RemoveDuplicateAccountsAsync(
                employeeUsers.Where(u => u.Id != withTargetEmail.Id));
            return removeDuplicates;
        }

        var emailOwner = await userManager.FindByEmailAsync(targetEmail);
        if (emailOwner is not null)
        {
            if (emailOwner.EmployeeId == command.EmployeeId)
            {
                var removeOthers = await RemoveDuplicateAccountsAsync(
                    employeeUsers.Where(u => u.Id != emailOwner.Id));
                return removeOthers;
            }

            return Result.Fail(
                "Этот email уже используется для входа у другого сотрудника. " +
                "Выберите другой адрес или удалите лишнюю учётную запись в Auth.");
        }

        if (employeeUsers.Count > 1)
        {
            var removeDuplicates = await RemoveDuplicateAccountsAsync(employeeUsers.Skip(1));
            if (removeDuplicates.IsFailed)
                return removeDuplicates;
            employeeUsers = [employeeUsers[0]];
        }

        var user = employeeUsers[0];
        user.Email = targetEmail;
        user.UserName = targetEmail;
        user.NormalizedEmail = userManager.NormalizeEmail(targetEmail);
        user.NormalizedUserName = userManager.NormalizeName(targetEmail);

        var update = await userManager.UpdateAsync(user);
        return update.Succeeded
            ? Result.Ok()
            : Result.Fail(update.Errors.Select(e => e.Description));
    }

    private async Task<Result> RemoveDuplicateAccountsAsync(IEnumerable<ApplicationUser> duplicates)
    {
        foreach (var duplicate in duplicates)
        {
            var delete = await userManager.DeleteAsync(duplicate);
            if (!delete.Succeeded)
                return Result.Fail(delete.Errors.Select(e => e.Description));
        }

        return Result.Ok();
    }
}