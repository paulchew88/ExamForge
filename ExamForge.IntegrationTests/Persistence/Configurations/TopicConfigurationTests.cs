using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public sealed class TopicConfigurationTests
{
    private readonly IEntityType _entityType;

    public TopicConfigurationTests()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;" +
                "Database=ExamForgeConfigurationTests;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;")
            .Options;

        using var context = new ExamForgeDbContext(options);

        _entityType = context.Model.FindEntityType(typeof(Topic))
            ?? throw new InvalidOperationException(
                "Topic was not found in the EF Core model.");
    }

    [Fact]
    public void Topic_ShouldMapToTopicTable()
    {
        Assert.Equal("Topic", _entityType.GetTableName());
    }

    [Fact]
    public void Topic_ShouldUseIdAsPrimaryKey()
    {
        var primaryKey = _entityType.FindPrimaryKey();

        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal(
            nameof(Topic.Id),
            primaryKey.Properties[0].Name);
    }

    [Fact]
    public void Id_ShouldNotBeGeneratedByDatabase()
    {
        var property = GetProperty(nameof(Topic.Id));

        Assert.Equal(
            ValueGenerated.Never,
            property.ValueGenerated);
    }

    [Fact]
    public void UnitId_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Topic.UnitId));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void Name_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Topic.Name));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void Name_ShouldHaveMaximumLengthOf150()
    {
        var property = GetProperty(nameof(Topic.Name));

        Assert.Equal(Topic.MaxNameLength, property.GetMaxLength());
    }

    [Fact]
    public void Description_ShouldBeOptional()
    {
        var property = GetProperty(nameof(Topic.Description));

        Assert.True(property.IsNullable);
    }

    [Fact]
    public void Description_ShouldHaveMaximumLengthOf1000()
    {
        var property = GetProperty(nameof(Topic.Description));

        Assert.Equal(Topic.MaxDescriptionLength, property.GetMaxLength());
    }

    [Fact]
    public void DisplayOrder_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Topic.DisplayOrder));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void IsActive_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Topic.IsActive));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void CreatedAt_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Topic.CreatedAt));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void Topic_ShouldHaveRelationshipWithUnit()
    {
        var foreignKey = GetUnitForeignKey();

        Assert.Equal(
            typeof(Unit),
            foreignKey.PrincipalEntityType.ClrType);

        Assert.Single(foreignKey.Properties);

        Assert.Equal(
            nameof(Topic.UnitId),
            foreignKey.Properties[0].Name);
    }

    [Fact]
    public void UnitRelationship_ShouldUseCascadeDelete()
    {
        var foreignKey = GetUnitForeignKey();

        Assert.Equal(
            DeleteBehavior.Cascade,
            foreignKey.DeleteBehavior);
    }

    [Fact]
    public void UnitIdAndDisplayOrder_ShouldHaveCompositeIndex()
    {
        var index = _entityType
            .GetIndexes()
            .SingleOrDefault(index =>
                index.Properties.Count == 2 &&
                index.Properties[0].Name ==
                    nameof(Topic.UnitId) &&
                index.Properties[1].Name ==
                    nameof(Topic.DisplayOrder));

        Assert.NotNull(index);
        Assert.False(index.IsUnique);
    }

    private IProperty GetProperty(string propertyName)
    {
        return _entityType.FindProperty(propertyName)
            ?? throw new InvalidOperationException(
                $"{propertyName} was not found in the Topic mapping.");
    }

    private IForeignKey GetUnitForeignKey()
    {
        return _entityType
            .GetForeignKeys()
            .SingleOrDefault(foreignKey =>
                foreignKey.Properties.Count == 1 &&
                foreignKey.Properties[0].Name ==
                    nameof(Topic.UnitId))
            ?? throw new InvalidOperationException(
                "The Topic to Unit foreign key was not found.");
    }
}