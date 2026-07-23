using ExamForge.Domain.Common;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Entities;

public class Unit : Entity
{
    public const int MaxNameLength = 150;
    public const int MaxDescriptionLength = 1000;
    public Guid CourseId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public int DisplayOrder { get; private set; }
    public bool IsActive { get; private set; }

    private Unit()
    {
        Name = string.Empty;
    }
    private Unit(
        Guid id,
        Guid courseId,
        string name,
        string? description,
        int displayOrder,
        DateTimeOffset createdAt)
        : base(id, createdAt)
    {
        CourseId = courseId;
        Name = name;
        Description = description;
        DisplayOrder = displayOrder;
        IsActive = true;
    }
    public static Unit Create(
       Guid courseId,
       string name,
       string? description,
       int displayOrder)
    {
        return new Unit(
            Guid.CreateVersion7(),
            ValidateCourseId(courseId),
            ValidateName(name),
            ValidateDescription(description),
            ValidateDisplayOrder(displayOrder),
            DateTimeOffset.UtcNow);
    }
    private static Guid ValidateCourseId(Guid courseId)
    {
        if (courseId == Guid.Empty)
        {
            throw new DomainException(
                "Course ID cannot be empty.");
        }

        return courseId;
    }
    public void Deactivate()
    {
        IsActive = false;
    }
    public void Activate()
    {
        IsActive = true;
    }


    public void Rename(string newName)
    {
        Name = ValidateName(newName);
    }

    private static string ValidateName(string newName)
    {
        if (string.IsNullOrWhiteSpace(newName))
        {
            throw new DomainException("Unit name cannot be empty.");
        }

        var trimmedName = newName.Trim();

        if (trimmedName.Length > MaxNameLength)
        {
            throw new DomainException($"Unit name cannot exceed {MaxNameLength} characters.");
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
                $"Unit description cannot exceed {MaxDescriptionLength} characters.");
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
}
