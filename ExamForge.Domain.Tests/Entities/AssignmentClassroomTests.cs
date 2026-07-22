using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Tests.Entities;

public class AssignmentClassroomTests
{
    [Fact]
    public void Create_WithValidValues_CreatesAssignmentClassroom()
    {
        // Arrange
        var assignmentId = Guid.CreateVersion7();
        var classroomId = Guid.CreateVersion7();
        var beforeCreation = DateTimeOffset.UtcNow;

        // Act
        var assignmentClassroom = AssignmentClassroom.Create(
            assignmentId,
            classroomId);

        var afterCreation = DateTimeOffset.UtcNow;

        // Assert
        Assert.NotEqual(Guid.Empty, assignmentClassroom.Id);
        Assert.Equal(assignmentId, assignmentClassroom.AssignmentId);
        Assert.Equal(classroomId, assignmentClassroom.ClassroomId);
        Assert.True(assignmentClassroom.IsActive);
        Assert.Null(assignmentClassroom.UnassignedAt);

        Assert.Equal(
            assignmentClassroom.CreatedAt,
            assignmentClassroom.AssignedAt);

        Assert.InRange(
            assignmentClassroom.CreatedAt,
            beforeCreation,
            afterCreation);
    }

    [Fact]
    public void Create_WithEmptyAssignmentId_ThrowsDomainException()
    {
        // Act
        var action = () => AssignmentClassroom.Create(
            Guid.Empty,
            Guid.CreateVersion7());

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Assignment ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithEmptyClassroomId_ThrowsDomainException()
    {
        // Act
        var action = () => AssignmentClassroom.Create(
            Guid.CreateVersion7(),
            Guid.Empty);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Classroom ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Unassign_WhenActive_MarksRelationshipAsInactive()
    {
        // Arrange
        var assignmentClassroom = CreateValidAssignmentClassroom();

        // Act
        assignmentClassroom.Unassign();

        // Assert
        Assert.False(assignmentClassroom.IsActive);
        Assert.NotNull(assignmentClassroom.UnassignedAt);
    }

    [Fact]
    public void Unassign_WhenActive_SetsUnassignedAtToCurrentTime()
    {
        // Arrange
        var assignmentClassroom = CreateValidAssignmentClassroom();
        var beforeUnassign = DateTimeOffset.UtcNow;

        // Act
        assignmentClassroom.Unassign();

        var afterUnassign = DateTimeOffset.UtcNow;

        // Assert
        Assert.NotNull(assignmentClassroom.UnassignedAt);

        Assert.InRange(
            assignmentClassroom.UnassignedAt!.Value,
            beforeUnassign,
            afterUnassign);
    }

    [Fact]
    public void Unassign_WhenAlreadyInactive_ThrowsDomainExceptionAndPreservesState()
    {
        // Arrange
        var assignmentClassroom = CreateValidAssignmentClassroom();

        assignmentClassroom.Unassign();

        var originalAssignedAt = assignmentClassroom.AssignedAt;
        var originalUnassignedAt = assignmentClassroom.UnassignedAt;

        // Act
        var action = () => assignmentClassroom.Unassign();

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Assignment is not currently assigned to the classroom.",
            exception.Message);

        Assert.False(assignmentClassroom.IsActive);
        Assert.Equal(originalAssignedAt, assignmentClassroom.AssignedAt);
        Assert.Equal(originalUnassignedAt, assignmentClassroom.UnassignedAt);
    }

    [Fact]
    public void Reassign_WhenInactive_ReactivatesRelationship()
    {
        // Arrange
        var assignmentClassroom = CreateValidAssignmentClassroom();

        assignmentClassroom.Unassign();

        // Act
        assignmentClassroom.Reassign();

        // Assert
        Assert.True(assignmentClassroom.IsActive);
        Assert.Null(assignmentClassroom.UnassignedAt);
    }

    [Fact]
    public void Reassign_WhenInactive_PreservesAssignedAt()
    {
        // Arrange
        var assignmentClassroom = CreateValidAssignmentClassroom();

        var originalAssignedAt = assignmentClassroom.AssignedAt;

        assignmentClassroom.Unassign();

        // Act
        assignmentClassroom.Reassign();

        // Assert
        Assert.Equal(
            originalAssignedAt,
            assignmentClassroom.AssignedAt);
    }

    [Fact]
    public void Reassign_WhenAlreadyActive_ThrowsDomainExceptionAndPreservesState()
    {
        // Arrange
        var assignmentClassroom = CreateValidAssignmentClassroom();

        var originalAssignedAt = assignmentClassroom.AssignedAt;

        // Act
        var action = () => assignmentClassroom.Reassign();

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Assignment is already assigned to the classroom.",
            exception.Message);

        Assert.True(assignmentClassroom.IsActive);
        Assert.Null(assignmentClassroom.UnassignedAt);
        Assert.Equal(originalAssignedAt, assignmentClassroom.AssignedAt);
    }

    [Fact]
    public void Unassign_DoesNotChangeOtherProperties()
    {
        // Arrange
        var assignmentClassroom = CreateValidAssignmentClassroom();

        var assignmentId = assignmentClassroom.AssignmentId;
        var classroomId = assignmentClassroom.ClassroomId;
        var assignedAt = assignmentClassroom.AssignedAt;
        var createdAt = assignmentClassroom.CreatedAt;

        // Act
        assignmentClassroom.Unassign();

        // Assert
        Assert.Equal(assignmentId, assignmentClassroom.AssignmentId);
        Assert.Equal(classroomId, assignmentClassroom.ClassroomId);
        Assert.Equal(assignedAt, assignmentClassroom.AssignedAt);
        Assert.Equal(createdAt, assignmentClassroom.CreatedAt);
    }

    [Fact]
    public void Reassign_DoesNotChangeOtherProperties()
    {
        // Arrange
        var assignmentClassroom = CreateValidAssignmentClassroom();

        assignmentClassroom.Unassign();

        var assignmentId = assignmentClassroom.AssignmentId;
        var classroomId = assignmentClassroom.ClassroomId;
        var assignedAt = assignmentClassroom.AssignedAt;
        var createdAt = assignmentClassroom.CreatedAt;

        // Act
        assignmentClassroom.Reassign();

        // Assert
        Assert.Equal(assignmentId, assignmentClassroom.AssignmentId);
        Assert.Equal(classroomId, assignmentClassroom.ClassroomId);
        Assert.Equal(assignedAt, assignmentClassroom.AssignedAt);
        Assert.Equal(createdAt, assignmentClassroom.CreatedAt);
    }

    [Fact]
    public void CompleteLifecycle_CreateUnassignReassign_WorksCorrectly()
    {
        // Arrange
        var assignmentClassroom = AssignmentClassroom.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7());

        var originalAssignedAt = assignmentClassroom.AssignedAt;

        // Assert
        Assert.True(assignmentClassroom.IsActive);

        // Act
        assignmentClassroom.Unassign();

        // Assert
        Assert.False(assignmentClassroom.IsActive);
        Assert.NotNull(assignmentClassroom.UnassignedAt);

        // Act
        assignmentClassroom.Reassign();

        // Assert
        Assert.True(assignmentClassroom.IsActive);
        Assert.Null(assignmentClassroom.UnassignedAt);
        Assert.Equal(originalAssignedAt, assignmentClassroom.AssignedAt);
    }

    private static AssignmentClassroom CreateValidAssignmentClassroom()
    {
        return AssignmentClassroom.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7());
    }
}