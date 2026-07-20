using ExamForge.Domain.Entities;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Tests.Entities;

public class AssignmentQuestionTests
{
    [Fact]
    public void Create_WithValidValues_CreatesActiveAssignmentQuestion()
    {
        // Arrange
        var assignmentId = Guid.CreateVersion7();
        var questionId = Guid.CreateVersion7();
        const int displayOrder = 1;
        const int maximumMarks = 5;

        // Act
        var assignmentQuestion = AssignmentQuestion.Create(
            assignmentId,
            questionId,
            displayOrder,
            maximumMarks);

        // Assert
        Assert.NotEqual(Guid.Empty, assignmentQuestion.Id);
        Assert.Equal(
            assignmentId,
            assignmentQuestion.AssignmentId);
        Assert.Equal(
            questionId,
            assignmentQuestion.QuestionId);
        Assert.Equal(
            displayOrder,
            assignmentQuestion.DisplayOrder);
        Assert.Equal(
            maximumMarks,
            assignmentQuestion.MaximumMarks);
        Assert.True(assignmentQuestion.IsActive);
        Assert.NotEqual(
            default,
            assignmentQuestion.CreatedAt);
    }

    [Fact]
    public void Create_WithEmptyAssignmentId_ThrowsDomainException()
    {
        // Act
        var action = () => AssignmentQuestion.Create(
            Guid.Empty,
            Guid.CreateVersion7(),
            1,
            5);

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
            1,
            5);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Question ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithDisplayOrderEqualToOne_CreatesAssignmentQuestion()
    {
        // Act
        var assignmentQuestion = AssignmentQuestion.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            1,
            5);

        // Assert
        Assert.Equal(1, assignmentQuestion.DisplayOrder);
    }

    [Fact]
    public void Create_WithDisplayOrderEqualToZero_ThrowsDomainException()
    {
        // Act
        var action = () => AssignmentQuestion.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            0,
            5);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Display order must be greater than zero.",
            exception.Message);
    }

    [Fact]
    public void Create_WithNegativeDisplayOrder_ThrowsDomainException()
    {
        // Act
        var action = () => AssignmentQuestion.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            -1,
            5);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Display order must be greater than zero.",
            exception.Message);
    }

    [Fact]
    public void Create_WithMaximumMarksEqualToOne_CreatesAssignmentQuestion()
    {
        // Act
        var assignmentQuestion = AssignmentQuestion.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            1,
            1);

        // Assert
        Assert.Equal(1, assignmentQuestion.MaximumMarks);
    }

    [Fact]
    public void Create_WithMaximumMarksEqualToZero_ThrowsDomainException()
    {
        // Act
        var action = () => AssignmentQuestion.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            1,
            0);

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
        var action = () => AssignmentQuestion.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            1,
            -1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Maximum marks must be greater than zero.",
            exception.Message);
    }

    [Fact]
    public void ChangeDisplayOrder_WithValidOrder_ChangesDisplayOrder()
    {
        // Arrange
        var assignmentQuestion = CreateValidAssignmentQuestion();

        // Act
        assignmentQuestion.ChangeDisplayOrder(3);

        // Assert
        Assert.Equal(3, assignmentQuestion.DisplayOrder);
    }

    [Fact]
    public void ChangeDisplayOrder_WithOrderEqualToOne_ChangesDisplayOrder()
    {
        // Arrange
        var assignmentQuestion = AssignmentQuestion.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            4,
            5);

        // Act
        assignmentQuestion.ChangeDisplayOrder(1);

        // Assert
        Assert.Equal(1, assignmentQuestion.DisplayOrder);
    }

    [Fact]
    public void ChangeDisplayOrder_WithZero_DoesNotChangeDisplayOrder()
    {
        // Arrange
        var assignmentQuestion = CreateValidAssignmentQuestion();
        var originalDisplayOrder =
            assignmentQuestion.DisplayOrder;

        // Act
        var action = () =>
            assignmentQuestion.ChangeDisplayOrder(0);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Display order must be greater than zero.",
            exception.Message);
        Assert.Equal(
            originalDisplayOrder,
            assignmentQuestion.DisplayOrder);
    }

    [Fact]
    public void ChangeDisplayOrder_WithNegativeOrder_DoesNotChangeDisplayOrder()
    {
        // Arrange
        var assignmentQuestion = CreateValidAssignmentQuestion();
        var originalDisplayOrder =
            assignmentQuestion.DisplayOrder;

        // Act
        var action = () =>
            assignmentQuestion.ChangeDisplayOrder(-1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Display order must be greater than zero.",
            exception.Message);
        Assert.Equal(
            originalDisplayOrder,
            assignmentQuestion.DisplayOrder);
    }

    [Fact]
    public void ChangeMaximumMarks_WithValidMarks_ChangesMaximumMarks()
    {
        // Arrange
        var assignmentQuestion = CreateValidAssignmentQuestion();

        // Act
        assignmentQuestion.ChangeMaximumMarks(10);

        // Assert
        Assert.Equal(10, assignmentQuestion.MaximumMarks);
    }

    [Fact]
    public void ChangeMaximumMarks_WithMarksEqualToOne_ChangesMaximumMarks()
    {
        // Arrange
        var assignmentQuestion = CreateValidAssignmentQuestion();

        // Act
        assignmentQuestion.ChangeMaximumMarks(1);

        // Assert
        Assert.Equal(1, assignmentQuestion.MaximumMarks);
    }

    [Fact]
    public void ChangeMaximumMarks_WithZero_DoesNotChangeMaximumMarks()
    {
        // Arrange
        var assignmentQuestion = CreateValidAssignmentQuestion();
        var originalMaximumMarks =
            assignmentQuestion.MaximumMarks;

        // Act
        var action = () =>
            assignmentQuestion.ChangeMaximumMarks(0);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Maximum marks must be greater than zero.",
            exception.Message);
        Assert.Equal(
            originalMaximumMarks,
            assignmentQuestion.MaximumMarks);
    }

    [Fact]
    public void ChangeMaximumMarks_WithNegativeMarks_DoesNotChangeMaximumMarks()
    {
        // Arrange
        var assignmentQuestion = CreateValidAssignmentQuestion();
        var originalMaximumMarks =
            assignmentQuestion.MaximumMarks;

        // Act
        var action = () =>
            assignmentQuestion.ChangeMaximumMarks(-1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Maximum marks must be greater than zero.",
            exception.Message);
        Assert.Equal(
            originalMaximumMarks,
            assignmentQuestion.MaximumMarks);
    }

    [Fact]
    public void Deactivate_WhenAssignmentQuestionIsActive_SetsAssignmentQuestionToInactive()
    {
        // Arrange
        var assignmentQuestion = CreateValidAssignmentQuestion();

        // Act
        assignmentQuestion.Deactivate();

        // Assert
        Assert.False(assignmentQuestion.IsActive);
    }

    [Fact]
    public void Deactivate_WhenAssignmentQuestionIsAlreadyInactive_RemainsInactive()
    {
        // Arrange
        var assignmentQuestion = CreateValidAssignmentQuestion();
        assignmentQuestion.Deactivate();

        // Act
        assignmentQuestion.Deactivate();

        // Assert
        Assert.False(assignmentQuestion.IsActive);
    }

    [Fact]
    public void Activate_WhenAssignmentQuestionIsInactive_SetsAssignmentQuestionToActive()
    {
        // Arrange
        var assignmentQuestion = CreateValidAssignmentQuestion();
        assignmentQuestion.Deactivate();

        // Act
        assignmentQuestion.Activate();

        // Assert
        Assert.True(assignmentQuestion.IsActive);
    }

    [Fact]
    public void Activate_WhenAssignmentQuestionIsAlreadyActive_RemainsActive()
    {
        // Arrange
        var assignmentQuestion = CreateValidAssignmentQuestion();

        // Act
        assignmentQuestion.Activate();

        // Assert
        Assert.True(assignmentQuestion.IsActive);
    }

    private static AssignmentQuestion CreateValidAssignmentQuestion()
    {
        return AssignmentQuestion.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            2,
            5);
    }
}