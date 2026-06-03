using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.Domain.Projects;

namespace ProjectService.Infrastructure.Persistence.Projects.EntityTypeConfigurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("Projects");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.CustomerCompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.ExecutorCompanyName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.StartDate).IsRequired();
        builder.Property(p => p.EndDate).IsRequired();
        builder.Property(p => p.Priority).IsRequired();

        builder.HasOne(p => p.Manager)
            .WithMany()
            .HasForeignKey(p => p.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Employees)
            .WithMany()
            .UsingEntity("ProjectEmployees");
    }
}
