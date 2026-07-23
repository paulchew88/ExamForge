using ExamForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamForge.Infrastructure.Persistence.Configurations;

public sealed class QuestionAssetConfiguration : IEntityTypeConfiguration<QuestionAsset>
{
    public void Configure(EntityTypeBuilder<QuestionAsset> builder)
    {
        builder.ToTable("QuestionAssets");
        builder.HasKey(qa => qa.Id);
        builder.Property(qa => qa.Id)
            .ValueGeneratedNever();
        builder.Property(qa => qa.QuestionId)
            .IsRequired();
        builder.Property(qa => qa.OriginalFileName)
            .HasMaxLength(QuestionAsset.MaxFileNameLength)
            .IsRequired();
        builder.Property(qa => qa.StorageKey)
            .HasMaxLength(QuestionAsset.MaxStorageKeyLength)
            .IsRequired();
        builder.Property(qa => qa.ContentType)
            .HasMaxLength(QuestionAsset.MaxContentTypeLength)
            .IsRequired();
        builder.Property(qa => qa.AssetType)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(qa => qa.SizeBytes)
            .IsRequired();
        builder.Property(qa => qa.Caption)
            .HasMaxLength(QuestionAsset.MaxCaptionLength)
            .IsRequired(false);
        builder.Property(qa => qa.AltText)
            .HasMaxLength(QuestionAsset.MaxAltTextLength)
            .IsRequired(false);
        builder.Property(qa => qa.DisplayOrder)
            .IsRequired();
        builder.Property(qa => qa.CreatedAt)
            .IsRequired();
        builder.HasOne<Question>()
            .WithMany()
            .HasForeignKey(qa => qa.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(qa => qa.QuestionId);
        builder.HasIndex(qa => new
        {
            qa.QuestionId,
            qa.DisplayOrder
        });
    }
}
