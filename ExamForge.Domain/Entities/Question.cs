using ExamForge.Domain.Common;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Entities;

public class Question : Entity
{
    private const int MaxPromptLength = 5000;
    private const int MaxMarkSchemeLength = 10000;

    public Guid TopicId { get; private set; }

    public string Prompt { get; private set; }

    public string MarkScheme { get; private set; }

    public int MaximumMarks { get; private set; }

    public int DisplayOrder { get; private set; }

    public bool IsActive { get; private set; }

    private Question()
    {
        Prompt = string.Empty;
        MarkScheme = string.Empty;
    }
    private Question(
        Guid id,
        Guid topicId,
        string prompt,
        string markScheme,
        int maximumMarks,
        int displayOrder,
        DateTimeOffset createdAt)
        : base(id, createdAt)
    {

        TopicId = topicId;
        Prompt = prompt;
        MarkScheme = markScheme;
        MaximumMarks = maximumMarks;
        DisplayOrder = displayOrder;
        IsActive = true;
    }
    public static Question Create(
        Guid topicId,
        string prompt,
        string markScheme,
        int maximumMarks,
        int displayOrder)
    {
        return new Question(
            Guid.CreateVersion7(),
            ValidateTopicId(topicId),
            ValidatePrompt(prompt),
            ValidateMarkScheme(markScheme),
            ValidateMaximumMarks(maximumMarks),
            ValidateDisplayOrder(displayOrder),
            DateTimeOffset.UtcNow);
    }
    private static Guid ValidateTopicId(Guid topicId)
    {
        if (topicId == Guid.Empty)
        {
            throw new DomainException(
                "Topic ID cannot be empty.");
        }

        return topicId;
    }
    private static string ValidatePrompt(string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
        {
            throw new DomainException("Question prompt cannot be empty.");
        }
        var trimmedPrompt = prompt.Trim();

        if (trimmedPrompt.Length > MaxPromptLength)
        {
            throw new DomainException($"Question prompt cannot exceed {MaxPromptLength} characters.");
        }
        return trimmedPrompt;
    }
    private static string ValidateMarkScheme(string markScheme)
    {
        if (string.IsNullOrWhiteSpace(markScheme))
        {
            throw new DomainException("Mark scheme cannot be empty.");
        }
        var trimmedMarkScheme = markScheme.Trim();
        if (trimmedMarkScheme.Length > MaxMarkSchemeLength)
        {
            throw new DomainException($"Mark scheme cannot exceed {MaxMarkSchemeLength} characters.");
        }
        return trimmedMarkScheme;
    }
    private static int ValidateMaximumMarks(int maximumMarks)
    {
        if (maximumMarks <= 0)
        {
            throw new DomainException("Maximum marks must be greater than zero.");
        }
        return maximumMarks;
    }
    private static int ValidateDisplayOrder(int displayOrder)
    {
        if (displayOrder < 1)
        {
            throw new DomainException("Display order cannot be less than 1.");
        }
        return displayOrder;
    }


    public void changePrompt(string newPrompt)
    {
        Prompt = ValidatePrompt(newPrompt);
    }
    public void ChangeMarkScheme(string newMarkScheme)
    {
        MarkScheme = ValidateMarkScheme(newMarkScheme);
    }
    public void ChangeMaximumMarks(int newMaximumMarks)
    {
        MaximumMarks = ValidateMaximumMarks(newMaximumMarks);
    }
    public void ChangeDisplayOrder(int newDisplayOrder)
    {
        DisplayOrder = ValidateDisplayOrder(newDisplayOrder);
    }
    public void Activate()
    {
        IsActive = true;
    }
    public void Deactivate()
    {
        IsActive = false;
    }
    public void ChangeTopic(Guid newTopicId)
    {
        TopicId = ValidateTopicId(newTopicId);
    }

}
