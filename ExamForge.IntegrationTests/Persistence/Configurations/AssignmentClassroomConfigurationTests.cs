using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public sealed class AssignmentClassroomConfigurationTests
{
    private readonly IEntityType _entityType;

    public AssignmentClassroomConfigurationTests()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=localhost;Database=ExamForgeTests;" +
                "Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new ExamForgeDbContext(options);

        _entityType = context.Model.FindEntityType(typeof(AssignmentClassroom))
            ?? throw new InvalidOperationException(
                "AssignmentClassroom was not found in the EF Core model.");
    }

    [Fact]
    public void AssignmentClassroom_ShouldMapToAssignmentClassroomTable()
    {
        Assert.Equal(
            "AssignmentClassroom",
            _entityType.GetTableName());
    }

    [Fact]
    public void AssignmentClassroom_ShouldUseIdAsPrimaryKey()
    {
        var primaryKey = _entityType.FindPrimaryKey();

        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);

        Assert.Equal(
            nameof(AssignmentClassroom.Id),
            primaryKey.Properties[0].Name);
    }

    [Fact]
    public void Id_ShouldNotBeGeneratedByDatabase()
    {
        var property = GetProperty(nameof(AssignmentClassroom.Id));

        Assert.Equal(
            ValueGenerated.Never,
            property.ValueGenerated);
    }

    [Fact]
    public void AssignmentId_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(AssignmentClassroom.AssignmentId))
                .IsNullable);
    }

    [Fact]
    public void ClassroomId_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(AssignmentClassroom.ClassroomId))
                .IsNullable);
    }

    [Fact]
    public void AssignedAt_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(AssignmentClassroom.AssignedAt))
                .IsNullable);
    }

    [Fact]
    public void IsActive_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(AssignmentClassroom.IsActive))
                .IsNullable);
    }

    [Fact]
    public void CreatedAt_ShouldBeRequired()
    {
        Assert.False(
            GetProperty(nameof(AssignmentClassroom.CreatedAt))
                .IsNullable);
    }

    [Fact]
    public void UnassignedAt_ShouldBeOptional()
    {
        Assert.True(
            GetProperty(nameof(AssignmentClassroom.UnassignedAt))
                .IsNullable);
    }

    [Fact]
    public void AssignmentClassroom_ShouldHaveUniqueIndexOnAssignmentIdAndClassroomId()
    {
        var index = _entityType.GetIndexes()
            .Single(i =>
                i.Properties.Count == 2 &&
                i.Properties[0].Name ==
                    nameof(AssignmentClassroom.AssignmentId) &&
                i.Properties[1].Name ==
                    nameof(AssignmentClassroom.ClassroomId));

        Assert.True(index.IsUnique);
    }

    [Fact]
    public void AssignmentClassroom_ShouldHaveRequiredRelationshipToAssignment()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk =>
                fk.Properties.Single().Name ==
                nameof(AssignmentClassroom.AssignmentId));

        Assert.Equal(
            nameof(Assignment),
            foreignKey.PrincipalEntityType.ClrType.Name);

        Assert.Equal(
            DeleteBehavior.Restrict,
            foreignKey.DeleteBehavior);

        Assert.True(foreignKey.IsRequired);
    }

    [Fact]
    public void AssignmentClassroom_ShouldHaveRequiredRelationshipToClassroom()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk =>
                fk.Properties.Single().Name ==
                nameof(AssignmentClassroom.ClassroomId));

        Assert.Equal(
            nameof(Classroom),
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
                $"{propertyName} was not found in the AssignmentClassroom mapping.");
    }
}