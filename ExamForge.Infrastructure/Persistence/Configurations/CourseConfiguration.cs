using ExamForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamForge.Infrastructure.Persistence.Configurations;

public sealed class CourseConfiguration
    : IEntityTypeConfiguration<Course>
{
    public void Configure(
        EntityTypeBuilder<Course> builder)
    {
        builder.ToTable("Courses");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedNever();
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(150);
        builder.Property(c => c.Description)
            .HasMaxLength(1000);
        builder.Property(c => c.IsActive)
            .IsRequired();
        builder.Property(c => c.CreatedAt)
            .IsRequired();
    }
}