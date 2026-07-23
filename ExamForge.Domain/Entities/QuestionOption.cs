using ExamForge.Domain.Common;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Entities;

public class QuestionOption : Entity
{
    public const int MaxTextLength = 1_000;

    public Guid QuestionId { get; private set; }
    public string Text { get; private set; } = null!;
    public bool IsCorrect { get; private set; }
    public int DisplayOrder { get; private set; }

    private QuestionOption() { }

    private QuestionOption(
        Guid questionId,
        string text,
        bool isCorrect,
        int order)
        : base(Guid.CreateVersion7(), DateTimeOffset.UtcNow)
    {
        QuestionId = ValidateQuestionId(questionId);
        Text = ValidateText(text);
        IsCorrect = isCorrect;
        DisplayOrder = ValidateOrder(order);
    }

    public static QuestionOption Create(
        Guid questionId,
        string text,
        bool isCorrect,
        int order)
    {
        return new QuestionOption(
            questionId,
            text,
            isCorrect,
            order);
    }

    public void UpdateText(string newText)
    {
        Text = ValidateText(newText);
    }

    public void MarkAsCorrect()
    {
        IsCorrect = true;
    }

    public void MarkAsIncorrect()
    {
        IsCorrect = false;
    }

    public void ChangeOrder(int newDisplayOrder)
    {
        DisplayOrder = ValidateOrder(newDisplayOrder);
    }

    private static Guid ValidateQuestionId(Guid questionId)
    {
        if (questionId == Guid.Empty)
        {
            throw new DomainException(
                "Question ID cannot be empty.");
        }

        return questionId;
    }

    private static string ValidateText(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new DomainException(
                "Option text cannot be empty.");
        }

        var trimmedText = text.Trim();

        if (trimmedText.Length > MaxTextLength)
        {
            throw new DomainException(
                $"Option text cannot exceed {MaxTextLength} characters.");
        }

        return trimmedText;
    }

    private static int ValidateOrder(int order)
    {
        if (order <= 0)
        {
            throw new DomainException(
                "Order must be greater than zero.");
        }

        return order;
    }
}