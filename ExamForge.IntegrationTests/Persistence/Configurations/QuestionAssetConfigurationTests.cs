using ExamForge.Domain.Entities;
using ExamForge.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExamForge.IntegrationTests.Persistence.Configurations;

public class QuestionAssetConfigurationTests
{
    private static ExamForgeDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ExamForgeDbContext>()
            .UseSqlServer(
                "Server=(localdb)\\mssqllocaldb;" +
                "Database=ExamForgeConfigurationTests;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;")
            .Options;

        return new ExamForgeDbContext(options);
    }

    [Fact]
    public void QuestionAsset_ShouldMapToQuestionAssetsTable()
    {
        using var context = CreateContext();

        var entityType = context.Model.FindEntityType(typeof(QuestionAsset));

        Assert.NotNull(entityType);
        Assert.Equal("QuestionAsset", entityType.GetTableName());
    }

    [Fact]
    public void QuestionAsset_Id_ShouldBePrimaryKeyAndNotDatabaseGenerated()
    {
        using var context = CreateContext();

        var entityType = context.Model.FindEntityType(typeof(QuestionAsset));
        var primaryKey = entityType!.FindPrimaryKey();
        var idProperty = entityType.FindProperty(nameof(QuestionAsset.Id));

        Assert.NotNull(primaryKey);
        Assert.NotNull(idProperty);

        Assert.Single(primaryKey.Properties);
        Assert.Equal(nameof(QuestionAsset.Id), primaryKey.Properties[0].Name);
        Assert.Equal(ValueGenerated.Never, idProperty.ValueGenerated);
    }

    [Theory]
    [InlineData(nameof(QuestionAsset.QuestionId))]
    [InlineData(nameof(QuestionAsset.OriginalFileName))]
    [InlineData(nameof(QuestionAsset.StorageKey))]
    [InlineData(nameof(QuestionAsset.ContentType))]
    [InlineData(nameof(QuestionAsset.AssetType))]
    [InlineData(nameof(QuestionAsset.SizeBytes))]
    [InlineData(nameof(QuestionAsset.DisplayOrder))]
    [InlineData(nameof(QuestionAsset.CreatedAt))]
    public void RequiredProperties_ShouldNotBeNullable(string propertyName)
    {
        using var context = CreateContext();

        var entityType = context.Model.FindEntityType(typeof(QuestionAsset));
        var property = entityType!.FindProperty(propertyName);

        Assert.NotNull(property);
        Assert.False(property.IsNullable);
    }

    [Theory]
    [InlineData(nameof(QuestionAsset.Caption))]
    [InlineData(nameof(QuestionAsset.AltText))]
    public void OptionalProperties_ShouldBeNullable(string propertyName)
    {
        using var context = CreateContext();

        var entityType = context.Model.FindEntityType(typeof(QuestionAsset));
        var property = entityType!.FindProperty(propertyName);

        Assert.NotNull(property);
        Assert.True(property.IsNullable);
    }

    [Fact]
    public void OriginalFileName_ShouldHaveCorrectMaximumLength()
    {
        using var context = CreateContext();

        var property = GetProperty(context, nameof(QuestionAsset.OriginalFileName));

        Assert.Equal(
            QuestionAsset.MaxFileNameLength,
            property.GetMaxLength());
    }

    [Fact]
    public void StorageKey_ShouldHaveCorrectMaximumLength()
    {
        using var context = CreateContext();

        var property = GetProperty(context, nameof(QuestionAsset.StorageKey));

        Assert.Equal(
            QuestionAsset.MaxStorageKeyLength,
            property.GetMaxLength());
    }

    [Fact]
    public void ContentType_ShouldHaveCorrectMaximumLength()
    {
        using var context = CreateContext();

        var property = GetProperty(context, nameof(QuestionAsset.ContentType));

        Assert.Equal(
            QuestionAsset.MaxContentTypeLength,
            property.GetMaxLength());
    }

    [Fact]
    public void Caption_ShouldHaveCorrectMaximumLength()
    {
        using var context = CreateContext();

        var property = GetProperty(context, nameof(QuestionAsset.Caption));

        Assert.Equal(
            QuestionAsset.MaxCaptionLength,
            property.GetMaxLength());
    }

    [Fact]
    public void AltText_ShouldHaveCorrectMaximumLength()
    {
        using var context = CreateContext();

        var property = GetProperty(context, nameof(QuestionAsset.AltText));

        Assert.Equal(
            QuestionAsset.MaxAltTextLength,
            property.GetMaxLength());
    }

    [Fact]
    public void AssetType_ShouldBeStoredAsString()
    {
        using var context = CreateContext();

        var property = GetProperty(
            context,
            nameof(QuestionAsset.AssetType));

        var converter = property.GetTypeMapping().Converter;

        Assert.NotNull(converter);
        Assert.Equal(typeof(string), converter.ProviderClrType);
    }

    [Fact]
    public void AssetType_ShouldHaveMaximumLengthOfFifty()
    {
        using var context = CreateContext();

        var property = GetProperty(
            context,
            nameof(QuestionAsset.AssetType));

        Assert.Equal(50, property.GetMaxLength());
    }

    [Fact]
    public void QuestionAsset_ShouldHaveRelationshipWithQuestion()
    {
        using var context = CreateContext();

        var entityType = context.Model.FindEntityType(typeof(QuestionAsset));

        var foreignKey = Assert.Single(
            entityType!.GetForeignKeys());

        Assert.Equal(typeof(Question), foreignKey.PrincipalEntityType.ClrType);

        Assert.Single(foreignKey.Properties);

        Assert.Equal(
            nameof(QuestionAsset.QuestionId),
            foreignKey.Properties[0].Name);
    }

    [Fact]
    public void QuestionRelationship_ShouldUseCascadeDelete()
    {
        using var context = CreateContext();

        var entityType = context.Model.FindEntityType(typeof(QuestionAsset));

        var foreignKey = Assert.Single(
            entityType!.GetForeignKeys());

        Assert.Equal(
            DeleteBehavior.Cascade,
            foreignKey.DeleteBehavior);
    }

    [Fact]
    public void QuestionId_ShouldHaveAnIndex()
    {
        using var context = CreateContext();

        var entityType = context.Model.FindEntityType(typeof(QuestionAsset));

        var indexExists = entityType!
            .GetIndexes()
            .Any(index =>
                index.Properties.Count == 1 &&
                index.Properties[0].Name ==
                    nameof(QuestionAsset.QuestionId));

        Assert.True(indexExists);
    }

    [Fact]
    public void QuestionIdAndDisplayOrder_ShouldHaveCompositeIndex()
    {
        using var context = CreateContext();

        var entityType = context.Model.FindEntityType(typeof(QuestionAsset));

        var index = entityType!
            .GetIndexes()
            .SingleOrDefault(index =>
                index.Properties.Count == 2 &&
                index.Properties[0].Name ==
                    nameof(QuestionAsset.QuestionId) &&
                index.Properties[1].Name ==
                    nameof(QuestionAsset.DisplayOrder));

        Assert.NotNull(index);
        Assert.False(index.IsUnique);
    }

    private static IReadOnlyProperty GetProperty(
        ExamForgeDbContext context,
        string propertyName)
    {
        var entityType = context.Model.FindEntityType(typeof(QuestionAsset));

        Assert.NotNull(entityType);

        var property = entityType.FindProperty(propertyName);

        Assert.NotNull(property);

        return property;
    }
}