using ExamForge.Domain.Entities;
using ExamForge.Domain.Exceptions;
namespace ExamForge.Domain.Tests.Entities;

public class UnitTests
{
    [Fact]
    public void Create_WithValidValues_CreatesActiveUnit()
    {
        //Arrange
        var courseId = Guid.CreateVersion7();
        const string name = "Unit 1 - Computational Thinking";
        const string description = "This unit covers the basics of computational thinking.";
        const int displayOrder = 1;

        //Act
        var unit = Unit.Create(courseId, name, description, displayOrder);

        //Assert
        Assert.NotEqual(Guid.Empty, unit.Id);
        Assert.Equal(courseId, unit.CourseId);
        Assert.Equal(name, unit.Name);
        Assert.Equal(description, unit.Description);
        Assert.Equal(displayOrder, unit.DisplayOrder);
        Assert.True(unit.IsActive);
        Assert.NotEqual(default, unit.CreatedAt);

    }
    [Fact]
    public void Create_WithEmptyCourseId_ThrowsDomainException()
    {
        // Arrange
        var courseId = Guid.Empty;

        // Act
        var action = () => Unit.Create(
            courseId,
            "Programming",
            "Introduction",
            1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Course ID cannot be empty.",
            exception.Message);
    }
    [Fact]
    public void Create_WithWhitespaceName_ThrowsDomainException()
    {
        // Act
        var action = () => Unit.Create(
            Guid.CreateVersion7(),
            "   ",
            null,
            1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Unit name cannot be empty.",
            exception.Message);
    }
    [Fact]
    public void Create_WithSurroundingWhitespace_TrimsName()
    {
        // Act
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "  Programming  ",
            null,
            1);

        // Assert
        Assert.Equal("Programming", unit.Name);
    }
    [Fact]
    public void Create_WithWhitespaceDescription_SetsDescriptionToNull()
    {
        // Act
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Programming",
            "   ",
            1);

        // Assert
        Assert.Null(unit.Description);
    }
    [Fact]
    public void Create_WithDescriptionAtMaximumLength_CreatesUnit()
    {
        // Arrange
        var description = new string('A', 1000);

        // Act
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Programming",
            description,
            1);

        // Assert
        Assert.Equal(description, unit.Description);
    }
    [Fact]
    public void Create_WithDescriptionLongerThanMaximum_ThrowsDomainException()
    {
        // Arrange
        var description = new string('A', 1001);

        // Act
        var action = () => Unit.Create(
            Guid.CreateVersion7(),
            "Programming",
            description,
            1);

        // Assert
        Assert.Throws<DomainException>(action);
    }
    [Fact]
    public void Rename_WithValidName_UpdatesName()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Old name",
            null,
            1);

        // Act
        unit.Rename("New name");

        // Assert
        Assert.Equal("New name", unit.Name);
    }
    [Fact]
    public void Rename_WithInvalidName_DoesNotChangeName()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Existing name",
            null,
            1);

        var invalidName = new string('A', 151);

        // Act
        var action = () => unit.Rename(invalidName);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal("Existing name", unit.Name);
    }
    [Fact]
    public void Deactivate_WhenUnitIsActive_SetsUnitToInactive()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Unit Name",
            null,
            1);
        // Act
        unit.Deactivate();
        // Assert
        Assert.False(unit.IsActive);
    }
    [Fact]
    public void Activate_WhenUnitIsInactive_SetsUnitToActive()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Unit Name",
            null,
            1);
        unit.Deactivate();
        // Act
        unit.Activate();
        // Assert
        Assert.True(unit.IsActive);
    }
    [Fact]
    public void Create_WithNegativeDisplayOrder_ThrowsDomainException()
    {
        // Act
        var action = () => Unit.Create(
            Guid.CreateVersion7(),
            "Programming",
            null,
            -1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Display order must be greater than zero.",
            exception.Message);
    }
    [Fact]
    public void ChangeDisplayOrder_WithValidValue_UpdatesDisplayOrder()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Unit Name",
            null,
            1);
        // Act
        unit.ChangeDisplayOrder(2);
        // Assert
        Assert.Equal(2, unit.DisplayOrder);
    }
    [Fact]
    public void ChangeDisplayOrder_WithInvalidValue_ThrowsDomainException()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Unit Name",
            null,
            1);
        // Act
        var action = () => unit.ChangeDisplayOrder(0);
        // Assert
        var exception = Assert.Throws<DomainException>(action);
        Assert.Equal(
            "Display order must be greater than zero.",
            exception.Message);
    }
    [Fact]
    public void Create_WithZeroDisplayOrder_ThrowsDomainException()
    {
        // Act
        var action = () => Unit.Create(
            Guid.CreateVersion7(),
            "Programming",
            null,
            0);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Display order must be greater than zero.",
            exception.Message);
    }
    [Fact]
    public void ChangeDisplayOrder_WithInvalidValue_DoesNotChangeDisplayOrder()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Unit Name",
            null,
            1);

        // Act
        var action = () => unit.ChangeDisplayOrder(0);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(1, unit.DisplayOrder);
    }
    [Fact]
    public void UpdateDescription_WithValidDescription_UpdatesDescription()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Unit Name",
            null,
            1);
        // Act
        unit.UpdateDescription("New description");
        // Assert
        Assert.Equal("New description", unit.Description);
    }
    [Fact]
    public void UpdateDescription_WithNull_ClearsDescription()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Unit Name",
            "Old description",
            1);
        // Act
        unit.UpdateDescription(null);
        // Assert
        Assert.Null(unit.Description);
    }
    [Fact]
    public void UpdateDescription_WithWhitespace_ClearsDescription()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Unit Name",
            "Old description",
            1);
        // Act
        unit.UpdateDescription("   ");
        // Assert
        Assert.Null(unit.Description);
    }
    [Fact]
    public void UpdateDescription_WithSurroundingWhitespace_TrimsDescription()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Unit Name",
            null,
            1);
        // Act
        unit.UpdateDescription("  New description  ");
        // Assert
        Assert.Equal("New description", unit.Description);
    }
    [Fact]
    public void UpdateDescription_WithDescriptionLongerThanMaximum_ThrowsDomainException()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Unit Name",
            null,
            1);
        var longDescription = new string('A', 1001);
        // Act
        var action = () => unit.UpdateDescription(longDescription);
        // Assert
        var exception = Assert.Throws<DomainException>(action);
        Assert.Equal(
            "Unit description cannot exceed 1000 characters.",
            exception.Message);
    }
    [Fact]
    public void UpdateDescription_WithInvalidDescription_DoesNotChangeDescription()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Unit Name",
            "Old description",
            1);
        var longDescription = new string('A', 1001);
        // Act
        var action = () => unit.UpdateDescription(longDescription);
        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal("Old description", unit.Description);
    }
    [Fact]
    public void Rename_WithNameAtMaximumLength_UpdatesName()
    {
        // Arrange
        var unit = Unit.Create(
            Guid.CreateVersion7(),
            "Existing name",
            null,
            1);

        var maximumLengthName = new string('A', 150);

        // Act
        unit.Rename(maximumLengthName);

        // Assert
        Assert.Equal(maximumLengthName, unit.Name);
    }
}
