using ExamForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamForge.Infrastructure.Persistence.Configurations;

internal class ClassroomStudentConfiguration : IEntityTypeConfiguration<ClassroomStudent>
{
    public void Configure(EntityTypeBuilder<ClassroomStudent> builder)
    {
        builder.ToTable("ClassroomStudent");
        builder.HasKey(cs => cs.Id);
        builder.Property(cs => cs.Id)
            .ValueGeneratedNever();
        builder.Property(cs => cs.ClassroomId)
            .IsRequired();
        builder.Property(cs => cs.StudentId)
            .IsRequired();
        builder.Property(cs => cs.JoinedAt)
            .IsRequired();
        builder.Property(cs => cs.IsActive)
            .IsRequired();
        builder.Property(cs => cs.CreatedAt)
            .IsRequired();

        builder.HasIndex(cs => new { cs.ClassroomId, cs.StudentId })
            .IsUnique();

        builder.HasOne<Classroom>()
            .WithMany()
            .HasForeignKey(cs => cs.ClassroomId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(cs => cs.StudentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
