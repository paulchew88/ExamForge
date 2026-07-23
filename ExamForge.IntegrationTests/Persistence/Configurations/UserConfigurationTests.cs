using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public sealed class UserConfigurationTests
{
    private readonly IEntityType _entityType;

    public UserConfigurationTests()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=localhost;Database=ExamForgeTests;" +
                "Trusted_Connection=True;TrustServerCertificate=True;")
            .Options;

        using var context = new ExamForgeDbContext(options);

        _entityType = context.Model.FindEntityType(typeof(User))
            ?? throw new InvalidOperationException(
                "User was not found in the EF Core model.");
    }

    [Fact]
    public void User_ShouldMapToUsersTable()
    {
        Assert.Equal("User", _entityType.GetTableName());
    }

    [Fact]
    public void User_ShouldUseIdAsPrimaryKey()
    {
        var primaryKey = _entityType.FindPrimaryKey();

        Assert.NotNull(primaryKey);
        Assert.Single(primaryKey.Properties);
        Assert.Equal(
            nameof(User.Id),
            primaryKey.Properties[0].Name);
    }

    [Fact]
    public void Id_ShouldNotBeGeneratedByDatabase()
    {
        var property = GetProperty(nameof(User.Id));

        Assert.Equal(
            ValueGenerated.Never,
            property.ValueGenerated);
    }

    [Fact]
    public void CreatedAt_ShouldBeRequired()
    {
        var property = GetProperty(nameof(User.CreatedAt));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void ExternalIdentityId_ShouldBeRequired()
    {
        var property = GetProperty(nameof(User.ExternalIdentityId));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void FirstName_ShouldBeRequired()
    {
        var property = GetProperty(nameof(User.FirstName));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void FirstName_ShouldHaveMaximumLengthOf100()
    {
        var property = GetProperty(nameof(User.FirstName));

        Assert.Equal(100, property.GetMaxLength());
    }

    [Fact]
    public void LastName_ShouldBeRequired()
    {
        var property = GetProperty(nameof(User.LastName));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void LastName_ShouldHaveMaximumLengthOf100()
    {
        var property = GetProperty(nameof(User.LastName));

        Assert.Equal(100, property.GetMaxLength());
    }

    [Fact]
    public void Email_ShouldBeRequired()
    {
        var property = GetProperty(nameof(User.Email));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void Email_ShouldHaveMaximumLengthOf320()
    {
        var property = GetProperty(nameof(User.Email));

        Assert.Equal(320, property.GetMaxLength());
    }

    [Fact]
    public void Role_ShouldBeRequired()
    {
        var property = GetProperty(nameof(User.Role));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void Status_ShouldBeRequired()
    {
        var property = GetProperty(nameof(User.Status));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void IsActive_ShouldBeRequired()
    {
        var property = GetProperty(nameof(User.IsActive));

        Assert.False(property.IsNullable);
    }

    [Fact]
    public void ExternalIdentityId_ShouldHaveUniqueIndex()
    {
        var index = _entityType.GetIndexes()
            .Single(i =>
                i.Properties.Count == 1 &&
                i.Properties[0].Name == nameof(User.ExternalIdentityId));

        Assert.True(index.IsUnique);
    }

    [Fact]
    public void Email_ShouldHaveUniqueIndex()
    {
        var index = _entityType.GetIndexes()
            .Single(i =>
                i.Properties.Count == 1 &&
                i.Properties[0].Name == nameof(User.Email));

        Assert.True(index.IsUnique);
    }

    private IProperty GetProperty(string propertyName)
    {
        return _entityType.FindProperty(propertyName)
            ?? throw new InvalidOperationException(
                $"{propertyName} was not found in the User mapping.");
    }
}