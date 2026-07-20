
using ExamForge.Domain.Entities;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Tests.Entities;

public class ClassroomTests
{
    [Fact]
    public void Create_WithValidValues_CreatesActiveClassroom()
    {
        // Arrange
        var courseId = Guid.CreateVersion7();
        var teacherId = Guid.CreateVersion7();
        const string name = "Year 10 Computer Science";
        const string joinCode = "CS10A";

        // Act
        var classroom = Classroom.Create(
            courseId,
            teacherId,
            name,
            joinCode);

        // Assert
        Assert.NotEqual(Guid.Empty, classroom.Id);
        Assert.Equal(courseId, classroom.CourseId);
        Assert.Equal(teacherId, classroom.TeacherId);
        Assert.Equal(name, classroom.Name);
        Assert.Equal(joinCode, classroom.JoinCode);
        Assert.True(classroom.IsActive);
        Assert.NotEqual(default, classroom.CreatedAt);
    }

    [Fact]
    public void Create_WithEmptyCourseId_ThrowsDomainException()
    {
        // Act
        var action = () => Classroom.Create(
            Guid.Empty,
            Guid.CreateVersion7(),
            "Year 10 Computer Science",
            "CS10A");

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Course ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithEmptyTeacherId_ThrowsDomainException()
    {
        // Act
        var action = () => Classroom.Create(
            Guid.CreateVersion7(),
            Guid.Empty,
            "Year 10 Computer Science",
            "CS10A");

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Teacher ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsDomainException()
    {
        // Act
        var action = () => Classroom.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            string.Empty,
            "CS10A");

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Classroom name cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithWhitespaceName_ThrowsDomainException()
    {
        // Act
        var action = () => Classroom.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            "   ",
            "CS10A");

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Create_WithSurroundingWhitespace_TrimsName()
    {
        // Act
        var classroom = Classroom.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            "  Year 10 Computer Science  ",
            "CS10A");

        // Assert
        Assert.Equal(
            "Year 10 Computer Science",
            classroom.Name);
    }

    [Fact]
    public void Create_WithNameAtMaximumLength_CreatesClassroom()
    {
        // Arrange
        var name = new string('A', 150);

        // Act
        var classroom = Classroom.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            name,
            "CS10A");

        // Assert
        Assert.Equal(name, classroom.Name);
    }

    [Fact]
    public void Create_WithNameLongerThanMaximum_ThrowsDomainException()
    {
        // Arrange
        var name = new string('A', 151);

        // Act
        var action = () => Classroom.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            name,
            "CS10A");

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Classroom name cannot exceed 150 characters.",
            exception.Message);
    }

    [Fact]
    public void Create_WithEmptyJoinCode_ThrowsDomainException()
    {
        // Act
        var action = () => Classroom.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            "Year 10 Computer Science",
            string.Empty);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Classroom join code cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithWhitespaceJoinCode_ThrowsDomainException()
    {
        // Act
        var action = () => Classroom.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            "Year 10 Computer Science",
            "   ");

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Create_WithLowercaseJoinCode_NormalisesJoinCodeToUppercase()
    {
        // Act
        var classroom = Classroom.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            "Year 10 Computer Science",
            "cs10a");

        // Assert
        Assert.Equal("CS10A", classroom.JoinCode);
    }

    [Fact]
    public void Create_WithSurroundingWhitespace_TrimsJoinCode()
    {
        // Act
        var classroom = Classroom.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            "Year 10 Computer Science",
            "  CS10A  ");

        // Assert
        Assert.Equal("CS10A", classroom.JoinCode);
    }

    [Fact]
    public void Rename_WithValidName_ChangesName()
    {
        // Arrange
        var classroom = CreateValidClassroom();

        // Act
        classroom.Rename("Year 11 Computer Science");

        // Assert
        Assert.Equal(
            "Year 11 Computer Science",
            classroom.Name);
    }

    [Fact]
    public void Rename_WithSurroundingWhitespace_TrimsName()
    {
        // Arrange
        var classroom = CreateValidClassroom();

        // Act
        classroom.Rename("  Year 11 Computer Science  ");

        // Assert
        Assert.Equal(
            "Year 11 Computer Science",
            classroom.Name);
    }

    [Fact]
    public void Rename_WithInvalidName_DoesNotChangeName()
    {
        // Arrange
        var classroom = CreateValidClassroom();
        var originalName = classroom.Name;

        // Act
        var action = () => classroom.Rename("   ");

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalName, classroom.Name);
    }

    [Fact]
    public void ChangeCourse_WithValidCourseId_ChangesCourseId()
    {
        // Arrange
        var classroom = CreateValidClassroom();
        var newCourseId = Guid.CreateVersion7();

        // Act
        classroom.ChangeCourse(newCourseId);

        // Assert
        Assert.Equal(newCourseId, classroom.CourseId);
    }

    [Fact]
    public void ChangeCourse_WithEmptyCourseId_DoesNotChangeCourseId()
    {
        // Arrange
        var classroom = CreateValidClassroom();
        var originalCourseId = classroom.CourseId;

        // Act
        var action = () => classroom.ChangeCourse(Guid.Empty);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalCourseId, classroom.CourseId);
    }

    [Fact]
    public void RegenerateJoinCode_WithValidCode_ChangesJoinCode()
    {
        // Arrange
        var classroom = CreateValidClassroom();

        // Act
        classroom.RegenerateJoinCode("NEW10");

        // Assert
        Assert.Equal("NEW10", classroom.JoinCode);
    }

    [Fact]
    public void RegenerateJoinCode_WithLowercaseCode_NormalisesJoinCodeToUppercase()
    {
        // Arrange
        var classroom = CreateValidClassroom();

        // Act
        classroom.RegenerateJoinCode("new10");

        // Assert
        Assert.Equal("NEW10", classroom.JoinCode);
    }

    [Fact]
    public void RegenerateJoinCode_WithSurroundingWhitespace_TrimsJoinCode()
    {
        // Arrange
        var classroom = CreateValidClassroom();

        // Act
        classroom.RegenerateJoinCode("  NEW10  ");

        // Assert
        Assert.Equal("NEW10", classroom.JoinCode);
    }

    [Fact]
    public void RegenerateJoinCode_WithInvalidCode_DoesNotChangeJoinCode()
    {
        // Arrange
        var classroom = CreateValidClassroom();
        var originalJoinCode = classroom.JoinCode;

        // Act
        var action = () => classroom.RegenerateJoinCode("   ");

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalJoinCode, classroom.JoinCode);
    }

    [Fact]
    public void Deactivate_WhenClassroomIsActive_SetsClassroomToInactive()
    {
        // Arrange
        var classroom = CreateValidClassroom();

        // Act
        classroom.Deactivate();

        // Assert
        Assert.False(classroom.IsActive);
    }

    [Fact]
    public void Activate_WhenClassroomIsInactive_SetsClassroomToActive()
    {
        // Arrange
        var classroom = CreateValidClassroom();
        classroom.Deactivate();

        // Act
        classroom.Activate();

        // Assert
        Assert.True(classroom.IsActive);
    }

    private static Classroom CreateValidClassroom()
    {
        return Classroom.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            "Year 10 Computer Science",
            "CS10A");
    }


}
