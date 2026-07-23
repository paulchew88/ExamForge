using ExamForge.Domain.Entities;
using ExamForge.Domain.Enums;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Tests.Entities;

public sealed class QuestionAssetTests
{
    private static readonly Guid ValidQuestionId = Guid.CreateVersion7();

    [Fact]
    public void Create_WithValidDetails_ShouldCreateQuestionAsset()
    {
        var asset = CreateValidAsset();

        Assert.NotEqual(Guid.Empty, asset.Id);
        Assert.Equal(ValidQuestionId, asset.QuestionId);
        Assert.Equal("cpu-diagram.png", asset.OriginalFileName);
        Assert.Equal(
            "questions/question-id/assets/asset-id",
            asset.StorageKey);
        Assert.Equal("image/png", asset.ContentType);
        Assert.Equal(QuestionAssetType.Image, asset.AssetType);
        Assert.Equal(24_000, asset.SizeBytes);
        Assert.Equal("Figure 1: CPU architecture", asset.Caption);
        Assert.Equal(
            "Diagram showing the main components of a CPU",
            asset.AltText);
        Assert.Equal(1, asset.DisplayOrder);
        Assert.NotEqual(default, asset.CreatedAt);
    }

    [Fact]
    public void Create_ShouldTrimTextValues()
    {
        var asset = QuestionAsset.Create(
            ValidQuestionId,
            "  cpu-diagram.png  ",
            "  questions/question-id/assets/asset-id  ",
            "  image/png  ",
            QuestionAssetType.Image,
            24_000,
            "  Figure 1  ",
            "  CPU diagram  ",
            1);

        Assert.Equal("cpu-diagram.png", asset.OriginalFileName);
        Assert.Equal(
            "questions/question-id/assets/asset-id",
            asset.StorageKey);
        Assert.Equal("image/png", asset.ContentType);
        Assert.Equal("Figure 1", asset.Caption);
        Assert.Equal("CPU diagram", asset.AltText);
    }

    [Fact]
    public void Create_WithEmptyQuestionId_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            QuestionAsset.Create(
                Guid.Empty,
                "cpu-diagram.png",
                "questions/question-id/assets/asset-id",
                "image/png",
                QuestionAssetType.Image,
                24_000,
                null,
                null,
                1));

        Assert.Equal(
            "Question ID cannot be empty.",
            exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Create_WithEmptyOriginalFileName_ShouldThrowDomainException(
        string originalFileName)
    {
        var exception = Assert.Throws<DomainException>(() =>
            QuestionAsset.Create(
                ValidQuestionId,
                originalFileName,
                "questions/question-id/assets/asset-id",
                "image/png",
                QuestionAssetType.Image,
                24_000,
                null,
                null,
                1));

        Assert.Equal(
            "Original file name cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithOriginalFileNameOver255Characters_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            QuestionAsset.Create(
                ValidQuestionId,
                new string('a', 256),
                "questions/question-id/assets/asset-id",
                "image/png",
                QuestionAssetType.Image,
                24_000,
                null,
                null,
                1));

        Assert.Equal(
            "Original file name cannot exceed 255 characters.",
            exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Create_WithEmptyStorageKey_ShouldThrowDomainException(
        string storageKey)
    {
        var exception = Assert.Throws<DomainException>(() =>
            QuestionAsset.Create(
                ValidQuestionId,
                "cpu-diagram.png",
                storageKey,
                "image/png",
                QuestionAssetType.Image,
                24_000,
                null,
                null,
                1));

        Assert.Equal(
            "Storage key cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithStorageKeyOver1000Characters_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            QuestionAsset.Create(
                ValidQuestionId,
                "cpu-diagram.png",
                new string('a', 1001),
                "image/png",
                QuestionAssetType.Image,
                24_000,
                null,
                null,
                1));

        Assert.Equal(
            "Storage key cannot exceed 1000 characters.",
            exception.Message);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    public void Create_WithEmptyContentType_ShouldThrowDomainException(
        string contentType)
    {
        var exception = Assert.Throws<DomainException>(() =>
            QuestionAsset.Create(
                ValidQuestionId,
                "cpu-diagram.png",
                "questions/question-id/assets/asset-id",
                contentType,
                QuestionAssetType.Image,
                24_000,
                null,
                null,
                1));

        Assert.Equal(
            "Content type cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithContentTypeOver255Characters_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            QuestionAsset.Create(
                ValidQuestionId,
                "cpu-diagram.png",
                "questions/question-id/assets/asset-id",
                new string('a', 256),
                QuestionAssetType.Image,
                24_000,
                null,
                null,
                1));

        Assert.Equal(
            "Content type cannot exceed 255 characters.",
            exception.Message);
    }

    [Fact]
    public void Create_WithUndefinedAssetType_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            QuestionAsset.Create(
                ValidQuestionId,
                "cpu-diagram.png",
                "questions/question-id/assets/asset-id",
                "image/png",
                (QuestionAssetType)999,
                24_000,
                null,
                null,
                1));

        Assert.Equal(
            "Question asset type is invalid.",
            exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithInvalidSizeBytes_ShouldThrowDomainException(
        long sizeBytes)
    {
        var exception = Assert.Throws<DomainException>(() =>
            QuestionAsset.Create(
                ValidQuestionId,
                "cpu-diagram.png",
                "questions/question-id/assets/asset-id",
                "image/png",
                QuestionAssetType.Image,
                sizeBytes,
                null,
                null,
                1));

        Assert.Equal(
            "Asset size must be greater than zero.",
            exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Create_WithInvalidDisplayOrder_ShouldThrowDomainException(
        int displayOrder)
    {
        var exception = Assert.Throws<DomainException>(() =>
            QuestionAsset.Create(
                ValidQuestionId,
                "cpu-diagram.png",
                "questions/question-id/assets/asset-id",
                "image/png",
                QuestionAssetType.Image,
                24_000,
                null,
                null,
                displayOrder));

        Assert.Equal(
            "Display order must be greater than zero.",
            exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithEmptyCaption_ShouldStoreNull(string? caption)
    {
        var asset = QuestionAsset.Create(
            ValidQuestionId,
            "cpu-diagram.png",
            "questions/question-id/assets/asset-id",
            "image/png",
            QuestionAssetType.Image,
            24_000,
            caption,
            "CPU diagram",
            1);

        Assert.Null(asset.Caption);
    }

    [Fact]
    public void Create_WithCaptionOver1000Characters_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            QuestionAsset.Create(
                ValidQuestionId,
                "cpu-diagram.png",
                "questions/question-id/assets/asset-id",
                "image/png",
                QuestionAssetType.Image,
                24_000,
                new string('a', 1001),
                null,
                1));

        Assert.Equal(
            "Caption cannot exceed 1000 characters.",
            exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Create_WithEmptyAltText_ShouldStoreNull(string? altText)
    {
        var asset = QuestionAsset.Create(
            ValidQuestionId,
            "cpu-diagram.png",
            "questions/question-id/assets/asset-id",
            "image/png",
            QuestionAssetType.Image,
            24_000,
            null,
            altText,
            1);

        Assert.Null(asset.AltText);
    }

    [Fact]
    public void Create_WithAltTextOver1000Characters_ShouldThrowDomainException()
    {
        var exception = Assert.Throws<DomainException>(() =>
            QuestionAsset.Create(
                ValidQuestionId,
                "cpu-diagram.png",
                "questions/question-id/assets/asset-id",
                "image/png",
                QuestionAssetType.Image,
                24_000,
                null,
                new string('a', 1001),
                1));

        Assert.Equal(
            "Alt text cannot exceed 1000 characters.",
            exception.Message);
    }

    [Fact]
    public void UpdateCaption_WithValidCaption_ShouldUpdateCaption()
    {
        var asset = CreateValidAsset();

        asset.UpdateCaption("Updated caption");

        Assert.Equal("Updated caption", asset.Caption);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateCaption_WithEmptyCaption_ShouldSetCaptionToNull(
        string? caption)
    {
        var asset = CreateValidAsset();

        asset.UpdateCaption(caption);

        Assert.Null(asset.Caption);
    }

    [Fact]
    public void UpdateCaption_WithCaptionOver1000Characters_ShouldThrowDomainException()
    {
        var asset = CreateValidAsset();

        var exception = Assert.Throws<DomainException>(() =>
            asset.UpdateCaption(new string('a', 1001)));

        Assert.Equal(
            "Caption cannot exceed 1000 characters.",
            exception.Message);
    }

    [Fact]
    public void UpdateAltText_WithValidAltText_ShouldUpdateAltText()
    {
        var asset = CreateValidAsset();

        asset.UpdateAltText("Updated accessible description");

        Assert.Equal(
            "Updated accessible description",
            asset.AltText);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateAltText_WithEmptyAltText_ShouldSetAltTextToNull(
        string? altText)
    {
        var asset = CreateValidAsset();

        asset.UpdateAltText(altText);

        Assert.Null(asset.AltText);
    }

    [Fact]
    public void UpdateAltText_WithAltTextOver1000Characters_ShouldThrowDomainException()
    {
        var asset = CreateValidAsset();

        var exception = Assert.Throws<DomainException>(() =>
            asset.UpdateAltText(new string('a', 1001)));

        Assert.Equal(
            "Alt text cannot exceed 1000 characters.",
            exception.Message);
    }

    [Fact]
    public void ChangeDisplayOrder_WithValidOrder_ShouldUpdateDisplayOrder()
    {
        var asset = CreateValidAsset();

        asset.ChangeDisplayOrder(2);

        Assert.Equal(2, asset.DisplayOrder);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void ChangeDisplayOrder_WithInvalidOrder_ShouldThrowDomainException(
        int displayOrder)
    {
        var asset = CreateValidAsset();

        var exception = Assert.Throws<DomainException>(() =>
            asset.ChangeDisplayOrder(displayOrder));

        Assert.Equal(
            "Display order must be greater than zero.",
            exception.Message);
    }

    [Fact]
    public void ReplaceFile_WithValidDetails_ShouldReplaceFileMetadata()
    {
        var asset = CreateValidAsset();
        var originalId = asset.Id;
        var originalCreatedAt = asset.CreatedAt;

        asset.ReplaceFile(
            "starter-code.zip",
            "questions/question-id/assets/new-asset-id",
            "application/zip",
            QuestionAssetType.Archive,
            50_000);

        Assert.Equal(originalId, asset.Id);
        Assert.Equal(originalCreatedAt, asset.CreatedAt);
        Assert.Equal("starter-code.zip", asset.OriginalFileName);
        Assert.Equal(
            "questions/question-id/assets/new-asset-id",
            asset.StorageKey);
        Assert.Equal("application/zip", asset.ContentType);
        Assert.Equal(QuestionAssetType.Archive, asset.AssetType);
        Assert.Equal(50_000, asset.SizeBytes);

        Assert.Equal(
            "Figure 1: CPU architecture",
            asset.Caption);
        Assert.Equal(
            "Diagram showing the main components of a CPU",
            asset.AltText);
        Assert.Equal(1, asset.DisplayOrder);
    }

    [Fact]
    public void ReplaceFile_WithInvalidDetails_ShouldNotPartiallyUpdateAsset()
    {
        var asset = CreateValidAsset();

        Assert.Throws<DomainException>(() =>
            asset.ReplaceFile(
                "starter-code.zip",
                "",
                "application/zip",
                QuestionAssetType.Archive,
                50_000));

        Assert.Equal("cpu-diagram.png", asset.OriginalFileName);
        Assert.Equal(
            "questions/question-id/assets/asset-id",
            asset.StorageKey);
        Assert.Equal("image/png", asset.ContentType);
        Assert.Equal(QuestionAssetType.Image, asset.AssetType);
        Assert.Equal(24_000, asset.SizeBytes);
    }

    private static QuestionAsset CreateValidAsset()
    {
        return QuestionAsset.Create(
            ValidQuestionId,
            "cpu-diagram.png",
            "questions/question-id/assets/asset-id",
            "image/png",
            QuestionAssetType.Image,
            24_000,
            "Figure 1: CPU architecture",
            "Diagram showing the main components of a CPU",
            1);
    }
}