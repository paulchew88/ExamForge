using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public sealed class ClassroomStudentConfigurationTests
{
    private readonly IEntityType _entityType;

    public ClassroomStudentConfigurationTests()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=localhost;Database=ExamForgeTests;" +
                "Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new ExamForgeDbContext(options);

        _entityType = context.Model.FindEntityType(typeof(ClassroomStudent))
            ?? throw new InvalidOperationException(
                "ClassroomStudent was not found in the EF Core model.");
    }

    [Fact]
    public void ClassroomStudent_ShouldMapToClassroomStudentTable()
    {
        Assert.Equal("ClassroomStudent", _entityType.GetTableName());
    }

    [Fact]
    public void ClassroomStudent_ShouldUseIdAsPrimaryKey()
    {
        var primaryKey = _entityType.FindPrimaryKey();

        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);

        Assert.Equal(
            nameof(ClassroomStudent.Id),
            primaryKey.Properties[0].Name);
    }

    [Fact]
    public void Id_ShouldNotBeGeneratedByDatabase()
    {
        var property = GetProperty(nameof(ClassroomStudent.Id));

        Assert.Equal(
            ValueGenerated.Never,
            property.ValueGenerated);
    }

    [Fact]
    public void ClassroomId_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(ClassroomStudent.ClassroomId)).IsNullable);
    }

    [Fact]
    public void StudentId_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(ClassroomStudent.StudentId)).IsNullable);
    }

    [Fact]
    public void JoinedAt_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(ClassroomStudent.JoinedAt)).IsNullable);
    }

    [Fact]
    public void LeftAt_ShouldBeOptional()
    {
        Assert.True(GetProperty(nameof(ClassroomStudent.LeftAt)).IsNullable);
    }

    [Fact]
    public void IsActive_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(ClassroomStudent.IsActive)).IsNullable);
    }

    [Fact]
    public void CreatedAt_ShouldBeRequired()
    {
        Assert.False(GetProperty(nameof(ClassroomStudent.CreatedAt)).IsNullable);
    }

    [Fact]
    public void ClassroomStudent_ShouldHaveUniqueIndexOnClassroomIdAndStudentId()
    {
        var index = _entityType.GetIndexes()
            .Single(i =>
                i.Properties.Count == 2 &&
                i.Properties[0].Name == nameof(ClassroomStudent.ClassroomId) &&
                i.Properties[1].Name == nameof(ClassroomStudent.StudentId));

        Assert.True(index.IsUnique);
    }

    [Fact]
    public void ClassroomStudent_ShouldHaveRequiredRelationshipToClassroom()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk =>
                fk.Properties.Single().Name ==
                nameof(ClassroomStudent.ClassroomId));

        Assert.Equal(nameof(Classroom), foreignKey.PrincipalEntityType.ClrType.Name);
        Assert.Equal(DeleteBehavior.Restrict, foreignKey.DeleteBehavior);
    }

    [Fact]
    public void ClassroomStudent_ShouldHaveRequiredRelationshipToStudent()
    {
        var foreignKey = _entityType.GetForeignKeys()
            .Single(fk =>
                fk.Properties.Single().Name ==
                nameof(ClassroomStudent.StudentId));

        Assert.Equal(nameof(User), foreignKey.PrincipalEntityType.ClrType.Name);
        Assert.Equal(DeleteBehavior.Restrict, foreignKey.DeleteBehavior);
    }

    private IProperty GetProperty(string propertyName)
    {
        return _entityType.FindProperty(propertyName)
            ?? throw new InvalidOperationException(
                $"{propertyName} was not found in the ClassroomStudent mapping.");
    }
}