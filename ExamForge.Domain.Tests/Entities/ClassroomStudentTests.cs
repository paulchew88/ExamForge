using ExamForge.Domain.Entities;
using ExamForge.Domain.Exceptions;
namespace ExamForge.Domain.Tests.Entities;

public class ClassroomStudentTests
{
    [Fact]
    public void Create_WithValidValues_CreatesClassroomStudent()
    {
        // Arrange
        var classroomId = Guid.CreateVersion7();
        var studentId = Guid.CreateVersion7();
        var beforeCreation = DateTimeOffset.UtcNow;

        // Act
        var classroomStudent = ClassroomStudent.Create(
            classroomId,
            studentId);

        var afterCreation = DateTimeOffset.UtcNow;

        // Assert
        Assert.NotEqual(Guid.Empty, classroomStudent.Id);
        Assert.Equal(classroomId, classroomStudent.ClassroomId);
        Assert.Equal(studentId, classroomStudent.StudentId);

        Assert.True(classroomStudent.IsActive);
        Assert.Null(classroomStudent.LeftAt);

        Assert.Equal(
            classroomStudent.CreatedAt,
            classroomStudent.JoinedAt);

        Assert.InRange(
            classroomStudent.CreatedAt,
            beforeCreation,
            afterCreation);
    }

    [Fact]
    public void Create_WithEmptyClassroomId_ThrowsDomainException()
    {
        // Act
        var action = () => ClassroomStudent.Create(
            Guid.Empty,
            Guid.CreateVersion7());

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Classroom ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithEmptyStudentId_ThrowsDomainException()
    {
        // Act
        var action = () => ClassroomStudent.Create(
            Guid.CreateVersion7(),
            Guid.Empty);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Student ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Leave_WhenActive_MarksStudentAsInactive()
    {
        // Arrange
        var classroomStudent = CreateValidClassroomStudent();

        // Act
        classroomStudent.Leave();

        // Assert
        Assert.False(classroomStudent.IsActive);
        Assert.NotNull(classroomStudent.LeftAt);
    }

    [Fact]
    public void Leave_WhenActive_SetsLeftAtToCurrentTime()
    {
        // Arrange
        var classroomStudent = CreateValidClassroomStudent();

        var beforeLeave = DateTimeOffset.UtcNow;

        // Act
        classroomStudent.Leave();

        var afterLeave = DateTimeOffset.UtcNow;

        // Assert
        Assert.NotNull(classroomStudent.LeftAt);

        Assert.InRange(
            classroomStudent.LeftAt!.Value,
            beforeLeave,
            afterLeave);
    }

    [Fact]
    public void Leave_WhenAlreadyInactive_ThrowsDomainExceptionAndPreservesState()
    {
        // Arrange
        var classroomStudent = CreateValidClassroomStudent();

        classroomStudent.Leave();

        var originalLeftAt = classroomStudent.LeftAt;
        var originalJoinedAt = classroomStudent.JoinedAt;

        // Act
        var action = () => classroomStudent.Leave();

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Student is not currently enrolled in the classroom.",
            exception.Message);

        Assert.False(classroomStudent.IsActive);
        Assert.Equal(originalLeftAt, classroomStudent.LeftAt);
        Assert.Equal(originalJoinedAt, classroomStudent.JoinedAt);
    }

    [Fact]
    public void Rejoin_WhenAlreadyActive_ThrowsDomainExceptionAndPreservesState()
    {
        // Arrange
        var classroomStudent = CreateValidClassroomStudent();

        var originalJoinedAt = classroomStudent.JoinedAt;

        // Act
        var action = () => classroomStudent.Rejoin();

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Student is already enrolled in the classroom.",
            exception.Message);

        Assert.True(classroomStudent.IsActive);
        Assert.Null(classroomStudent.LeftAt);
        Assert.Equal(originalJoinedAt, classroomStudent.JoinedAt);
    }

    [Fact]
    public void Rejoin_WhenInactive_ReactivatesStudent()
    {
        // Arrange
        var classroomStudent = CreateValidClassroomStudent();

        classroomStudent.Leave();

        // Act
        classroomStudent.Rejoin();

        // Assert
        Assert.True(classroomStudent.IsActive);
        Assert.Null(classroomStudent.LeftAt);
    }

    [Fact]
    public void Leave_DoesNotChangeOtherProperties()
    {
        // Arrange
        var classroomStudent = CreateValidClassroomStudent();

        var classroomId = classroomStudent.ClassroomId;
        var studentId = classroomStudent.StudentId;
        var createdAt = classroomStudent.CreatedAt;

        // Act
        classroomStudent.Leave();

        // Assert
        Assert.Equal(classroomId, classroomStudent.ClassroomId);
        Assert.Equal(studentId, classroomStudent.StudentId);
        Assert.Equal(createdAt, classroomStudent.CreatedAt);
    }

    [Fact]
    public void Rejoin_DoesNotChangeOtherProperties()
    {
        // Arrange
        var classroomStudent = CreateValidClassroomStudent();

        classroomStudent.Leave();

        var classroomId = classroomStudent.ClassroomId;
        var studentId = classroomStudent.StudentId;
        var createdAt = classroomStudent.CreatedAt;

        // Act
        classroomStudent.Rejoin();

        // Assert
        Assert.Equal(classroomId, classroomStudent.ClassroomId);
        Assert.Equal(studentId, classroomStudent.StudentId);
        Assert.Equal(createdAt, classroomStudent.CreatedAt);
    }
    [Fact]
    public void Rejoin_PreservesJoinedAt()
    {
        // Arrange
        var classroomStudent = CreateValidClassroomStudent();

        var originalJoinedAt = classroomStudent.JoinedAt;

        classroomStudent.Leave();

        // Act
        classroomStudent.Rejoin();

        // Assert
        Assert.Equal(originalJoinedAt, classroomStudent.JoinedAt);
    }
    [Fact]
    public void CompleteLifecycle_CreateLeaveRejoin_WorksCorrectly()
    {
        // Arrange
        var classroomStudent = ClassroomStudent.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7());

        // Assert
        Assert.True(classroomStudent.IsActive);

        // Act
        classroomStudent.Leave();

        // Assert
        Assert.False(classroomStudent.IsActive);
        Assert.NotNull(classroomStudent.LeftAt);

        // Act
        classroomStudent.Rejoin();

        // Assert
        Assert.True(classroomStudent.IsActive);
        Assert.Null(classroomStudent.LeftAt);
    }

    private static ClassroomStudent CreateValidClassroomStudent()
    {
        return ClassroomStudent.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7());
    }
}