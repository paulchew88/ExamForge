using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public sealed class SubmissionAnswerConfigurationTests
{
    private readonly IEntityType _entityType;

    public SubmissionAnswerConfigurationTests()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=localhost;Database=ExamForgeTests;" +
                "Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new ExamForgeDbContext(options);

        _entityType = context.Model.FindEntityType(typeof(SubmissionAnswer))
            ?? throw new InvalidOperationException(
                "SubmissionAnswer was not found in the EF Core model.");
    }

    [Fact]
    public void SubmissionAnswer_ShouldMapToSubmissionAnswerTable()
    {
        Assert.Equal(
            "SubmissionAnswer",
            _entityType.GetTableName());
    }

    [Fact]
    public void SubmissionAnswer_ShouldUseIdAsPrimaryKey()
    {
        var primaryKey = _entityType.FindPrimaryKey();

        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal(
            nameof(SubmissionAnswer.Id),
            primaryKey.Properties[0].Name);
    }

    [Fact]
    public void Id_ShouldNotBeGeneratedByDatabase()
    {
        var property = GetProperty(nameof(SubmissionAnswer.Id));

        Assert.Equal(
            ValueGenerated.Never,
            property.ValueGenerated);
    }

    [Fact]
    public void SubmissionId_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(SubmissionAnswer.SubmissionId))
                .IsNullable);
    }

    [Fact]
    public void AssignmentQuestionId_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(SubmissionAnswer.AssignmentQuestionId))
                .IsNullable);
    }

    [Fact]
    public void Answer_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(SubmissionAnswer.Answer))
                .IsNullable);
    }

    [Fact]
    public void Answer_ShouldHaveCorrectMaxLength()
    {
        Assert.Equal(
            SubmissionAnswer.MaxAnswerLength,
            GetProperty(nameof(SubmissionAnswer.Answer))
                .GetMaxLength());
    }

    [Fact]
    public void AwardedMarks_ShouldBeOptional()
    {
        Assert.True(
            GetProperty(nameof(SubmissionAnswer.AwardedMarks))
                .IsNullable);
    }

    [Fact]
    public void Feedback_ShouldBeOptional()
    {
        Assert.True(
            GetProperty(nameof(SubmissionAnswer.Feedback))
                .IsNullable);
    }

    [Fact]
    public void Feedback_ShouldHaveCorrectMaxLength()
    {
        Assert.Equal(
            SubmissionAnswer.MaxFeedbackLength,
            GetProperty(nameof(SubmissionAnswer.Feedback))
                .GetMaxLength());
    }

    [Fact]
    public void LastUpdatedAt_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(SubmissionAnswer.LastUpdatedAt))
                .IsNullable);
    }

    [Fact]
    public void MarkedAt_ShouldBeOptional()
    {
        Assert.True(
            GetProperty(nameof(SubmissionAnswer.MarkedAt))
                .IsNullable);
    }

    [Fact]
    public void CreatedAt_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(SubmissionAnswer.CreatedAt))
                .IsNullable);
    }

    [Fact]
    public void SubmissionAnswer_ShouldHaveUniqueIndexOnSubmissionIdAndAssignmentQuestionId()
    {
        var index = _entityType.GetIndexes()
            .Single(i =>
                i.Properties.Count == 2 &&
                i.Properties[0].Name ==
                    nameof(SubmissionAnswer.SubmissionId) &&
                i.Properties[1].Name ==
                    nameof(SubmissionAnswer.AssignmentQuestionId));

        Assert.True(index.IsUnique);
    }

    [Fact]
    public void SubmissionAnswer_ShouldHaveRequiredRelationshipToSubmission()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk =>
                fk.Properties.Single().Name ==
                nameof(SubmissionAnswer.SubmissionId));

        Assert.Equal(
            nameof(Submission),
            foreignKey.PrincipalEntityType.ClrType.Name);

        Assert.Equal(
            DeleteBehavior.Restrict,
            foreignKey.DeleteBehavior);

        Assert.True(foreignKey.IsRequired);
    }

    [Fact]
    public void SubmissionAnswer_ShouldHaveRequiredRelationshipToAssignmentQuestion()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk =>
                fk.Properties.Single().Name ==
                nameof(SubmissionAnswer.AssignmentQuestionId));

        Assert.Equal(
            nameof(AssignmentQuestion),
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
                $"{propertyName} was not found in the SubmissionAnswer mapping.");
    }
}