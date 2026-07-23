using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public sealed class SubmissionConfigurationTests
{
    private readonly IEntityType _entityType;

    public SubmissionConfigurationTests()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=localhost;Database=ExamForgeTests;" +
                "Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new ExamForgeDbContext(options);

        _entityType = context.Model.FindEntityType(typeof(Submission))
            ?? throw new InvalidOperationException(
                "Submission was not found in the EF Core model.");
    }

    [Fact]
    public void Submission_ShouldMapToSubmissionTable()
    {
        Assert.Equal(
            "Submission",
            _entityType.GetTableName());
    }

    [Fact]
    public void Submission_ShouldUseIdAsPrimaryKey()
    {
        var primaryKey = _entityType.FindPrimaryKey();

        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal(
            nameof(Submission.Id),
            primaryKey.Properties[0].Name);
    }

    [Fact]
    public void Id_ShouldNotBeGeneratedByDatabase()
    {
        var property = GetProperty(nameof(Submission.Id));

        Assert.Equal(
            ValueGenerated.Never,
            property.ValueGenerated);
    }

    [Fact]
    public void AssignmentId_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(Submission.AssignmentId))
                .IsNullable);
    }

    [Fact]
    public void StudentId_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(Submission.StudentId))
                .IsNullable);
    }

    [Fact]
    public void Status_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(Submission.Status))
                .IsNullable);
    }

    [Fact]
    public void SubmittedAt_ShouldBeOptional()
    {
        Assert.True(
            GetProperty(nameof(Submission.SubmittedAt))
                .IsNullable);
    }

    [Fact]
    public void MarkedAt_ShouldBeOptional()
    {
        Assert.True(
            GetProperty(nameof(Submission.MarkedAt))
                .IsNullable);
    }

    [Fact]
    public void ReleasedAt_ShouldBeOptional()
    {
        Assert.True(
            GetProperty(nameof(Submission.ReleasedAt))
                .IsNullable);
    }

    [Fact]
    public void CreatedAt_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(Submission.CreatedAt))
                .IsNullable);
    }

    [Fact]
    public void Submission_ShouldHaveIndexOnAssignmentId()
    {
        var index = _entityType.GetIndexes()
            .Single(i =>
                i.Properties.Count == 1 &&
                i.Properties[0].Name ==
                nameof(Submission.AssignmentId));

        Assert.False(index.IsUnique);
    }

    [Fact]
    public void Submission_ShouldHaveIndexOnStudentId()
    {
        var index = _entityType.GetIndexes()
            .Single(i =>
                i.Properties.Count == 1 &&
                i.Properties[0].Name ==
                nameof(Submission.StudentId));

        Assert.False(index.IsUnique);
    }

    [Fact]
    public void Submission_ShouldHaveUniqueIndexOnAssignmentIdAndStudentId()
    {
        var index = _entityType.GetIndexes()
            .Single(i =>
                i.Properties.Count == 2 &&
                i.Properties[0].Name ==
                    nameof(Submission.AssignmentId) &&
                i.Properties[1].Name ==
                    nameof(Submission.StudentId));

        Assert.True(index.IsUnique);
    }

    [Fact]
    public void Submission_ShouldHaveRequiredRelationshipToAssignment()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk =>
                fk.Properties.Single().Name ==
                nameof(Submission.AssignmentId));

        Assert.Equal(
            nameof(Assignment),
            foreignKey.PrincipalEntityType.ClrType.Name);

        Assert.Equal(
            DeleteBehavior.Restrict,
            foreignKey.DeleteBehavior);

        Assert.True(foreignKey.IsRequired);
    }

    [Fact]
    public void Submission_ShouldHaveRequiredRelationshipToStudent()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk =>
                fk.Properties.Single().Name ==
                nameof(Submission.StudentId));

        Assert.Equal(
            nameof(User),
            foreignKey.PrincipalEntityType.ClrType.Name);

        Assert.Equal(
            DeleteBehavior.Restrict,
            foreignKey.DeleteBehavior);

        Assert.True(foreignKey.IsRequired);
    }

    private IProperty GetProperty(string propertyName)
    {
        return _entityType.FindProperty(propertyName)
            ?? throw new InvalidOperationException(
                $"{propertyName} was not found in the Submission mapping.");
    }
}