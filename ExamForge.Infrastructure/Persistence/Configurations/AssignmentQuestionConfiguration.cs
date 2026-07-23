using ExamForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ExamForge.Infrastructure.Persistence.Configurations;

public sealed class AssignmentQuestionConfiguration : IEntityTypeConfiguration<AssignmentQuestion>
{
    public void Configure(EntityTypeBuilder<AssignmentQuestion> builder)
    {
        builder.ToTable("AssignmentQuestion");
        builder.HasKey(aq => aq.Id);
        builder.Property(aq => aq.Id)
            .ValueGeneratedNever();
        builder.Property(aq => aq.AssignmentId)
            .IsRequired();
        builder.Property(aq => aq.QuestionId)
            .IsRequired();
        builder.Property(aq => aq.Order)
            .IsRequired();
        builder.Property(aq => aq.MaximumMarks)
            .IsRequired();
        builder.Property(aq => aq.CreatedAt)
            .IsRequired();

        builder.HasIndex(aq => new
        {
            aq.AssignmentId,
            aq.QuestionId
        })
        .IsUnique();

        builder.HasIndex(aq => new { aq.AssignmentId, aq.Order }).IsUnique();

        builder.HasOne<Assignment>()
            .WithMany()
            .HasForeignKey(aq => aq.AssignmentId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Question>()
            .WithMany()
            .HasForeignKey(aq => aq.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
