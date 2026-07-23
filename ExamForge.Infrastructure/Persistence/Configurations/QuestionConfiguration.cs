using ExamForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamForge.Infrastructure.Persistence.Configurations;

public sealed class QuestionConfiguration : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Question");
        builder.HasKey(q => q.Id);
        builder.Property(q => q.Id)
            .ValueGeneratedNever();
        builder.Property(q => q.TopicId)
            .IsRequired();
        builder.Property(q => q.Prompt)
            .IsRequired()
            .HasMaxLength(Question.MaxPromptLength);
        builder.Property(q => q.MarkScheme)
            .IsRequired();
        builder.Property(q => q.DisplayOrder)
            .IsRequired();
        builder.Property(q => q.IsActive)
            .IsRequired();
        builder.Property(q => q.CreatedAt)
            .IsRequired();
        builder.HasOne<Topic>()
            .WithMany()
            .HasForeignKey(q => q.TopicId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(q => new { q.TopicId, q.DisplayOrder })
            .IsUnique(false);
    }
}
