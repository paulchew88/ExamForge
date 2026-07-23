using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public sealed class AssignmentQuestionConfigurationTests
{
    private readonly IEntityType _entityType;

    public AssignmentQuestionConfigurationTests()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=localhost;Database=ExamForgeTests;" +
                "Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new ExamForgeDbContext(options);

        _entityType = context.Model.FindEntityType(typeof(AssignmentQuestion))
            ?? throw new InvalidOperationException(
                "AssignmentQuestion was not found in the EF Core model.");
    }

    [Fact]
    public void AssignmentQuestion_ShouldMapToAssignmentQuestionTable()
    {
        Assert.Equal(
            "AssignmentQuestion",
            _entityType.GetTableName());
    }

    [Fact]
    public void AssignmentQuestion_ShouldUseIdAsPrimaryKey()
    {
        var primaryKey = _entityType.FindPrimaryKey();

        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal(
            nameof(AssignmentQuestion.Id),
            primaryKey.Properties[0].Name);
    }

    [Fact]
    public void Id_ShouldNotBeGeneratedByDatabase()
    {
        var property = GetProperty(nameof(AssignmentQuestion.Id));

        Assert.Equal(
            ValueGenerated.Never,
            property.ValueGenerated);
    }

    [Fact]
    public void AssignmentId_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(AssignmentQuestion.AssignmentId))
                .IsNullable);
    }

    [Fact]
    public void QuestionId_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(AssignmentQuestion.QuestionId))
                .IsNullable);
    }

    [Fact]
    public void Order_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(AssignmentQuestion.Order))
                .IsNullable);
    }

    [Fact]
    public void MaximumMarks_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(AssignmentQuestion.MaximumMarks))
                .IsNullable);
    }

    [Fact]
    public void CreatedAt_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(AssignmentQuestion.CreatedAt))
                .IsNullable);
    }

    [Fact]
    public void AssignmentQuestion_ShouldHaveUniqueIndexOnAssignmentIdAndOrder()
    {
        var index = _entityType.GetIndexes()
            .Single(i =>
                i.Properties.Count == 2 &&
                i.Properties[0].Name ==
                    nameof(AssignmentQuestion.AssignmentId) &&
                i.Properties[1].Name ==
                    nameof(AssignmentQuestion.Order));

        Assert.True(index.IsUnique);
    }

    [Fact]
    public void AssignmentQuestion_ShouldHaveUniqueIndexOnAssignmentIdAndQuestionId()
    {
        var index = _entityType.GetIndexes()
            .Single(i =>
                i.Properties.Count == 2 &&
                i.Properties[0].Name ==
                    nameof(AssignmentQuestion.AssignmentId) &&
                i.Properties[1].Name ==
                    nameof(AssignmentQuestion.QuestionId));

        Assert.True(index.IsUnique);
    }

    [Fact]
    public void AssignmentQuestion_ShouldHaveRequiredRelationshipToAssignment()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk =>
                fk.Properties.Single().Name ==
                nameof(AssignmentQuestion.AssignmentId));

        Assert.Equal(
            nameof(Assignment),
            foreignKey.PrincipalEntityType.ClrType.Name);

        Assert.Equal(
            DeleteBehavior.Restrict,
            foreignKey.DeleteBehavior);

        Assert.True(foreignKey.IsRequired);
    }

    [Fact]
    public void AssignmentQuestion_ShouldHaveRequiredRelationshipToQuestion()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk =>
                fk.Properties.Single().Name ==
                nameof(AssignmentQuestion.QuestionId));

        Assert.Equal(
            nameof(Question),
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
                $"{propertyName} was not found in the AssignmentQuestion mapping.");
    }
}