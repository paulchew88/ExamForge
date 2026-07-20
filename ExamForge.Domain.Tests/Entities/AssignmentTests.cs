using ExamForge.Domain.Entities;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Tests.Entities;

public class AssignmentTests
{
    [Fact]
    public void Create_WithValidValues_CreatesDraftAssignment()
    {
        // Arrange
        var classroomId = Guid.CreateVersion7();
        const string title = "Networks Assessment";
        const string instructions =
            "Answer every question and show your calculations.";
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);
        var dueAt = opensAt.AddDays(7);

        // Act
        var assignment = Assignment.Create(
            classroomId,
            title,
            instructions,
            opensAt,
            dueAt);

        // Assert
        Assert.NotEqual(Guid.Empty, assignment.Id);
        Assert.Equal(classroomId, assignment.ClassroomId);
        Assert.Equal(title, assignment.Title);
        Assert.Equal(instructions, assignment.Instructions);
        Assert.Equal(opensAt, assignment.OpensAt);
        Assert.Equal(dueAt, assignment.DueAt);
        Assert.False(assignment.IsPublished);
        Assert.NotEqual(default, assignment.CreatedAt);
    }

    [Fact]
    public void Create_WithEmptyClassroomId_ThrowsDomainException()
    {
        // Arrange
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);
        var dueAt = opensAt.AddDays(7);

        // Act
        var action = () => Assignment.Create(
            Guid.Empty,
            "Networks Assessment",
            "Answer every question.",
            opensAt,
            dueAt);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Classroom ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithEmptyTitle_ThrowsDomainException()
    {
        // Arrange
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);
        var dueAt = opensAt.AddDays(7);

        // Act
        var action = () => Assignment.Create(
            Guid.CreateVersion7(),
            string.Empty,
            "Answer every question.",
            opensAt,
            dueAt);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Assignment title cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithWhitespaceTitle_ThrowsDomainException()
    {
        // Arrange
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);
        var dueAt = opensAt.AddDays(7);

        // Act
        var action = () => Assignment.Create(
            Guid.CreateVersion7(),
            "   ",
            "Answer every question.",
            opensAt,
            dueAt);

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Create_WithSurroundingWhitespace_TrimsTitle()
    {
        // Arrange
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);
        var dueAt = opensAt.AddDays(7);

        // Act
        var assignment = Assignment.Create(
            Guid.CreateVersion7(),
            "  Networks Assessment  ",
            "Answer every question.",
            opensAt,
            dueAt);

        // Assert
        Assert.Equal(
            "Networks Assessment",
            assignment.Title);
    }

    [Fact]
    public void Create_WithTitleAtMaximumLength_CreatesAssignment()
    {
        // Arrange
        var title = new string('A', 200);
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);
        var dueAt = opensAt.AddDays(7);

        // Act
        var assignment = Assignment.Create(
            Guid.CreateVersion7(),
            title,
            string.Empty,
            opensAt,
            dueAt);

        // Assert
        Assert.Equal(title, assignment.Title);
    }

    [Fact]
    public void Create_WithTitleLongerThanMaximum_ThrowsDomainException()
    {
        // Arrange
        var title = new string('A', 201);
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);
        var dueAt = opensAt.AddDays(7);

        // Act
        var action = () => Assignment.Create(
            Guid.CreateVersion7(),
            title,
            string.Empty,
            opensAt,
            dueAt);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Assignment title cannot exceed 200 characters.",
            exception.Message);
    }

    [Fact]
    public void Create_WithNullInstructions_CreatesAssignmentWithEmptyInstructions()
    {
        // Arrange
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);
        var dueAt = opensAt.AddDays(7);

        // Act
        var assignment = Assignment.Create(
            Guid.CreateVersion7(),
            "Networks Assessment",
            null,
            opensAt,
            dueAt);

        // Assert
        Assert.Equal(string.Empty, assignment.Instructions);
    }

    [Fact]
    public void Create_WithWhitespaceInstructions_CreatesAssignmentWithEmptyInstructions()
    {
        // Arrange
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);
        var dueAt = opensAt.AddDays(7);

        // Act
        var assignment = Assignment.Create(
            Guid.CreateVersion7(),
            "Networks Assessment",
            "   ",
            opensAt,
            dueAt);

        // Assert
        Assert.Equal(string.Empty, assignment.Instructions);
    }

    [Fact]
    public void Create_WithSurroundingWhitespace_TrimsInstructions()
    {
        // Arrange
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);
        var dueAt = opensAt.AddDays(7);

        // Act
        var assignment = Assignment.Create(
            Guid.CreateVersion7(),
            "Networks Assessment",
            "  Answer every question.  ",
            opensAt,
            dueAt);

        // Assert
        Assert.Equal(
            "Answer every question.",
            assignment.Instructions);
    }

    [Fact]
    public void Create_WithInstructionsAtMaximumLength_CreatesAssignment()
    {
        // Arrange
        var instructions = new string('A', 5000);
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);
        var dueAt = opensAt.AddDays(7);

        // Act
        var assignment = Assignment.Create(
            Guid.CreateVersion7(),
            "Networks Assessment",
            instructions,
            opensAt,
            dueAt);

        // Assert
        Assert.Equal(instructions, assignment.Instructions);
    }

    [Fact]
    public void Create_WithInstructionsLongerThanMaximum_ThrowsDomainException()
    {
        // Arrange
        var instructions = new string('A', 5001);
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);
        var dueAt = opensAt.AddDays(7);

        // Act
        var action = () => Assignment.Create(
            Guid.CreateVersion7(),
            "Networks Assessment",
            instructions,
            opensAt,
            dueAt);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Assignment instructions cannot exceed 5000 characters.",
            exception.Message);
    }

    [Fact]
    public void Create_WithDefaultOpenDate_ThrowsDomainException()
    {
        // Act
        var action = () => Assignment.Create(
            Guid.CreateVersion7(),
            "Networks Assessment",
            string.Empty,
            default,
            DateTimeOffset.UtcNow.AddDays(7));

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Assignment open date must be in the future.",
            exception.Message);
    }

    [Fact]
    public void Create_WithDefaultDueDate_ThrowsDomainException()
    {
        // Act
        var action = () => Assignment.Create(
            Guid.CreateVersion7(),
            "Networks Assessment",
            string.Empty,
            DateTimeOffset.UtcNow.AddDays(1),
            default);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Assignment due date cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithDueDateEqualToOpenDate_ThrowsDomainException()
    {
        // Arrange
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);

        // Act
        var action = () => Assignment.Create(
            Guid.CreateVersion7(),
            "Networks Assessment",
            string.Empty,
            opensAt,
            opensAt);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Assignment due date must be later than the open date.",
            exception.Message);
    }

    [Fact]
    public void Create_WithDueDateBeforeOpenDate_ThrowsDomainException()
    {
        // Arrange
        var opensAt = DateTimeOffset.UtcNow.AddDays(7);
        var dueAt = opensAt.AddDays(-1);

        // Act
        var action = () => Assignment.Create(
            Guid.CreateVersion7(),
            "Networks Assessment",
            string.Empty,
            opensAt,
            dueAt);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Assignment due date must be later than the open date.",
            exception.Message);
    }

    [Fact]
    public void ChangeTitle_WithValidTitle_ChangesTitle()
    {
        // Arrange
        var assignment = CreateValidAssignment();

        // Act
        assignment.ChangeTitle("Programming Assessment");

        // Assert
        Assert.Equal(
            "Programming Assessment",
            assignment.Title);
    }

    [Fact]
    public void ChangeTitle_WithSurroundingWhitespace_TrimsTitle()
    {
        // Arrange
        var assignment = CreateValidAssignment();

        // Act
        assignment.ChangeTitle("  Programming Assessment  ");

        // Assert
        Assert.Equal(
            "Programming Assessment",
            assignment.Title);
    }

    [Fact]
    public void ChangeTitle_WithInvalidTitle_DoesNotChangeTitle()
    {
        // Arrange
        var assignment = CreateValidAssignment();
        var originalTitle = assignment.Title;

        // Act
        var action = () => assignment.ChangeTitle("   ");

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalTitle, assignment.Title);
    }

    [Fact]
    public void ChangeTitle_WithTitleLongerThanMaximum_DoesNotChangeTitle()
    {
        // Arrange
        var assignment = CreateValidAssignment();
        var originalTitle = assignment.Title;
        var invalidTitle = new string('A', 201);

        // Act
        var action = () => assignment.ChangeTitle(invalidTitle);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalTitle, assignment.Title);
        Assert.Equal(originalTitle, assignment.Title);
    }

    [Fact]
    public void ChangeInstructions_WithValidInstructions_ChangesInstructions()
    {
        // Arrange
        var assignment = CreateValidAssignment();

        // Act
        assignment.ChangeInstructions(
            "Complete every question independently.");

        // Assert
        Assert.Equal(
            "Complete every question independently.",
            assignment.Instructions);
    }

    [Fact]
    public void ChangeInstructions_WithSurroundingWhitespace_TrimsInstructions()
    {
        // Arrange
        var assignment = CreateValidAssignment();

        // Act
        assignment.ChangeInstructions(
            "  Complete every question independently.  ");

        // Assert
        Assert.Equal(
            "Complete every question independently.",
            assignment.Instructions);
    }

    [Fact]
    public void ChangeInstructions_WithNullInstructions_ClearsInstructions()
    {
        // Arrange
        var assignment = CreateValidAssignment();

        // Act
        assignment.ChangeInstructions(null);

        // Assert
        Assert.Equal(string.Empty, assignment.Instructions);
    }

    [Fact]
    public void ChangeInstructions_WithWhitespaceInstructions_ClearsInstructions()
    {
        // Arrange
        var assignment = CreateValidAssignment();

        // Act
        assignment.ChangeInstructions("   ");

        // Assert
        Assert.Equal(string.Empty, assignment.Instructions);
    }

    [Fact]
    public void ChangeInstructions_WithInstructionsLongerThanMaximum_DoesNotChangeInstructions()
    {
        // Arrange
        var assignment = CreateValidAssignment();
        var originalInstructions = assignment.Instructions;
        var invalidInstructions = new string('A', 5001);

        // Act
        var action = () =>
            assignment.ChangeInstructions(invalidInstructions);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(
            originalInstructions,
            assignment.Instructions);
    }

    [Fact]
    public void Reschedule_WithValidDates_ChangesOpenAndDueDates()
    {
        // Arrange
        var assignment = CreateValidAssignment();
        var newOpensAt = DateTimeOffset.UtcNow.AddDays(10);
        var newDueAt = newOpensAt.AddDays(14);

        // Act
        assignment.Reschedule(newOpensAt, newDueAt);

        // Assert
        Assert.Equal(newOpensAt, assignment.OpensAt);
        Assert.Equal(newDueAt, assignment.DueAt);
    }

    [Fact]
    public void Reschedule_WithDefaultOpenDate_DoesNotChangeSchedule()
    {
        // Arrange
        var assignment = CreateValidAssignment();
        var originalOpensAt = assignment.OpensAt;
        var originalDueAt = assignment.DueAt;

        // Act
        var action = () => assignment.Reschedule(
            default,
            DateTimeOffset.UtcNow.AddDays(14));

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalOpensAt, assignment.OpensAt);
        Assert.Equal(originalDueAt, assignment.DueAt);
    }

    [Fact]
    public void Reschedule_WithDefaultDueDate_DoesNotChangeSchedule()
    {
        // Arrange
        var assignment = CreateValidAssignment();
        var originalOpensAt = assignment.OpensAt;
        var originalDueAt = assignment.DueAt;

        // Act
        var action = () => assignment.Reschedule(
            DateTimeOffset.UtcNow.AddDays(10),
            default);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalOpensAt, assignment.OpensAt);
        Assert.Equal(originalDueAt, assignment.DueAt);
    }

    [Fact]
    public void Reschedule_WithDueDateEqualToOpenDate_DoesNotChangeSchedule()
    {
        // Arrange
        var assignment = CreateValidAssignment();
        var originalOpensAt = assignment.OpensAt;
        var originalDueAt = assignment.DueAt;
        var newOpensAt = DateTimeOffset.UtcNow.AddDays(10);

        // Act
        var action = () =>
            assignment.Reschedule(newOpensAt, newOpensAt);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalOpensAt, assignment.OpensAt);
        Assert.Equal(originalDueAt, assignment.DueAt);
    }

    [Fact]
    public void Reschedule_WithDueDateBeforeOpenDate_DoesNotChangeSchedule()
    {
        // Arrange
        var assignment = CreateValidAssignment();
        var originalOpensAt = assignment.OpensAt;
        var originalDueAt = assignment.DueAt;
        var newOpensAt = DateTimeOffset.UtcNow.AddDays(10);
        var newDueAt = newOpensAt.AddDays(-1);

        // Act
        var action = () =>
            assignment.Reschedule(newOpensAt, newDueAt);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalOpensAt, assignment.OpensAt);
        Assert.Equal(originalDueAt, assignment.DueAt);
    }

    [Fact]
    public void Publish_WhenAssignmentIsDraft_SetsAssignmentToPublished()
    {
        // Arrange
        var assignment = CreateValidAssignment();

        // Act
        assignment.Publish();

        // Assert
        Assert.True(assignment.IsPublished);
    }

    [Fact]
    public void ReturnToDraft_WhenAssignmentIsPublished_SetsAssignmentToDraft()
    {
        // Arrange
        var assignment = CreateValidAssignment();
        assignment.Publish();

        // Act
        assignment.ReturnToDraft();

        // Assert
        Assert.False(assignment.IsPublished);
    }

    private static Assignment CreateValidAssignment()
    {
        var opensAt = DateTimeOffset.UtcNow.AddDays(1);
        var dueAt = opensAt.AddDays(7);

        return Assignment.Create(
            Guid.CreateVersion7(),
            "Networks Assessment",
            "Answer every question.",
            opensAt,
            dueAt);
    }
}