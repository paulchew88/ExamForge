using ExamForge.Domain.Common;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Entities;

public class SubmissionAnswer : Entity
{
    public const int MaxAnswerLength = 4_000;
    public const int MaxFeedbackLength = 4_000;

    public Guid SubmissionId { get; private set; }
    public Guid AssignmentQuestionId { get; private set; }
    public string Answer { get; private set; }
    public int? AwardedMarks { get; private set; }
    public string? Feedback { get; private set; }
    public DateTimeOffset LastUpdatedAt { get; private set; }
    public DateTimeOffset? MarkedAt { get; private set; }

    private SubmissionAnswer()
    {
        Answer = string.Empty;
    }

    private SubmissionAnswer(
        Guid submissionId,
        Guid assignmentQuestionId,
        string answer)
        : base(Guid.CreateVersion7(), DateTimeOffset.UtcNow)
    {
        SubmissionId = ValidateSubmissionId(submissionId);
        AssignmentQuestionId =
            ValidateAssignmentQuestionId(assignmentQuestionId);
        Answer = ValidateAnswer(answer);

        LastUpdatedAt = CreatedAt;
    }

    public static SubmissionAnswer Create(
        Guid submissionId,
        Guid assignmentQuestionId,
        string answer)
    {
        return new SubmissionAnswer(
            submissionId,
            assignmentQuestionId,
            answer);
    }

    public void UpdateAnswer(string newAnswer)
    {
        var validatedAnswer = ValidateAnswer(newAnswer);

        Answer = validatedAnswer;
        LastUpdatedAt = DateTimeOffset.UtcNow;

        ClearMark();
    }

    public void Mark(int awardedMarks, string? feedback)
    {
        var validatedMarks = ValidateAwardedMarks(awardedMarks);
        var validatedFeedback = ValidateFeedback(feedback);

        AwardedMarks = validatedMarks;
        Feedback = validatedFeedback;
        MarkedAt = DateTimeOffset.UtcNow;
    }

    public void ClearMark()
    {
        AwardedMarks = null;
        Feedback = null;
        MarkedAt = null;
    }

    private static Guid ValidateSubmissionId(Guid submissionId)
    {
        if (submissionId == Guid.Empty)
        {
            throw new DomainException(
                "Submission ID cannot be empty.");
        }

        return submissionId;
    }

    private static Guid ValidateAssignmentQuestionId(
        Guid assignmentQuestionId)
    {
        if (assignmentQuestionId == Guid.Empty)
        {
            throw new DomainException(
                "Assignment question ID cannot be empty.");
        }

        return assignmentQuestionId;
    }

    private static string ValidateAnswer(string answer)
    {
        if (string.IsNullOrWhiteSpace(answer))
        {
            throw new DomainException(
                "Answer cannot be empty.");
        }

        var trimmedAnswer = answer.Trim();

        if (trimmedAnswer.Length > MaxAnswerLength)
        {
            throw new DomainException(
                $"Answer cannot exceed {MaxAnswerLength} characters.");
        }

        return trimmedAnswer;
    }

    private static int ValidateAwardedMarks(int awardedMarks)
    {
        if (awardedMarks < 0)
        {
            throw new DomainException(
                "Awarded marks cannot be negative.");
        }

        return awardedMarks;
    }

    private static string? ValidateFeedback(string? feedback)
    {
        if (string.IsNullOrWhiteSpace(feedback))
        {
            return null;
        }

        var trimmedFeedback = feedback.Trim();

        if (trimmedFeedback.Length > MaxFeedbackLength)
        {
            throw new DomainException(
                $"Feedback cannot exceed {MaxFeedbackLength} characters.");
        }

        return trimmedFeedback;
    }
}