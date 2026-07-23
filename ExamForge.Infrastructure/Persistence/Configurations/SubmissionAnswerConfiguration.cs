using ExamForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace ExamForge.Infrastructure.Persistence.Configurations;

public sealed class SubmissionAnswerConfiguration : IEntityTypeConfiguration<SubmissionAnswer>
{
    public void Configure(EntityTypeBuilder<SubmissionAnswer> builder)
    {
        builder.ToTable("SubmissionAnswer");
        builder.HasKey(sa => sa.Id);
        builder.Property(sa => sa.Id)
            .ValueGeneratedNever();
        builder.Property(sa => sa.SubmissionId)
            .IsRequired();
        builder.Property(sa => sa.AssignmentQuestionId)
            .IsRequired();
        builder.Property(sa => sa.Answer)
            .IsRequired()
            .HasMaxLength(SubmissionAnswer.MaxAnswerLength);
        builder.Property(sa => sa.Feedback)
            .HasMaxLength(SubmissionAnswer.MaxFeedbackLength);
        builder.Property(sa => sa.LastUpdatedAt)
            .IsRequired();
        builder.HasIndex(sa => new { sa.SubmissionId, sa.AssignmentQuestionId })
            .IsUnique();
        builder.HasOne<Submission>()
            .WithMany()
            .HasForeignKey(sa => sa.SubmissionId)
            .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<AssignmentQuestion>()
            .WithMany()
            .HasForeignKey(sa => sa.AssignmentQuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
