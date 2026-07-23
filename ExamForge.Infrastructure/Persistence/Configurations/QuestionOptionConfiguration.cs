using ExamForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamForge.Infrastructure.Persistence.Configurations;

public sealed class QuestionOptionConfiguration : IEntityTypeConfiguration<QuestionOption>
{
    public void Configure(EntityTypeBuilder<QuestionOption> builder)
    {
        builder.ToTable("QuestionOption");
        builder.HasKey(qo => qo.Id);
        builder.Property(qo => qo.Id)
            .ValueGeneratedNever();
        builder.Property(qo => qo.QuestionId)
            .IsRequired();
        builder.Property(qo => qo.Text)
            .IsRequired()
            .HasMaxLength(QuestionOption.MaxTextLength);
        builder.Property(qo => qo.IsCorrect)
            .IsRequired();
        builder.Property(qo => qo.DisplayOrder)
            .IsRequired();
        builder.Property(qo => qo.CreatedAt)
            .IsRequired();
        builder.HasIndex(qo => new { qo.QuestionId, qo.DisplayOrder })
            .IsUnique();
        builder.HasOne<Question>()
            .WithMany()
            .HasForeignKey(qo => qo.QuestionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
