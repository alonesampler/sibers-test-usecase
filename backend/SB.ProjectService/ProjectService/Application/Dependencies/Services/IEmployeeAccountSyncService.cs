using FluentResults;

namespace ProjectService.Application.Dependencies.Services;

public interface IEmployeeAccountSyncService
{
    Task<Result> SyncEmailAsync(Guid employeeId, string email, CancellationToken ct = default);
}