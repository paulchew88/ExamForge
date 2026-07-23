using ExamForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamForge.Infrastructure.Persistence.Configurations;

public sealed class UnitConfiguration : IEntityTypeConfiguration<Unit>
{
    public void Configure(EntityTypeBuilder<Unit> builder)
    {
        builder.ToTable("Unit");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedNever();
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(Unit.MaxNameLength);
        builder.Property(u => u.Description)
            .HasMaxLength(Unit.MaxDescriptionLength);
        builder.Property(u => u.IsActive)
            .IsRequired();
        builder.Property(u => u.CreatedAt)
            .IsRequired();
        builder.HasOne<Course>()
            .WithMany()
            .HasForeignKey(u => u.CourseId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasIndex(u => new { u.CourseId, u.DisplayOrder })
            .IsUnique(false);

    }
}
