using ExamForge.Domain.Entities;
using ExamForge.Domain.Exceptions;
namespace ExamForge.Domain.Tests.Entities;

public class CourseTests
{
    [Fact]
    public void Create_WithValidValues_CreatesActiveCourse()
    {
        // Arrange
        const string name = "AQA A-Level Computer Science";
        const string description = "AQA specification 7517";

        // Act
        var course = Course.Create(name, description);

        // Assert
        Assert.NotEqual(Guid.Empty, course.Id);
        Assert.Equal(name, course.Name);
        Assert.Equal(description, course.Description);
        Assert.True(course.IsActive);
        Assert.NotEqual(default, course.CreatedAt);
    }

    [Fact]
    public void Create_WithEmptyName_ThrowsDomainException()
    {
        // Act
        var action = () => Course.Create("");

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Course name cannot be empty.",
            exception.Message);

    }


    [Fact]
    public void Deactivate_WhenCourseIsActive_SetsCourseToInactive()
    {
        // Arrange
        var course = Course.Create(
            "AQA A-Level Computer Science",
            "AQA specification 7517");

        // Act
        course.Deactivate();

        // Assert
        Assert.False(course.IsActive);
    }

    [Fact]
    public void Activate_WhenCourseIsInactive_SetsCourseToActive()
    {
        // Arrange
        var course = Course.Create(
            "AQA A-Level Computer Science",
            "AQA specification 7517");
        course.Deactivate();
        // Act
        course.Activate();
        // Assert
        Assert.True(course.IsActive);
    }
    [Fact]
    public void Rename_WithValidName_UpdatesName()
    {
        // Arrange
        var course = Course.Create("Old course name");

        // Act
        course.Rename("New course name");

        // Assert
        Assert.Equal("New course name", course.Name);
    }

    [Fact]
    public void Rename_WithSurroundingWhitespace_TrimsName()
    {
        // Arrange
        var course = Course.Create("Old course name");

        // Act
        course.Rename("  New course name  ");

        // Assert
        Assert.Equal("New course name", course.Name);
    }

    [Fact]
    public void Rename_WithEmptyName_ThrowsDomainException()
    {
        // Arrange
        var course = Course.Create("Existing course");

        // Act
        var action = () => course.Rename("");

        // Assert
        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Rename_WithWhitespaceName_ThrowsDomainException()
    {
        // Arrange
        var course = Course.Create("Existing course");

        // Act
        var action = () => course.Rename("   ");

        // Assert
        Assert.Throws<DomainException>(action);
    }
    [Fact]
    public void Rename_WithNameLongerThanMaximum_ThrowsDomainException()
    {
        // Arrange
        var course = Course.Create("Existing course");
        var longName = new string('A', 151);

        // Act
        var action = () => course.Rename(longName);

        // Assert
        Assert.Throws<DomainException>(action);
    }
    [Fact]
    public void Rename_withNameAtMaximum_UpdatesName()
    {
        // Arrange
        var course = Course.Create("Existing course");
        var maximumName = new string('A', 150);

        // Act
        course.Rename(maximumName);

        // Assert
        Assert.Equal(maximumName, course.Name);
    }
    [Fact]
    public void UpdateDescription_WithValidDescription_UpdatesDescription()
    {
        // Arrange
        var course = Course.Create("Existing course", "Old description");
        // Act
        course.UpdateDescription("New description");
        // Assert
        Assert.Equal("New description", course.Description);
    }
    [Fact]
    public void UpdateDescription_WithNull_ClearsDescription()
    {
        // Arrange
        var course = Course.Create("Existing course", "Old description");
        // Act
        course.UpdateDescription(null);
        // Assert
        Assert.Null(course.Description);
    }
    [Fact]
    public void UpdateDescription_WithWhitespace_ClearsDescription()
    {
        // Arrange
        var course = Course.Create("Existing course", "Old description");
        // Act
        course.UpdateDescription("   ");
        // Assert
        Assert.Null(course.Description);
    }
    [Fact]
    public void UpdateDescription_WithTrimmedText_TrimsDescription()
    {
        // Arrange
        var course = Course.Create("Existing course", "Old description");
        // Act
        course.UpdateDescription("  New description  ");
        // Assert
        Assert.Equal("New description", course.Description);
    }
    [Fact]
    public void UpdateDescription_WithDescriptionLongerThan1000Characters_ThrowsDomainException()
    {
        // Arrange
        var course = Course.Create("Existing course", "Old description");
        var longDescription = new string('A', 1001);
        // Act
        var action = () => course.UpdateDescription(longDescription);
        // Assert
        Assert.Throws<DomainException>(action);
    }
    [Fact]
    public void UpdateDescription_WithDescriptionLongerThanMaximum_DoesNotChangeDescription()
    {
        // Arrange
        var course = Course.Create("Existing course", "Old description");
        var longDescription = new string('A', 1001);

        // Act
        var action = () => course.UpdateDescription(longDescription);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal("Old description", course.Description);
    }
    [Fact]
    public void UpdateDescription_WithDescriptionAtMaximum_UpdatesDescription()
    {
        // Arrange
        var course = Course.Create("Existing course", "Old description");
        var maximumDescription = new string('A', 1000);
        // Act
        course.UpdateDescription(maximumDescription);
        // Assert
        Assert.Equal(maximumDescription, course.Description);
    }
    [Fact]
    public void Create_WithWhitespaceDescription_SetsDescriptionToNull()
    {
        // Act
        var course = Course.Create("Existing course", "   ");

        // Assert
        Assert.Null(course.Description);
    }

    [Fact]
    public void Create_WithDescriptionLongerThanMaximum_ThrowsDomainException()
    {
        // Arrange
        var longDescription = new string('A', 1001);

        // Act
        var action = () => Course.Create(
            "Existing course",
            longDescription);

        // Assert
        Assert.Throws<DomainException>(action);
    }
}