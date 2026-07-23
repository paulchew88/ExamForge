using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public sealed class QuestionConfigurationTests
{
    private readonly IEntityType _entityType;

    public QuestionConfigurationTests()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;" +
                "Database=ExamForgeConfigurationTests;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;")
            .Options;

        using var context = new ExamForgeDbContext(options);

        _entityType = context.Model.FindEntityType(typeof(Question))
            ?? throw new InvalidOperationException(
                "Question was not found in the EF Core model.");
    }

    [Fact]
    public void Question_ShouldMapToQuestionTable()
    {
        Assert.Equal("Question", _entityType.GetTableName());
    }

    [Fact]
    public void Question_ShouldUseIdAsPrimaryKey()
    {
        var primaryKey = _entityType.FindPrimaryKey();

        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);

        Assert.Equal(
            nameof(Question.Id),
            primaryKey.Properties[0].Name);
    }

    [Fact]
    public void Id_ShouldNotBeGeneratedByDatabase()
    {
        var property = GetProperty(nameof(Question.Id));

        Assert.Equal(
            ValueGenerated.Never,
            property.ValueGenerated);
    }

    [Fact]
    public void TopicId_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Question.TopicId));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void Prompt_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Question.Prompt));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void Prompt_ShouldHaveCorrectMaximumLength()
    {
        var property = GetProperty(nameof(Question.Prompt));

        Assert.Equal(
            Question.MaxPromptLength,
            property.GetMaxLength());
    }

    [Fact]
    public void MarkScheme_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Question.MarkScheme));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void MaximumMarks_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Question.MaximumMarks));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void DisplayOrder_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Question.DisplayOrder));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void IsActive_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Question.IsActive));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void CreatedAt_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Question.CreatedAt));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void Question_ShouldHaveRelationshipWithTopic()
    {
        var foreignKey = GetTopicForeignKey();

        Assert.Equal(
            typeof(Topic),
            foreignKey.PrincipalEntityType.ClrType);

        Assert.Single(foreignKey.Properties);

        Assert.Equal(
            nameof(Question.TopicId),
            foreignKey.Properties[0].Name);
    }

    [Fact]
    public void TopicRelationship_ShouldUseCascadeDelete()
    {
        var foreignKey = GetTopicForeignKey();

        Assert.Equal(
            DeleteBehavior.Cascade,
            foreignKey.DeleteBehavior);
    }

    [Fact]
    public void TopicIdAndDisplayOrder_ShouldHaveCompositeIndex()
    {
        var index = _entityType
            .GetIndexes()
            .SingleOrDefault(index =>
                index.Properties.Count == 2 &&
                index.Properties[0].Name ==
                    nameof(Question.TopicId) &&
                index.Properties[1].Name ==
                    nameof(Question.DisplayOrder));

        Assert.NotNull(index);
        Assert.False(index.IsUnique);
    }

    private IProperty GetProperty(string propertyName)
    {
        return _entityType.FindProperty(propertyName)
            ?? throw new InvalidOperationException(
                $"{propertyName} was not found in the Question mapping.");
    }

    private IForeignKey GetTopicForeignKey()
    {
        return _entityType
            .GetForeignKeys()
            .SingleOrDefault(foreignKey =>
                foreignKey.Properties.Count == 1 &&
                foreignKey.Properties[0].Name ==
                    nameof(Question.TopicId))
            ?? throw new InvalidOperationException(
                "The Question to Topic foreign key was not found.");
    }
}