using ExamForge.Domain.Common;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Entities;

public class Classroom : Entity
{
    public const int MaxNameLength = 150;
    public const int MaxJoinCodeLength = 10;
    public Guid CourseId { get; private set; }
    public Guid TeacherId { get; private set; }
    public string Name { get; private set; }
    public string JoinCode { get; private set; }
    public bool IsActive { get; private set; }

    private Classroom()
    {
        Name = string.Empty;
        JoinCode = string.Empty;
    }

    private Classroom(
        Guid id,
        Guid courseId,
        Guid teacherId,
        string name,
        string joinCode,
        DateTimeOffset createdAt)
        : base(id, createdAt)
    {
        CourseId = courseId;
        TeacherId = teacherId;
        Name = name;
        JoinCode = joinCode;
        IsActive = true;
    }

    public static Classroom Create(
        Guid courseId,
        Guid teacherId,
        string name,
        string joinCode)
    {
        return new Classroom(
            Guid.CreateVersion7(),
            ValidateCourseId(courseId),
            ValidateTeacherId(teacherId),
            ValidateName(name),
            ValidateJoinCode(joinCode),
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
    private static string ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException(
                "Classroom name cannot be empty.");
        }
        var trimmedName = name.Trim();

        if (trimmedName.Length > MaxNameLength)
        {
            throw new DomainException(
                $"Classroom name cannot exceed {MaxNameLength} characters.");
        }
        return trimmedName;
    }

    public void RegenerateJoinCode(string newJoinCode)
    {
        JoinCode = ValidateJoinCode(newJoinCode);
    }

    private static string ValidateJoinCode(string joinCode)
    {
        if (string.IsNullOrWhiteSpace(joinCode))
        {
            throw new DomainException(
                "Classroom join code cannot be empty.");
        }
        var trimmedJoinCode = joinCode.Trim();
        if (trimmedJoinCode.Length > MaxJoinCodeLength)
        {
            throw new DomainException(
                $"Classroom join code cannot exceed {MaxJoinCodeLength} characters.");
        }
        return trimmedJoinCode.ToUpper();


    }
    public void ChangeTeacher(Guid newTeacherId)
    {
        TeacherId = ValidateTeacherId(newTeacherId);
    }
    private static Guid ValidateTeacherId(Guid teacherId)
    {
        if (teacherId == Guid.Empty)
        {
            throw new DomainException(
                "Teacher ID cannot be empty.");
        }
        return teacherId;
    }
    public void ChangeCourse(Guid newCourseId)
    {
        CourseId = ValidateCourseId(newCourseId);
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

}

