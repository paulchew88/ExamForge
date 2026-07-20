using ExamForge.Domain.Entities;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Tests.Entities;

public class QuestionTests
{
    [Fact]
    public void Create_WithValidValues_CreatesActiveQuestion()
    {
        // Arrange
        var topicId = Guid.CreateVersion7();
        const string prompt =
            "Explain one difference between RAM and secondary storage.";
        const string markScheme =
            "Award one mark for identifying that RAM is volatile.";
        const int maximumMarks = 1;
        const int displayOrder = 1;

        // Act
        var question = Question.Create(
            topicId,
            prompt,
            markScheme,
            maximumMarks,
            displayOrder);

        // Assert
        Assert.NotEqual(Guid.Empty, question.Id);
        Assert.Equal(topicId, question.TopicId);
        Assert.Equal(prompt, question.Prompt);
        Assert.Equal(markScheme, question.MarkScheme);
        Assert.Equal(maximumMarks, question.MaximumMarks);
        Assert.Equal(displayOrder, question.DisplayOrder);
        Assert.True(question.IsActive);
        Assert.NotEqual(default, question.CreatedAt);
    }

    [Fact]
    public void Create_WithEmptyTopicId_ThrowsDomainException()
    {
        // Act
        var action = () => Question.Create(
            Guid.Empty,
            "Explain what is meant by an algorithm.",
            "Award one mark for a correct definition.",
            1,
            1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Topic ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithEmptyPrompt_ThrowsDomainException()
    {
        // Act
        var action = () => Question.Create(
            Guid.CreateVersion7(),
            string.Empty,
            "Award one mark for a correct answer.",
            1,
            1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Question prompt cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithWhitespacePrompt_ThrowsDomainException()
    {
        // Act
        var action = () => Question.Create(
            Guid.CreateVersion7(),
            "   ",
            "Award one mark for a correct answer.",
            1,
            1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Question prompt cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithSurroundingWhitespace_TrimsPrompt()
    {
        // Act
        var question = Question.Create(
            Guid.CreateVersion7(),
            "  Explain what is meant by abstraction.  ",
            "Award one mark for a correct definition.",
            1,
            1);

        // Assert
        Assert.Equal(
            "Explain what is meant by abstraction.",
            question.Prompt);
    }

    [Fact]
    public void Create_WithPromptAtMaximumLength_CreatesQuestion()
    {
        // Arrange
        var prompt = new string('A', 5000);

        // Act
        var question = Question.Create(
            Guid.CreateVersion7(),
            prompt,
            "Award one mark for a correct answer.",
            1,
            1);

        // Assert
        Assert.Equal(prompt, question.Prompt);
    }

    [Fact]
    public void Create_WithPromptLongerThanMaximum_ThrowsDomainException()
    {
        // Arrange
        var prompt = new string('A', 5001);

        // Act
        var action = () => Question.Create(
            Guid.CreateVersion7(),
            prompt,
            "Award one mark for a correct answer.",
            1,
            1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Question prompt cannot exceed 5000 characters.",
            exception.Message);
    }

    [Fact]
    public void Create_WithEmptyMarkScheme_ThrowsDomainException()
    {
        // Act
        var action = () => Question.Create(
            Guid.CreateVersion7(),
            "Explain what is meant by decomposition.",
            string.Empty,
            1,
            1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Mark scheme cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithWhitespaceMarkScheme_ThrowsDomainException()
    {
        // Act
        var action = () => Question.Create(
            Guid.CreateVersion7(),
            "Explain what is meant by decomposition.",
            "   ",
            1,
            1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Mark scheme cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithSurroundingWhitespace_TrimsMarkScheme()
    {
        // Act
        var question = Question.Create(
            Guid.CreateVersion7(),
            "Explain what is meant by decomposition.",
            "  Award one mark for a correct definition.  ",
            1,
            1);

        // Assert
        Assert.Equal(
            "Award one mark for a correct definition.",
            question.MarkScheme);
    }

    [Fact]
    public void Create_WithMarkSchemeAtMaximumLength_CreatesQuestion()
    {
        // Arrange
        var markScheme = new string('A', 10000);

        // Act
        var question = Question.Create(
            Guid.CreateVersion7(),
            "Explain what is meant by decomposition.",
            markScheme,
            1,
            1);

        // Assert
        Assert.Equal(markScheme, question.MarkScheme);
    }

    [Fact]
    public void Create_WithMarkSchemeLongerThanMaximum_ThrowsDomainException()
    {
        // Arrange
        var markScheme = new string('A', 10001);

        // Act
        var action = () => Question.Create(
            Guid.CreateVersion7(),
            "Explain what is meant by decomposition.",
            markScheme,
            1,
            1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Mark scheme cannot exceed 10000 characters.",
            exception.Message);
    }

    [Fact]
    public void Create_WithZeroMaximumMarks_ThrowsDomainException()
    {
        // Act
        var action = () => Question.Create(
            Guid.CreateVersion7(),
            "Explain what is meant by abstraction.",
            "Award one mark for a correct definition.",
            0,
            1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Maximum marks must be greater than zero.",
            exception.Message);
    }

    [Fact]
    public void Create_WithNegativeMaximumMarks_ThrowsDomainException()
    {
        // Act
        var action = () => Question.Create(
            Guid.CreateVersion7(),
            "Explain what is meant by abstraction.",
            "Award one mark for a correct definition.",
            -1,
            1);

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Create_WithZeroDisplayOrder_ThrowsDomainException()
    {
        // Act
        var action = () => Question.Create(
            Guid.CreateVersion7(),
            "Explain what is meant by abstraction.",
            "Award one mark for a correct definition.",
            1,
            0);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Display order cannot be less than 1.",
            exception.Message);
    }

    [Fact]
    public void Create_WithNegativeDisplayOrder_ThrowsDomainException()
    {
        // Act
        var action = () => Question.Create(
            Guid.CreateVersion7(),
            "Explain what is meant by abstraction.",
            "Award one mark for a correct definition.",
            1,
            -1);

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void UpdatePrompt_WithValidPrompt_UpdatesPrompt()
    {
        // Arrange
        var question = CreateValidQuestion();

        // Act
        question.changePrompt(
            "Explain two benefits of decomposition.");

        // Assert
        Assert.Equal(
            "Explain two benefits of decomposition.",
            question.Prompt);
    }

    [Fact]
    public void UpdatePrompt_WithSurroundingWhitespace_TrimsPrompt()
    {
        // Arrange
        var question = CreateValidQuestion();

        // Act
        question.changePrompt(
            "  Explain two benefits of decomposition.  ");

        // Assert
        Assert.Equal(
            "Explain two benefits of decomposition.",
            question.Prompt);
    }

    [Fact]
    public void UpdatePrompt_WithInvalidPrompt_DoesNotChangePrompt()
    {
        // Arrange
        var question = CreateValidQuestion();
        var originalPrompt = question.Prompt;
        var invalidPrompt = new string('A', 5001);

        // Act
        var action = () => question.changePrompt(invalidPrompt);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalPrompt, question.Prompt);
    }

    [Fact]
    public void UpdateMarkScheme_WithValidMarkScheme_UpdatesMarkScheme()
    {
        // Arrange
        var question = CreateValidQuestion();

        // Act
        question.ChangeMarkScheme(
            "Award one mark for each valid benefit.");

        // Assert
        Assert.Equal(
            "Award one mark for each valid benefit.",
            question.MarkScheme);
    }

    [Fact]
    public void UpdateMarkScheme_WithSurroundingWhitespace_TrimsMarkScheme()
    {
        // Arrange
        var question = CreateValidQuestion();

        // Act
        question.ChangeMarkScheme(
            "  Award one mark for each valid benefit.  ");

        // Assert
        Assert.Equal(
            "Award one mark for each valid benefit.",
            question.MarkScheme);
    }

    [Fact]
    public void UpdateMarkScheme_WithInvalidMarkScheme_DoesNotChangeMarkScheme()
    {
        // Arrange
        var question = CreateValidQuestion();
        var originalMarkScheme = question.MarkScheme;
        var invalidMarkScheme = new string('A', 10001);

        // Act
        var action = () =>
            question.ChangeMarkScheme(invalidMarkScheme);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalMarkScheme, question.MarkScheme);
    }

    [Fact]
    public void ChangeMaximumMarks_WithValidValue_UpdatesMaximumMarks()
    {
        // Arrange
        var question = CreateValidQuestion();

        // Act
        question.ChangeMaximumMarks(4);

        // Assert
        Assert.Equal(4, question.MaximumMarks);
    }

    [Fact]
    public void ChangeMaximumMarks_WithInvalidValue_ThrowsDomainException()
    {
        // Arrange
        var question = CreateValidQuestion();

        // Act
        var action = () => question.ChangeMaximumMarks(0);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Maximum marks must be greater than zero.",
            exception.Message);
    }

    [Fact]
    public void ChangeMaximumMarks_WithInvalidValue_DoesNotChangeMaximumMarks()
    {
        // Arrange
        var question = CreateValidQuestion();
        var originalMaximumMarks = question.MaximumMarks;

        // Act
        var action = () => question.ChangeMaximumMarks(0);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(
            originalMaximumMarks,
            question.MaximumMarks);
    }

    [Fact]
    public void ChangeDisplayOrder_WithValidValue_UpdatesDisplayOrder()
    {
        // Arrange
        var question = CreateValidQuestion();

        // Act
        question.ChangeDisplayOrder(2);

        // Assert
        Assert.Equal(2, question.DisplayOrder);
    }

    [Fact]
    public void ChangeDisplayOrder_WithInvalidValue_ThrowsDomainException()
    {
        // Arrange
        var question = CreateValidQuestion();

        // Act
        var action = () => question.ChangeDisplayOrder(0);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Display order cannot be less than 1.",
            exception.Message);
    }

    [Fact]
    public void ChangeDisplayOrder_WithInvalidValue_DoesNotChangeDisplayOrder()
    {
        // Arrange
        var question = CreateValidQuestion();
        var originalDisplayOrder = question.DisplayOrder;

        // Act
        var action = () => question.ChangeDisplayOrder(0);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(
            originalDisplayOrder,
            question.DisplayOrder);
    }

    [Fact]
    public void Deactivate_WhenQuestionIsActive_SetsQuestionToInactive()
    {
        // Arrange
        var question = CreateValidQuestion();

        // Act
        question.Deactivate();

        // Assert
        Assert.False(question.IsActive);
    }

    [Fact]
    public void Activate_WhenQuestionIsInactive_SetsQuestionToActive()
    {
        // Arrange
        var question = CreateValidQuestion();
        question.Deactivate();

        // Act
        question.Activate();

        // Assert
        Assert.True(question.IsActive);
    }

    private static Question CreateValidQuestion()
    {
        return Question.Create(
            Guid.CreateVersion7(),
            "Explain what is meant by decomposition.",
            "Award one mark for a correct definition.",
            1,
            1);
    }
}