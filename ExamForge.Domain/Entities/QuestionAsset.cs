using ExamForge.Domain.Common;
using ExamForge.Domain.Enums;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Entities;

public class QuestionAsset : Entity
{
    public const int MaxFileNameLength = 255;
    public const int MaxStorageKeyLength = 1_000;
    public const int MaxContentTypeLength = 255;
    public const int MaxAltTextLength = 1_000;
    public const int MaxCaptionLength = 1_000;

    public Guid QuestionId { get; private set; }
    public string OriginalFileName { get; private set; } = null;
    public string StorageKey { get; private set; }
    public string ContentType { get; private set; }
    public QuestionAssetType AssetType { get; private set; }
    public long SizeBytes { get; private set; }
    public string? Caption { get; private set; }
    public string? AltText { get; private set; }
    public int DisplayOrder { get; private set; }

    private QuestionAsset() { } // For EF Core


    private QuestionAsset(
        Guid id,
         Guid questionId,
         string originalFileName,
         string storageKey,
         string contentType,
         QuestionAssetType assetType,
         long sizeBytes,
         string? caption,
         string? altText,
         DateTimeOffset createdAt,
         int displayOrder)
        : base(id, createdAt)
    {
        QuestionId = ValidateQuestionId(questionId);
        OriginalFileName = ValidateOriginalFileName(originalFileName);
        StorageKey = ValidateStorageKey(storageKey);
        ContentType = ValidateContentType(contentType);
        AssetType = ValidateAssetType(assetType);
        SizeBytes = ValidateSizeBytes(sizeBytes);
        Caption = ValidateCaption(caption);
        AltText = ValidateAltText(altText);
        DisplayOrder = ValidateDisplayOrder(displayOrder);
    }

    public static QuestionAsset Create(
         Guid questionId,
         string originalFileName,
         string storageKey,
         string contentType,
         QuestionAssetType assetType,
         long sizeBytes,
         string? caption,
         string? altText,
         int displayOrder)
    {
        return new QuestionAsset(
            Guid.CreateVersion7(),
           questionId,
            originalFileName,
           storageKey,
            contentType,
            assetType,
            sizeBytes,
            caption,
            altText,
            DateTimeOffset.UtcNow,
            displayOrder);
    }

    private static Guid ValidateQuestionId(Guid questionId)
    {
        if (questionId == Guid.Empty)
            throw new DomainException("Question ID cannot be empty.");
        return questionId;
    }
    private static string ValidateOriginalFileName(string originalFileName)
    {
        if (string.IsNullOrWhiteSpace(originalFileName))
            throw new DomainException("Original file name cannot be empty.");
        var trimmedFileName = originalFileName.Trim();
        if (trimmedFileName.Length > MaxFileNameLength)
            throw new DomainException($"Original file name cannot exceed {MaxFileNameLength} characters.");
        return trimmedFileName;
    }
    private static string ValidateStorageKey(string storageKey)
    {
        if (string.IsNullOrWhiteSpace(storageKey))
            throw new DomainException("Storage key cannot be empty.");
        var trimmedStorageKey = storageKey.Trim();
        if (trimmedStorageKey.Length > MaxStorageKeyLength)
            throw new DomainException($"Storage key cannot exceed {MaxStorageKeyLength} characters.");
        return trimmedStorageKey;
    }
    private static string ValidateContentType(string contentType)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            throw new DomainException("Content type cannot be empty.");
        var trimmedContentType = contentType.Trim();
        if (trimmedContentType.Length > MaxContentTypeLength)
            throw new DomainException($"Content type cannot exceed {MaxContentTypeLength} characters.");
        return trimmedContentType;
    }
    private static long ValidateSizeBytes(long sizeBytes)
    {
        if (sizeBytes <= 0)
            throw new DomainException("Asset size must be greater than zero.");
        return sizeBytes;
    }

    private static string? ValidateCaption(string? caption)
    {
        if (string.IsNullOrWhiteSpace(caption))
        {
            return null;
        }
        var trimmedCaption = caption.Trim();

        if (trimmedCaption.Length > MaxCaptionLength)
        {
            throw new DomainException($"Caption cannot exceed {MaxCaptionLength} characters.");
        }
        return trimmedCaption;
    }

    private static string? ValidateAltText(string? altText)
    {
        if (string.IsNullOrWhiteSpace(altText))
        {
            return null;
        }

        var trimmedAltText = altText.Trim();
        if (trimmedAltText.Length > MaxAltTextLength)
        {
            throw new DomainException($"Alt text cannot exceed {MaxAltTextLength} characters.");
        }
        return trimmedAltText;
    }
    private static int ValidateDisplayOrder(int displayOrder)
    {
        if (displayOrder < 1)
        {
            throw new DomainException("Display order must be greater than zero.");
        }
        return displayOrder;
    }
    private static QuestionAssetType ValidateAssetType(QuestionAssetType assetType)
    {
        if (!Enum.IsDefined(typeof(QuestionAssetType), assetType))
            throw new DomainException("Question asset type is invalid.");
        return assetType;
    }
    public void UpdateCaption(string? newCaption)
    {
        Caption = ValidateCaption(newCaption);
    }
    public void UpdateAltText(string? newAltText)
    {
        AltText = ValidateAltText(newAltText);
    }
    public void ChangeDisplayOrder(int newDisplayOrder)
    {
        DisplayOrder = ValidateDisplayOrder(newDisplayOrder);
    }

    public void ReplaceFile(string newOriginalFileName, string newStorageKey, string newContentType, QuestionAssetType newAssetType, long newSizeBytes)
    {
        var validatedOriginalFileName = ValidateOriginalFileName(newOriginalFileName);
        var validatedStorageKey = ValidateStorageKey(newStorageKey);
        var validatedContentType = ValidateContentType(newContentType);
        var validatedAssetType = ValidateAssetType(newAssetType);
        var validatedSizeBytes = ValidateSizeBytes(newSizeBytes);

        OriginalFileName = validatedOriginalFileName;
        StorageKey = validatedStorageKey;
        ContentType = validatedContentType;
        AssetType = validatedAssetType;
        SizeBytes = validatedSizeBytes;
    }

}
