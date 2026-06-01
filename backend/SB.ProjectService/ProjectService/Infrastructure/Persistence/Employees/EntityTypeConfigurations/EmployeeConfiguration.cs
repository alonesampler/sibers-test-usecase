using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProjectService.Domain.Employees;

namespace ProjectService.Infrastructure.Persistence.Employees.EntityTypeConfigurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);

        builder.OwnsOne(e => e.FullName, fn =>
        {
            fn.Property(f => f.Name).HasColumnName("Name").HasMaxLength(100);
            fn.Property(f => f.Surname).HasColumnName("Surname").HasMaxLength(100);
            fn.Property(f => f.Patronymic).HasColumnName("Patronymic").HasMaxLength(100);
        });

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.IsManager)
            .IsRequired();
    }
}
