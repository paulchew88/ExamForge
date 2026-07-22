using ExamForge.Domain.Common;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Entities;

public class ClassroomStudent : Entity
{
    public Guid ClassroomId { get; private set; }
    public Guid StudentId { get; private set; }
    public DateTimeOffset JoinedAt { get; private set; }
    public DateTimeOffset? LeftAt { get; private set; }
    public bool IsActive { get; private set; }

    private ClassroomStudent() { }
    private ClassroomStudent(Guid classroomId, Guid studentId)
        : base(Guid.CreateVersion7(), DateTimeOffset.UtcNow)
    {
        ClassroomId = ValidateClassroomId(classroomId);
        StudentId = ValidateStudentId(studentId);
        JoinedAt = CreatedAt;
        IsActive = true;
    }

    public static ClassroomStudent Create(Guid classroomId, Guid studentId)
    {
        return new ClassroomStudent(classroomId, studentId);
    }
    private static Guid ValidateClassroomId(Guid classroomId)
    {
        if (classroomId == Guid.Empty)
        {
            throw new DomainException("Classroom ID cannot be empty.");
        }
        return classroomId;
    }
    private static Guid ValidateStudentId(Guid studentId)
    {
        if (studentId == Guid.Empty)
        {
            throw new DomainException("Student ID cannot be empty.");
        }
        return studentId;
    }
    public void Leave()
    {
        if (!IsActive)
        {
            throw new DomainException("Student is not currently enrolled in the classroom.");
        }
        LeftAt = DateTimeOffset.UtcNow;
        IsActive = false;
    }
    public void Rejoin()
    {
        if (IsActive)
        {
            throw new DomainException("Student is already enrolled in the classroom.");
        }
        LeftAt = null;
        IsActive = true;
    }

}
