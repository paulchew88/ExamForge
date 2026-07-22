using ExamForge.Domain.Common;
using ExamForge.Domain.Enums;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Entities;

public class Submission : Entity
{
    public Guid AssignmentId { get; private set; }
    public Guid StudentId { get; private set; }
    public SubmissionStatus Status { get; private set; }
    public DateTimeOffset? SubmittedAt { get; private set; }
    public DateTimeOffset? MarkedAt { get; private set; }
    public DateTimeOffset? ReleasedAt { get; private set; }

    private Submission()
    {
        Status = SubmissionStatus.Draft;
    }
    private Submission(
        Guid assignmentId,
        Guid studentId,
        SubmissionStatus status,
        DateTimeOffset? submittedAt,
        DateTimeOffset? markedAt,
        DateTimeOffset? releasedAt)
        : base(Guid.CreateVersion7(), DateTimeOffset.UtcNow)
    {
        AssignmentId = assignmentId;
        StudentId = studentId;
        Status = status;
        SubmittedAt = submittedAt;
        MarkedAt = markedAt;
        ReleasedAt = releasedAt;
    }

    public static Submission Create(Guid assignmentId, Guid studentId)
    {
        return new Submission(
            ValidateAssignmentId(assignmentId),
            ValidateStudentId(studentId),
            ValidateStatus(SubmissionStatus.Draft),
            null,
            null,
            null
           );
    }
    private static Guid ValidateAssignmentId(Guid assignmentId)
    {
        if (assignmentId == Guid.Empty)
        {
            throw new DomainException("Assignment ID cannot be empty.");
        }
        return assignmentId;
    }
    private static Guid ValidateStudentId(Guid studentId)
    {
        if (studentId == Guid.Empty)
        {
            throw new DomainException("Student ID cannot be empty.");
        }
        return studentId;
    }
    private static SubmissionStatus ValidateStatus(SubmissionStatus status)
    {
        if (!Enum.IsDefined(typeof(SubmissionStatus), status))
        {
            throw new DomainException("Invalid submission status.");
        }
        return status;
    }



    public void Submit()
    {
        if (Status != SubmissionStatus.Draft)
        {
            throw new DomainException("Only draft submissions can be submitted.");
        }
        Status = SubmissionStatus.Submitted;
        SubmittedAt = DateTimeOffset.UtcNow;
    }

    public void Mark()
    {
        if (Status != SubmissionStatus.Submitted)
        {
            throw new DomainException("Only submitted submissions can be marked.");
        }
        Status = SubmissionStatus.Marked;
        MarkedAt = DateTimeOffset.UtcNow;
    }
    public void ReturnToDraft()
    {
        if (Status != SubmissionStatus.Submitted)
        {
            throw new DomainException("Only submitted submissions can be returned to draft.");
        }
        Status = SubmissionStatus.Draft;
        SubmittedAt = null;
    }

    public void Release()
    {
        if (Status != SubmissionStatus.Marked)
        {
            throw new DomainException("Only marked submissions can be released.");
        }
        Status = SubmissionStatus.Released;
        ReleasedAt = DateTimeOffset.UtcNow;
    }




}
