namespace ExamForge.Domain.Entities;

using ExamForge.Domain.Common;
using ExamForge.Domain.Exceptions;
public class Course : Entity
{
    public const int MaxNameLength = 150;
    public const int MaxDescriptionLength = 1000;


    public string Name { get; private set; }

    public string? Description { get; private set; }

    public bool IsActive { get; private set; }



    private Course()
    {
        Name = string.Empty;
    }

    private Course(
    Guid id,
    string name,
    string? description,
    DateTimeOffset createdAt)
    : base(id, createdAt)
    {
        Name = name;
        Description = description;
        IsActive = true;
    }

    public static Course Create(
        string name,
        string? description = null)
    {
        return new Course(
            Guid.CreateVersion7(),
            ValidateName(name),
            ValidateDescription(description),
            DateTimeOffset.UtcNow);
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
            throw new DomainException("Course name cannot be empty.");
        }

        var trimmedName = newName.Trim();

        if (trimmedName.Length > MaxNameLength)
        {
            throw new DomainException($"Course name cannot exceed {MaxNameLength} characters.");
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
                $"Course description cannot exceed {MaxDescriptionLength} characters.");
        }

        return trimmedDescription;
    }



}