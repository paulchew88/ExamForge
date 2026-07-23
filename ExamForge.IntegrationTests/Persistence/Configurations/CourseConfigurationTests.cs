using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public sealed class CourseConfigurationTests
{
    private readonly IEntityType _entityType;

    public CourseConfigurationTests()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=localhost;Database=ExamForgeTests;" +
                "Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new ExamForgeDbContext(options);

        _entityType = context.Model.FindEntityType(typeof(Course))
            ?? throw new InvalidOperationException(
                "Course was not found in the EF Core model.");
    }

    [Fact]
    public void Course_ShouldMapToCoursesTable()
    {
        Assert.Equal("Course", _entityType.GetTableName());
    }

    [Fact]
    public void Course_ShouldUseIdAsPrimaryKey()
    {
        var primaryKey = _entityType.FindPrimaryKey();

        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal(
            nameof(Course.Id),
            primaryKey.Properties[0].Name);
    }

    [Fact]
    public void Id_ShouldNotBeGeneratedByDatabase()
    {
        var property = GetProperty(nameof(Course.Id));

        Assert.Equal(
            ValueGenerated.Never,
            property.ValueGenerated);
    }

    [Fact]
    public void CreatedAt_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Course.CreatedAt));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void Name_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Course.Name));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void Name_ShouldHaveMaximumLengthOf150()
    {
        var property = GetProperty(nameof(Course.Name));

        Assert.Equal(150, property.GetMaxLength());
    }

    [Fact]
    public void Description_ShouldBeOptional()
    {
        var property = GetProperty(nameof(Course.Description));

        Assert.True(property.IsNullable);
    }

    [Fact]
    public void Description_ShouldHaveMaximumLengthOf1000()
    {
        var property = GetProperty(nameof(Course.Description));

        Assert.Equal(1000, property.GetMaxLength());
    }

    [Fact]
    public void IsActive_ShouldBeRequired()
    {
        var property = GetProperty(nameof(Course.IsActive));

        Assert.False(property.IsNullable);
    }

    private IProperty GetProperty(string propertyName)
    {
        return _entityType.FindProperty(propertyName)
            ?? throw new InvalidOperationException(
                $"{propertyName} was not found in the Course mapping.");
    }
}