using ExamForge.Domain.Entities;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Tests.Entities;

public class AssignmentQuestionTests
{
    [Fact]
    public void Create_WithValidValues_CreatesAssignmentQuestion()
    {
        // Arrange
        var assignmentId = Guid.CreateVersion7();
        var questionId = Guid.CreateVersion7();

        var beforeCreation = DateTimeOffset.UtcNow;

        // Act
        var assignmentQuestion = AssignmentQuestion.Create(
            assignmentId,
            questionId,
            order: 1,
            maximumMarks: 5);

        var afterCreation = DateTimeOffset.UtcNow;

        // Assert
        Assert.NotEqual(Guid.Empty, assignmentQuestion.Id);
        Assert.Equal(assignmentId, assignmentQuestion.AssignmentId);
        Assert.Equal(questionId, assignmentQuestion.QuestionId);
        Assert.Equal(1, assignmentQuestion.Order);
        Assert.Equal(5, assignmentQuestion.MaximumMarks);

        Assert.InRange(
            assignmentQuestion.CreatedAt,
            beforeCreation,
            afterCreation);
    }

    [Fact]
    public void Create_WithEmptyAssignmentId_ThrowsDomainException()
    {
        // Act
        var action = () => AssignmentQuestion.Create(
            Guid.Empty,
            Guid.CreateVersion7(),
            order: 1,
            maximumMarks: 5);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Assignment ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithEmptyQuestionId_ThrowsDomainException()
    {
        // Act
        var action = () => AssignmentQuestion.Create(
            Guid.CreateVersion7(),
            Guid.Empty,
            order: 1,
            maximumMarks: 5);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Question ID cannot be empty.",
            exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Create_WithInvalidOrder_ThrowsDomainException(int order)
    {
        // Act
        var action = () => AssignmentQuestion.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            order,
            maximumMarks: 5);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Order must be greater than zero.",
            exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Create_WithInvalidMaximumMarks_ThrowsDomainException(
        int maximumMarks)
    {
        // Act
        var action = () => AssignmentQuestion.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            order: 1,
            maximumMarks);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Maximum marks must be greater than zero.",
            exception.Message);
    }
}