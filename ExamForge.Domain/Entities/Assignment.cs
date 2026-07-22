using ExamForge.Domain.Common;
using ExamForge.Domain.Exceptions;
namespace ExamForge.Domain.Entities;

public class Assignment : Entity
{
    public const int MaxInstructionsLength = 5000;
    public const int MaxTitleLength = 200;
    public Guid ClassroomId { get; private set; }
    public string Title { get; private set; }
    public string Instructions { get; private set; }
    public DateTimeOffset OpensAt { get; private set; }
    public DateTimeOffset DueAt { get; private set; }
    public bool IsPublished { get; private set; }

    private Assignment()
    {
        Title = string.Empty;
        Instructions = string.Empty;
    }
    private Assignment(
        Guid id,
        Guid classroomId,
        string title,
        string instructions,
        DateTimeOffset opensAt,
        DateTimeOffset dueAt,
        bool isPublished,
        DateTimeOffset createdAt)
        : base(id, createdAt)
    {
        ClassroomId = classroomId;
        Title = title;
        Instructions = instructions;
        OpensAt = opensAt;
        DueAt = dueAt;
        IsPublished = isPublished;
    }

    public static Assignment Create(
        Guid classroomId,
        string title,
        string instructions,
        DateTimeOffset opensAt,
        DateTimeOffset dueAt)
    {
        return new Assignment(
            Guid.CreateVersion7(),
            ValidateClassroomId(classroomId),
            ValidateTitle(title),
            ValidateInstructions(instructions),
            ValidateOpensAt(opensAt),
            ValidateDueAt(opensAt, dueAt),
            false, // Default to not published
            DateTimeOffset.UtcNow);
    }

    public void Publish()
    {
        IsPublished = true;
    }
    public void ReturnToDraft()
    {
        IsPublished = false;
    }
    private static Guid ValidateClassroomId(Guid classroomId)
    {
        if (classroomId == Guid.Empty)
        {
            throw new DomainException("Classroom ID cannot be empty.");
        }
        return classroomId;
    }
    public void ChangeTitle(string newTitle)
    {
        Title = ValidateTitle(newTitle);
    }

    private static string ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainException("Assignment title cannot be empty.");
        }
        var trimmedTitle = title?.Trim() ?? string.Empty;
        if (trimmedTitle.Length > MaxTitleLength)
        {
            throw new DomainException($"Assignment title cannot exceed {MaxTitleLength} characters.");
        }
        return trimmedTitle;
    }

    private static DateTimeOffset ValidateOpensAt(DateTimeOffset opensAt)
    {
        if (opensAt == default)
        {
            throw new DomainException(
                "Assignment open date must be in the future.");
        }

        return opensAt;
    }

    private static DateTimeOffset ValidateDueAt(DateTimeOffset opensAt, DateTimeOffset dueAt)
    {
        if (dueAt == default)
        {
            throw new DomainException(
                "Assignment due date cannot be empty.");
        }

        if (dueAt <= opensAt)
        {
            throw new DomainException(
                "Assignment due date must be later than the open date.");
        }

        return dueAt;
    }

    public void ChangeInstructions(string newInstructions)
    {
        Instructions = ValidateInstructions(newInstructions);
    }
    private static string ValidateInstructions(string instructions)
    {
        if (string.IsNullOrWhiteSpace(instructions))
        {
            return string.Empty;

        }

        var trimmedInstructions = instructions?.Trim() ?? string.Empty;

        if (trimmedInstructions.Length > MaxInstructionsLength)
        {
            throw new DomainException($"Assignment instructions cannot exceed {MaxInstructionsLength} characters.");
        }
        return trimmedInstructions;
    }
    public void Reschedule(DateTimeOffset opensAt, DateTimeOffset dueAt)
    {
        var validatedOpensAt = ValidateOpensAt(opensAt);
        var validatedDueAt = ValidateDueAt(validatedOpensAt, dueAt);

        OpensAt = validatedOpensAt;
        DueAt = validatedDueAt;
    }
}
