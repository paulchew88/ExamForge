using ExamForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamForge.Infrastructure.Persistence.Configurations;

public sealed class AssignmentClassroomConfiguration : IEntityTypeConfiguration<AssignmentClassroom>
{
    public void Configure(EntityTypeBuilder<AssignmentClassroom> builder)
    {
        builder.ToTable("AssignmentClassroom");
        builder.HasKey(ac => ac.Id);
        builder.Property(ac => ac.Id)
            .ValueGeneratedNever();
        builder.Property(ac => ac.AssignmentId)
            .IsRequired();
        builder.Property(ac => ac.ClassroomId)
            .IsRequired();
        builder.Property(ac => ac.AssignedAt)
            .IsRequired();
        builder.Property(ac => ac.IsActive)
            .IsRequired();
        builder.Property(ac => ac.CreatedAt)
            .IsRequired();
        builder.Property(ac => ac.UnassignedAt)
            .IsRequired(false);

        builder.HasIndex(ac => new { ac.AssignmentId, ac.ClassroomId })
            .IsUnique();
        builder.HasOne<Assignment>()
            .WithMany()
            .HasForeignKey(ac => ac.AssignmentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Classroom>()
            .WithMany()
            .HasForeignKey(ac => ac.ClassroomId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
