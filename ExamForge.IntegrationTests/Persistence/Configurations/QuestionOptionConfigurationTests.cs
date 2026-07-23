using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public sealed class QuestionOptionConfigurationTests
{
    private readonly IEntityType _entityType;

    public QuestionOptionConfigurationTests()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=localhost;Database=ExamForgeTests;" +
                "Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new ExamForgeDbContext(options);

        _entityType = context.Model.FindEntityType(typeof(QuestionOption))
            ?? throw new InvalidOperationException(
                "QuestionOption was not found in the EF Core model.");
    }

    [Fact]
    public void QuestionOption_ShouldMapToQuestionOptionTable()
    {
        Assert.Equal(
            "QuestionOption",
            _entityType.GetTableName());
    }

    [Fact]
    public void QuestionOption_ShouldUseIdAsPrimaryKey()
    {
        var primaryKey = _entityType.FindPrimaryKey();

        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);

        Assert.Equal(
            nameof(QuestionOption.Id),
            primaryKey.Properties[0].Name);
    }

    [Fact]
    public void Id_ShouldNotBeGeneratedByDatabase()
    {
        var property = GetProperty(nameof(QuestionOption.Id));

        Assert.Equal(
            ValueGenerated.Never,
            property.ValueGenerated);
    }

    [Fact]
    public void QuestionId_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(QuestionOption.QuestionId))
                .IsNullable);
    }

    [Fact]
    public void Text_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(QuestionOption.Text))
                .IsNullable);
    }

    [Fact]
    public void Text_ShouldHaveCorrectMaximumLength()
    {
        var property = GetProperty(nameof(QuestionOption.Text));

        Assert.Equal(
            QuestionOption.MaxTextLength,
            property.GetMaxLength());
    }

    [Fact]
    public void IsCorrect_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(QuestionOption.IsCorrect))
                .IsNullable);
    }

    [Fact]
    public void DisplayOrder_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(QuestionOption.DisplayOrder))
                .IsNullable);
    }

    [Fact]
    public void CreatedAt_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(QuestionOption.CreatedAt))
                .IsNullable);
    }

    [Fact]
    public void QuestionOption_ShouldHaveUniqueIndexOnQuestionIdAndDisplayOrder()
    {
        var index = _entityType.GetIndexes()
            .Single(i =>
                i.Properties.Count == 2 &&
                i.Properties[0].Name ==
                    nameof(QuestionOption.QuestionId) &&
                i.Properties[1].Name ==
                    nameof(QuestionOption.DisplayOrder));

        Assert.True(index.IsUnique);
    }

    [Fact]
    public void QuestionOption_ShouldHaveRequiredRelationshipToQuestion()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk =>
                fk.Properties.Single().Name ==
                nameof(QuestionOption.QuestionId));

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
                $"{propertyName} was not found in the QuestionOption mapping.");
    }
}