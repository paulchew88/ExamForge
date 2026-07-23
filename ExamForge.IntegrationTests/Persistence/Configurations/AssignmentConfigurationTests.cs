using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public sealed class AssignmentConfigurationTests
{
    private readonly IEntityType _entityType;

    public AssignmentConfigurationTests()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=localhost;Database=ExamForgeTests;" +
                "Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new ExamForgeDbContext(options);

        _entityType = context.Model.FindEntityType(typeof(Assignment))
            ?? throw new InvalidOperationException(
                "Assignment was not found in the EF Core model.");
    }

    [Fact]
    public void Assignment_ShouldMapToAssignmentTable()
    {
        Assert.Equal("Assignment", _entityType.GetTableName());
    }

    [Fact]
    public void Assignment_ShouldUseIdAsPrimaryKey()
    {
        var primaryKey = _entityType.FindPrimaryKey();

        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);

        Assert.Equal(nameof(Assignment.Id), primaryKey.Properties[0].Name);
    }

    [Fact]
    public void Id_ShouldNotBeGeneratedByDatabase()
    {
        var property = GetProperty(nameof(Assignment.Id));

        Assert.Equal(ValueGenerated.Never, property.ValueGenerated);
    }

    [Fact]
    public void ClassroomId_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(Assignment.ClassroomId)).IsNullable);
    }

    [Fact]
    public void Title_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(Assignment.Title)).IsNullable);
    }

    [Fact]
    public void Title_ShouldHaveCorrectMaxLength()
    {
        Assert.Equal(
            Assignment.MaxTitleLength,
            GetProperty(nameof(Assignment.Title)).GetMaxLength());
    }

    [Fact]
    public void Instructions_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(Assignment.Instructions)).IsNullable);
    }

    [Fact]
    public void Instructions_ShouldHaveCorrectMaxLength()
    {
        Assert.Equal(
            Assignment.MaxInstructionsLength,
            GetProperty(nameof(Assignment.Instructions)).GetMaxLength());
    }

    [Fact]
    public void OpensAt_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(Assignment.OpensAt)).IsNullable);
    }

    [Fact]
    public void DueAt_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(Assignment.DueAt)).IsNullable);
    }

    [Fact]
    public void IsPublished_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(Assignment.IsPublished)).IsNullable);
    }

    [Fact]
    public void CreatedAt_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(Assignment.CreatedAt)).IsNullable);
    }

    [Fact]
    public void Assignment_ShouldHaveIndexOnClassroomId()
    {
        var index = _entityType.GetIndexes()
            .Single(i =>
                i.Properties.Count == 1 &&
                i.Properties[0].Name == nameof(Assignment.ClassroomId));

        Assert.False(index.IsUnique);
    }

    [Fact]
    public void Assignment_ShouldHaveRequiredRelationshipToClassroom()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk =>
                fk.Properties.Single().Name ==
                nameof(Assignment.ClassroomId));

        Assert.Equal(
            nameof(Classroom),
            foreignKey.PrincipalEntityType.ClrType.Name);

        Assert.Equal(
            DeleteBehavior.Restrict,
            foreignKey.DeleteBehavior);
    }

    private IProperty GetProperty(string propertyName)
    {
        return _entityType.FindProperty(propertyName)
            ?? throw new InvalidOperationException(
                $"{propertyName} was not found in the Assignment mapping.");
    }
}