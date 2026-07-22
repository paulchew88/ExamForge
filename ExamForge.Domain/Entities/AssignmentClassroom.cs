using ExamForge.Domain.Common;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Entities;

public class AssignmentClassroom : Entity
{
    public Guid AssignmentId { get; private set; }
    public Guid ClassroomId { get; private set; }
    public DateTimeOffset AssignedAt { get; private set; }
    public DateTimeOffset? UnassignedAt { get; private set; }
    public bool IsActive { get; private set; }

    private AssignmentClassroom() { }
    private AssignmentClassroom(Guid assignmentId, Guid classroomId)
        : base(Guid.CreateVersion7(), DateTimeOffset.UtcNow)
    {
        AssignmentId = ValidateAssignmentId(assignmentId);
        ClassroomId = ValidateClassroomId(classroomId);
        AssignedAt = CreatedAt;
        IsActive = true;
    }

    public static AssignmentClassroom Create(Guid assignmentId, Guid classroomId)
    {
        return new AssignmentClassroom(assignmentId, classroomId);
    }

    private static Guid ValidateAssignmentId(Guid assignmentId)
    {
        if (assignmentId == Guid.Empty)
        {
            throw new DomainException("Assignment ID cannot be empty.");
        }
        return assignmentId;
    }

    private static Guid ValidateClassroomId(Guid classroomId)
    {
        if (classroomId == Guid.Empty)
        {
            throw new DomainException("Classroom ID cannot be empty.");
        }
        return classroomId;
    }

    public void Unassign()
    {
        if (!IsActive)
        {
            throw new DomainException("Assignment is not currently assigned to the classroom.");
        }
        UnassignedAt = DateTimeOffset.UtcNow;
        IsActive = false;
    }

    public void Reassign()
    {
        if (IsActive)
        {
            throw new DomainException("Assignment is already assigned to the classroom.");
        }
        UnassignedAt = null;
        IsActive = true;
    }
}