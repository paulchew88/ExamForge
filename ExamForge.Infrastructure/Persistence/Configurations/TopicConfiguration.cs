using ExamForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExamForge.Infrastructure.Persistence.Configurations;

public sealed class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        builder.ToTable("Topic");
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id)
            .ValueGeneratedNever();
        builder.Property(t => t.UnitId)
            .IsRequired();
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(Topic.MaxNameLength);
        builder.Property(t => t.Description)
            .HasMaxLength(Topic.MaxDescriptionLength);
        builder.Property(t => t.DisplayOrder)
            .IsRequired();
        builder.Property(t => t.IsActive)
            .IsRequired();
        builder.Property(t => t.CreatedAt)
            .IsRequired();
        builder.HasOne<Unit>()
            .WithMany()
            .HasForeignKey(t => t.UnitId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasIndex(t => new { t.UnitId, t.DisplayOrder })
            .IsUnique(false);
    }
}
