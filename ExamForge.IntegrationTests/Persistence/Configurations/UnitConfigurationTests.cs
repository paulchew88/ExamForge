using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public sealed class UnitConfigurationTests
{
    private readonly IEntityType _entityType;

    public UnitConfigurationTests()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;" +
                "Database=ExamForgeConfigurationTests;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;")
            .Options;

        using var context = new ExamForgeDbContext(options);

        _entityType = context.Model.FindEntityType(typeof(Unit))
            ?? throw new InvalidOperationException(
                "Unit was not found in the EF Core model.");
    }

    [Fact]
    public void Unit_ShouldMapToUnitTable()
    {
        Assert.Equal("Unit", _entityType.GetTableName());
    }

    [Fact]
    public void Unit_ShouldUseIdAsPrimaryKey()
    {
        var primaryKey = _entityType.FindPrimaryKey();

        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal(
            nameof(Unit.Id),
            primaryKey.Properties[0].Name);
    }

    [Fact]
    public void Id_ShouldNotBeGeneratedByDatabase()
    {
        var property = GetProperty(nameof(Unit.Id));

        Assert.Equal(
            ValueGenerated.Never,
            property.ValueGenerated);
    }

    [Fact]
    public void CourseId_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Unit.CourseId));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void Name_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Unit.Name));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void Name_ShouldHaveCorrectMaximumLength()
    {
        var property = GetProperty(nameof(Unit.Name));

        Assert.Equal(
            Unit.MaxNameLength,
            property.GetMaxLength());
    }

    [Fact]
    public void Description_ShouldBeOptional()
    {
        var property = GetProperty(nameof(Unit.Description));

        Assert.True(property.IsNullable);
    }

    [Fact]
    public void Description_ShouldHaveCorrectMaximumLength()
    {
        var property = GetProperty(nameof(Unit.Description));

        Assert.Equal(
            Unit.MaxDescriptionLength,
            property.GetMaxLength());
    }

    [Fact]
    public void DisplayOrder_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Unit.DisplayOrder));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void IsActive_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Unit.IsActive));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void CreatedAt_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Unit.CreatedAt));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void Unit_ShouldHaveRelationshipWithCourse()
    {
        var foreignKey = GetCourseForeignKey();

        Assert.Equal(
            typeof(Course),
            foreignKey.PrincipalEntityType.ClrType);

        Assert.Single(foreignKey.Properties);

        Assert.Equal(
            nameof(Unit.CourseId),
            foreignKey.Properties[0].Name);
    }

    [Fact]
    public void CourseRelationship_ShouldUseCascadeDelete()
    {
        var foreignKey = GetCourseForeignKey();

        Assert.Equal(
            DeleteBehavior.Cascade,
            foreignKey.DeleteBehavior);
    }

    [Fact]
    public void CourseId_ShouldHaveAnIndex()
    {
        var indexExists = _entityType
            .GetIndexes()
            .Any(index =>
                index.Properties.Count == 1 &&
                index.Properties[0].Name ==
                    nameof(Unit.CourseId));

        Assert.True(indexExists);
    }

    [Fact]
    public void CourseIdAndDisplayOrder_ShouldHaveCompositeIndex()
    {
        var index = _entityType
            .GetIndexes()
            .SingleOrDefault(index =>
                index.Properties.Count == 2 &&
                index.Properties[0].Name ==
                    nameof(Unit.CourseId) &&
                index.Properties[1].Name ==
                    nameof(Unit.DisplayOrder));

        Assert.NotNull(index);
        Assert.False(index.IsUnique);
    }

    private IProperty GetProperty(string propertyName)
    {
        return _entityType.FindProperty(propertyName)
            ?? throw new InvalidOperationException(
                $"{propertyName} was not found in the Unit mapping.");
    }

    private IForeignKey GetCourseForeignKey()
    {
        return _entityType
            .GetForeignKeys()
            .SingleOrDefault(foreignKey =>
                foreignKey.Properties.Count == 1 &&
                foreignKey.Properties[0].Name ==
                    nameof(Unit.CourseId))
            ?? throw new InvalidOperationException(
                "The Unit to Course foreign key was not found.");
    }
}