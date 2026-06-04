using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectService.Infrastructure.Persistence;

/// <summary>
/// Поэтапные миграции: MIGRATION_STAGE=employees|projects|documents|tasks
/// </summary>
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        var stage = Environment.GetEnvironmentVariable("MIGRATION_STAGE") ?? "all";

        var connectionString =
            "Server=localhost,1433;Database=siber-project;User Id=sa;Password=my-Se1c%ret-pw;TrustServerCertificate=True;Encrypt=True;";

        var options = new DbContextOptionsBuilder<DatabaseContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new DatabaseContext(options, stage);
    }
}