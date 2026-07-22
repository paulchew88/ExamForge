using ExamForge.Domain.Entities;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Tests.Entities;

public class QuestionOptionTests
{
    [Fact]
    public void Create_WithValidValues_CreatesQuestionOption()
    {
        // Arrange
        var questionId = Guid.CreateVersion7();
        var beforeCreation = DateTimeOffset.UtcNow;

        // Act
        var option = QuestionOption.Create(
            questionId,
            "Transmission Control Protocol",
            isCorrect: true,
            order: 1);

        var afterCreation = DateTimeOffset.UtcNow;

        // Assert
        Assert.NotEqual(Guid.Empty, option.Id);
        Assert.Equal(questionId, option.QuestionId);
        Assert.Equal("Transmission Control Protocol", option.Text);
        Assert.True(option.IsCorrect);
        Assert.Equal(1, option.Order);

        Assert.InRange(
            option.CreatedAt,
            beforeCreation,
            afterCreation);
    }

    [Fact]
    public void Create_WithIncorrectOption_CreatesIncorrectOption()
    {
        // Act
        var option = QuestionOption.Create(
            Guid.CreateVersion7(),
            "User Datagram Protocol",
            isCorrect: false,
            order: 2);

        // Assert
        Assert.False(option.IsCorrect);
    }

    [Fact]
    public void Create_WithEmptyQuestionId_ThrowsDomainException()
    {
        // Act
        var action = () => QuestionOption.Create(
            Guid.Empty,
            "Valid option",
            isCorrect: false,
            order: 1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Question ID cannot be empty.",
            exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyText_ThrowsDomainException(string? text)
    {
        // Act
        var action = () => QuestionOption.Create(
            Guid.CreateVersion7(),
            text!,
            isCorrect: false,
            order: 1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Option text cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_TrimsText()
    {
        // Act
        var option = QuestionOption.Create(
            Guid.CreateVersion7(),
            "  Transmission Control Protocol  ",
            isCorrect: true,
            order: 1);

        // Assert
        Assert.Equal(
            "Transmission Control Protocol",
            option.Text);
    }

    [Fact]
    public void Create_WithTextAtMaximumLength_Succeeds()
    {
        // Arrange
        var text = new string(
            'A',
            QuestionOption.MaxTextLength);

        // Act
        var option = QuestionOption.Create(
            Guid.CreateVersion7(),
            text,
            isCorrect: false,
            order: 1);

        // Assert
        Assert.Equal(text, option.Text);
    }

    [Fact]
    public void Create_WithTextOverMaximumLength_ThrowsDomainException()
    {
        // Arrange
        var text = new string(
            'A',
            QuestionOption.MaxTextLength + 1);

        // Act
        var action = () => QuestionOption.Create(
            Guid.CreateVersion7(),
            text,
            isCorrect: false,
            order: 1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            $"Option text cannot exceed {QuestionOption.MaxTextLength} characters.",
            exception.Message);
    }

    [Fact]
    public void Create_TrimsTextBeforeCheckingMaximumLength()
    {
        // Arrange
        var text =
            $"  {new string('A', QuestionOption.MaxTextLength)}  ";

        // Act
        var option = QuestionOption.Create(
            Guid.CreateVersion7(),
            text,
            isCorrect: false,
            order: 1);

        // Assert
        Assert.Equal(
            QuestionOption.MaxTextLength,
            option.Text.Length);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Create_WithInvalidOrder_ThrowsDomainException(int order)
    {
        // Act
        var action = () => QuestionOption.Create(
            Guid.CreateVersion7(),
            "Valid option",
            isCorrect: false,
            order);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Order must be greater than zero.",
            exception.Message);
    }

    [Fact]
    public void UpdateText_WithValidText_ReplacesText()
    {
        // Arrange
        var option = CreateValidOption();

        // Act
        option.UpdateText("Updated option");

        // Assert
        Assert.Equal("Updated option", option.Text);
    }

    [Fact]
    public void UpdateText_TrimsText()
    {
        // Arrange
        var option = CreateValidOption();

        // Act
        option.UpdateText("  Updated option  ");

        // Assert
        Assert.Equal("Updated option", option.Text);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateText_WithEmptyText_ThrowsAndPreservesState(
        string? newText)
    {
        // Arrange
        var option = CreateValidOption();
        var originalText = option.Text;

        // Act
        var action = () => option.UpdateText(newText!);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Option text cannot be empty.",
            exception.Message);

        Assert.Equal(originalText, option.Text);
    }

    [Fact]
    public void UpdateText_WithTextOverMaximumLength_ThrowsAndPreservesState()
    {
        // Arrange
        var option = CreateValidOption();
        var originalText = option.Text;

        var newText = new string(
            'A',
            QuestionOption.MaxTextLength + 1);

        // Act
        var action = () => option.UpdateText(newText);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            $"Option text cannot exceed {QuestionOption.MaxTextLength} characters.",
            exception.Message);

        Assert.Equal(originalText, option.Text);
    }

    [Fact]
    public void UpdateText_DoesNotChangeOtherProperties()
    {
        // Arrange
        var option = CreateValidOption();

        var originalQuestionId = option.QuestionId;
        var originalIsCorrect = option.IsCorrect;
        var originalOrder = option.Order;
        var originalCreatedAt = option.CreatedAt;

        // Act
        option.UpdateText("Updated option");

        // Assert
        Assert.Equal(originalQuestionId, option.QuestionId);
        Assert.Equal(originalIsCorrect, option.IsCorrect);
        Assert.Equal(originalOrder, option.Order);
        Assert.Equal(originalCreatedAt, option.CreatedAt);
    }

    [Fact]
    public void MarkAsCorrect_WhenIncorrect_MarksOptionAsCorrect()
    {
        // Arrange
        var option = QuestionOption.Create(
            Guid.CreateVersion7(),
            "Option",
            isCorrect: false,
            order: 1);

        // Act
        option.MarkAsCorrect();

        // Assert
        Assert.True(option.IsCorrect);
    }

    [Fact]
    public void MarkAsCorrect_WhenAlreadyCorrect_RemainsCorrect()
    {
        // Arrange
        var option = QuestionOption.Create(
            Guid.CreateVersion7(),
            "Option",
            isCorrect: true,
            order: 1);

        // Act
        option.MarkAsCorrect();

        // Assert
        Assert.True(option.IsCorrect);
    }

    [Fact]
    public void MarkAsIncorrect_WhenCorrect_MarksOptionAsIncorrect()
    {
        // Arrange
        var option = QuestionOption.Create(
            Guid.CreateVersion7(),
            "Option",
            isCorrect: true,
            order: 1);

        // Act
        option.MarkAsIncorrect();

        // Assert
        Assert.False(option.IsCorrect);
    }

    [Fact]
    public void MarkAsIncorrect_WhenAlreadyIncorrect_RemainsIncorrect()
    {
        // Arrange
        var option = QuestionOption.Create(
            Guid.CreateVersion7(),
            "Option",
            isCorrect: false,
            order: 1);

        // Act
        option.MarkAsIncorrect();

        // Assert
        Assert.False(option.IsCorrect);
    }

    [Fact]
    public void ChangeOrder_WithValidOrder_ChangesOrder()
    {
        // Arrange
        var option = CreateValidOption();

        // Act
        option.ChangeOrder(3);

        // Assert
        Assert.Equal(3, option.Order);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void ChangeOrder_WithInvalidOrder_ThrowsAndPreservesState(
        int newOrder)
    {
        // Arrange
        var option = CreateValidOption();
        var originalOrder = option.Order;

        // Act
        var action = () => option.ChangeOrder(newOrder);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Order must be greater than zero.",
            exception.Message);

        Assert.Equal(originalOrder, option.Order);
    }

    [Fact]
    public void ChangeOrder_DoesNotChangeOtherProperties()
    {
        // Arrange
        var option = CreateValidOption();

        var originalQuestionId = option.QuestionId;
        var originalText = option.Text;
        var originalIsCorrect = option.IsCorrect;
        var originalCreatedAt = option.CreatedAt;

        // Act
        option.ChangeOrder(4);

        // Assert
        Assert.Equal(originalQuestionId, option.QuestionId);
        Assert.Equal(originalText, option.Text);
        Assert.Equal(originalIsCorrect, option.IsCorrect);
        Assert.Equal(originalCreatedAt, option.CreatedAt);
    }

    [Fact]
    public void CompleteWorkflow_CreateUpdateCorrectnessAndOrder_Works()
    {
        // Arrange
        var option = QuestionOption.Create(
            Guid.CreateVersion7(),
            "Initial option",
            isCorrect: false,
            order: 1);

        // Act
        option.UpdateText("Updated option");
        option.MarkAsCorrect();
        option.ChangeOrder(2);

        // Assert
        Assert.Equal("Updated option", option.Text);
        Assert.True(option.IsCorrect);
        Assert.Equal(2, option.Order);

        // Act
        option.MarkAsIncorrect();

        // Assert
        Assert.False(option.IsCorrect);
    }

    private static QuestionOption CreateValidOption()
    {
        return QuestionOption.Create(
            Guid.CreateVersion7(),
            "Initial option",
            isCorrect: false,
            order: 1);
    }
}