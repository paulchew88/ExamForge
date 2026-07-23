using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public sealed class ClassroomConfigurationTests
{
    private readonly IEntityType _entityType;

    public ClassroomConfigurationTests()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=localhost;Database=ExamForgeTests;" +
                "Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new ExamForgeDbContext(options);

        _entityType = context.Model.FindEntityType(typeof(Classroom))
            ?? throw new InvalidOperationException(
                "Classroom was not found in the EF Core model.");
    }

    [Fact]
    public void Classroom_ShouldMapToClassroomTable()
    {
        Assert.Equal("Classroom", _entityType.GetTableName());
    }

    [Fact]
    public void Classroom_ShouldUseIdAsPrimaryKey()
    {
        var primaryKey = _entityType.FindPrimaryKey();

        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal(
            nameof(Classroom.Id),
            primaryKey.Properties[0].Name);
    }

    [Fact]
    public void Id_ShouldNotBeGeneratedByDatabase()
    {
        var property = GetProperty(nameof(Classroom.Id));

        Assert.Equal(
            ValueGenerated.Never,
            property.ValueGenerated);
    }

    [Fact]
    public void CourseId_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(Classroom.CourseId)).IsNullable);
    }

    [Fact]
    public void TeacherId_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(Classroom.TeacherId)).IsNullable);
    }

    [Fact]
    public void CreatedAt_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(Classroom.CreatedAt)).IsNullable);
    }

    [Fact]
    public void Name_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(Classroom.Name)).IsNullable);
    }

    [Fact]
    public void Name_ShouldHaveMaximumLengthOf150()
    {
        Assert.Equal(
            150,
            GetProperty(nameof(Classroom.Name)).GetMaxLength());
    }

    [Fact]
    public void JoinCode_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(Classroom.JoinCode)).IsNullable);
    }

    [Fact]
    public void JoinCode_ShouldHaveMaximumLengthOf10()
    {
        Assert.Equal(
            10,
            GetProperty(nameof(Classroom.JoinCode)).GetMaxLength());
    }

    [Fact]
    public void IsActive_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(Classroom.IsActive)).IsNullable);
    }

    [Fact]
    public void JoinCode_ShouldHaveUniqueIndex()
    {
        var index = _entityType.GetIndexes()
            .Single(i =>
                i.Properties.Count == 1 &&
                i.Properties[0].Name == nameof(Classroom.JoinCode));

        Assert.True(index.IsUnique);
    }

    [Fact]
    public void Classroom_ShouldHaveRequiredRelationshipToCourse()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk => fk.Properties.Single().Name == nameof(Classroom.CourseId));

        Assert.Equal(nameof(Course), foreignKey.PrincipalEntityType.ClrType.Name);
        Assert.False(foreignKey.IsRequiredDependent);
        Assert.Equal(DeleteBehavior.Restrict, foreignKey.DeleteBehavior);
    }

    [Fact]
    public void Classroom_ShouldHaveRequiredRelationshipToTeacher()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk => fk.Properties.Single().Name == nameof(Classroom.TeacherId));

        Assert.Equal(nameof(User), foreignKey.PrincipalEntityType.ClrType.Name);
        Assert.False(foreignKey.IsRequiredDependent);
        Assert.Equal(DeleteBehavior.Restrict, foreignKey.DeleteBehavior);
    }

    private IProperty GetProperty(string propertyName)
    {
        return _entityType.FindProperty(propertyName)
            ?? throw new InvalidOperationException(
                $"{propertyName} was not found in the Classroom mapping.");
    }
}