using ExamForge.Domain.Common;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Entities;

public class AssignmentQuestion : Entity
{
    public Guid AssignmentId { get; private set; }
    public Guid QuestionId { get; private set; }
    public int Order { get; private set; }
    public int MaximumMarks { get; private set; }


    private AssignmentQuestion() { }

    private AssignmentQuestion(
        Guid assignmentId,
        Guid questionId,
        int order,
        int maximumMarks)
        : base(Guid.CreateVersion7(), DateTimeOffset.UtcNow)
    {
        AssignmentId = ValidateAssignmentId(assignmentId);
        QuestionId = ValidateQuestionId(questionId);
        Order = ValidateDisplayOrder(order);
        MaximumMarks = ValidateMaximumMarks(maximumMarks);
    }

    public static AssignmentQuestion Create(
        Guid assignmentId,
        Guid questionId,
        int order,
        int maximumMarks)
    {
        return new AssignmentQuestion(
            assignmentId,
            questionId,
            order,
            maximumMarks);
    }


    public void ChangeOrder(int newDisplayOrder)
    {
        Order = ValidateDisplayOrder(newDisplayOrder);
    }

    private static Guid ValidateAssignmentId(Guid assignmentId)
    {
        if (assignmentId == Guid.Empty)
        {
            throw new DomainException("Assignment ID cannot be empty.");
        }
        return assignmentId;
    }
    private static Guid ValidateQuestionId(Guid questionId)
    {
        if (questionId == Guid.Empty)
        {
            throw new DomainException("Question ID cannot be empty.");
        }
        return questionId;
    }
    private static int ValidateDisplayOrder(int displayOrder)
    {
        if (displayOrder < 1)
        {
            throw new DomainException("Display order must be greater than zero.");
        }
        return displayOrder;
    }
    private static int ValidateMaximumMarks(int maximumMarks)
    {
        if (maximumMarks < 1)
        {
            throw new DomainException("Maximum marks must be greater than zero.");
        }
        return maximumMarks;
    }

    public void ChangeMaximumMarks(int newMaximumMarks)
    {
        MaximumMarks = ValidateMaximumMarks(newMaximumMarks);
    }

}
