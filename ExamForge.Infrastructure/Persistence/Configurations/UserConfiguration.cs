using ExamForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ExamForge.Infrastructure.Persistence.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedNever();
        builder.Property(u => u.ExternalIdentityId)
            .IsRequired();
        builder.HasIndex(u => u.ExternalIdentityId)
            .IsUnique();
        builder.Property(u => u.FirstName)
            .HasMaxLength(User.MaxFirstNameLength)
            .IsRequired();
        builder.Property(u => u.LastName)
            .HasMaxLength(User.MaxLastNameLength)
            .IsRequired();
        builder.Property(u => u.Email)
            .HasMaxLength(User.MaxEmailLength)
            .IsRequired();
        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(u => u.Status)
            .HasConversion<string>()
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(u => u.IsActive)
            .IsRequired();
        builder.Property(u => u.CreatedAt)
            .IsRequired();
        builder.HasIndex(u => u.Email)
            .IsUnique();
    }
}
