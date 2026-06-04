using Microsoft.EntityFrameworkCore;
using ProjectService.Domain.Documents;
using ProjectService.Domain.Employees;
using ProjectService.Domain.Projects;
using ProjectService.Infrastructure.Persistence.Documents.EntityTypeConfigurations;
using ProjectService.Infrastructure.Persistence.Employees.EntityTypeConfigurations;
using ProjectService.Infrastructure.Persistence.Projects.EntityTypeConfigurations;

namespace ProjectService.Infrastructure.Persistence;

public class DatabaseContext : DbContext
{
    private readonly string? _migrationStage;

    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options)
    {
    }

    internal DatabaseContext(DbContextOptions<DatabaseContext> options, string migrationStage)
        : base(options)
    {
        _migrationStage = migrationStage;
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Document> Documents { get; set; }
    public DbSet<ProjectTask> ProjectTasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (_migrationStage is null)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
            base.OnModelCreating(modelBuilder);
            return;
        }

        switch (_migrationStage)
        {
            case "employees":
                modelBuilder.Ignore<Project>();
                modelBuilder.Ignore<Document>();
                modelBuilder.Ignore<ProjectTask>();
                modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
                break;
            case "projects":
                modelBuilder.Ignore<Document>();
                modelBuilder.Ignore<ProjectTask>();
                modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
                modelBuilder.ApplyConfiguration(new ProjectConfiguration());
                break;
            case "documents":
                modelBuilder.Ignore<ProjectTask>();
                modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
                modelBuilder.ApplyConfiguration(new ProjectConfiguration());
                modelBuilder.ApplyConfiguration(new DocumentConfiguration());
                break;
            case "tasks":
                modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
                modelBuilder.ApplyConfiguration(new ProjectConfiguration());
                modelBuilder.ApplyConfiguration(new DocumentConfiguration());
                modelBuilder.ApplyConfiguration(new ProjectTaskConfiguration());
                break;
            default:
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
                break;
        }

        base.OnModelCreating(modelBuilder);
    }
}