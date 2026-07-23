using ExamForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ExamForge.Infrastructure.Persistence.Configurations;

public sealed class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
{
    public void Configure(EntityTypeBuilder<Assignment> builder)
    {
        builder.ToTable("Assignment");
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .ValueGeneratedNever();
        builder.Property(a => a.ClassroomId)
            .IsRequired();
        builder.Property(a => a.Title)
            .IsRequired()
            .HasMaxLength(Assignment.MaxTitleLength);
        builder.Property(a => a.Instructions)
            .IsRequired()
            .HasMaxLength(Assignment.MaxInstructionsLength);
        builder.Property(a => a.OpensAt)
            .IsRequired();
        builder.Property(a => a.DueAt)
            .IsRequired();
        builder.Property(a => a.IsPublished)
            .IsRequired();
        builder.Property(a => a.CreatedAt)
            .IsRequired();
        builder.HasIndex(a => a.ClassroomId);

        builder.HasOne<Classroom>()
            .WithMany()
            .HasForeignKey(a => a.ClassroomId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}
