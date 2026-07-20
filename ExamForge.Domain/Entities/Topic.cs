using ExamForge.Domain.Common;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Entities;

public class Topic : Entity
{
    private const int MaxNameLength = 150;
    private const int MaxDescriptionLength = 1000;
    public Guid UnitId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }

    private Topic()
    {
        Name = string.Empty;
    }
    private Topic(
         Guid id,
         Guid unitId,
         string name,
         string? description,
         int displayOrder,
         DateTimeOffset createdAt)
         : base(id, createdAt)
    {
        UnitId = unitId;
        Name = name;
        Description = description;
        DisplayOrder = displayOrder;
        IsActive = true;
    }
    public static Topic Create(
       Guid unitId,
       string name,
       string? description,
       int displayOrder)
    {
        return new Topic(
            Guid.CreateVersion7(),
            ValidateUnitId(unitId),
            ValidateName(name),
            ValidateDescription(description),
            ValidateDisplayOrder(displayOrder),
            DateTimeOffset.UtcNow);
    }
    public void Rename(string newName)
    {
        Name = ValidateName(newName);
    }

    private static Guid ValidateUnitId(Guid unitId)
    {
        if (unitId == Guid.Empty)
        {
            throw new DomainException(
                "Unit ID cannot be empty.");
        }

        return unitId;
    }
    private static string ValidateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new DomainException("Topic name cannot be empty.");
        }

        var trimmedName = newName.Trim();

        if (trimmedName.Length > MaxNameLength)
        {
            throw new DomainException($"Topic name cannot exceed {MaxNameLength} characters.");
        }
        return trimmedName;
    }

    public void UpdateDescription(string? newDescription)
    {
        Description = ValidateDescription(newDescription);
    }

    private static string? ValidateDescription(string? description)
    {
        if (string.IsNullOrWhiteSpace(description))
        {
            return null;
        }

        var trimmedDescription = description.Trim();

        if (trimmedDescription.Length > MaxDescriptionLength)
        {
            throw new DomainException(
                $"Topic description cannot exceed {MaxDescriptionLength} characters.");
        }

        return trimmedDescription;
    }
    public void ChangeDisplayOrder(int newDisplayOrder)
    {
        DisplayOrder = ValidateDisplayOrder(newDisplayOrder);
    }
    private static int ValidateDisplayOrder(int displayOrder)
    {
        if (displayOrder < 1)
        {
            throw new DomainException(
                "Display order must be greater than zero.");
        }
        return displayOrder;
    }
    public void Activate()
    {
        IsActive = true;
    }
    public void Deactivate()
    {
        IsActive = false;
    }
}
