using ExamForge.Domain.Entities;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Tests.Entities;

public class TopicTests
{
    [Fact]
    public void Create_WithValidValues_CreatesActiveTopic()
    {
        //Arrange
        var unitId = Guid.CreateVersion7();
        const string name = "Algorithms";
        const string description = "Understanding Algorithms";
        const int displayOrder = 1;

        //Act
        var topic = Topic.Create(unitId, name, description, displayOrder);

        //Assert
        Assert.NotEqual(Guid.Empty, topic.Id);
        Assert.Equal(unitId, topic.UnitId);
        Assert.Equal(name, topic.Name);
        Assert.Equal(description, topic.Description);
        Assert.Equal(displayOrder, topic.DisplayOrder);
        Assert.True(topic.IsActive);
        Assert.NotEqual(default, topic.CreatedAt);

    }
    [Fact]
    public void Create_WithEmptyUnitId_ThrowsDomainException()
    {
        // Arrange
        var unitId = Guid.Empty;

        // Act
        var action = () => Topic.Create(
            unitId,
            "Programming",
            "Introduction",
            1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Unit ID cannot be empty.",
            exception.Message);
    }
    [Fact]
    public void Create_WithWhitespaceName_ThrowsDomainException()
    {
        // Act
        var action = () => Topic.Create(
            Guid.CreateVersion7(),
            "   ",
            null,
            1);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Topic name cannot be empty.",
            exception.Message);
    }
    [Fact]
    public void Create_WithSurroundingWhitespace_TrimsName()
    {
        // Act
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "  Programming  ",
            null,
            1);

        // Assert
        Assert.Equal("Programming", topic.Name);
    }
    [Fact]
    public void Create_WithWhitespaceDescription_SetsDescriptionToNull()
    {
        // Act
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Programming",
            "   ",
            1);

        // Assert
        Assert.Null(topic.Description);
    }
    [Fact]
    public void Create_WithDescriptionAtMaximumLength_CreatesTopic()
    {
        // Arrange
        var description = new string('A', 1000);

        // Act
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Programming",
            description,
            1);

        // Assert
        Assert.Equal(description, topic.Description);
    }
    [Fact]
    public void Create_WithDescriptionLongerThanMaximum_ThrowsDomainException()
    {
        // Arrange
        var description = new string('A', 1001);

        // Act
        var action = () => Topic.Create(
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
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Old name",
            null,
            1);

        // Act
        topic.Rename("New name");

        // Assert
        Assert.Equal("New name", topic.Name);
    }
    [Fact]
    public void Rename_WithInvalidName_DoesNotChangeName()
    {
        // Arrange
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Existing name",
            null,
            1);

        var invalidName = new string('A', 151);

        // Act
        var action = () => topic.Rename(invalidName);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal("Existing name", topic.Name);
    }
    [Fact]
    public void Deactivate_WhenTopicIsActive_SetsTopicToInactive()
    {
        // Arrange
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Topic Name",
            null,
            1);
        // Act
        topic.Deactivate();
        // Assert
        Assert.False(topic.IsActive);
    }
    [Fact]
    public void Activate_WhenTopicIsInactive_SetsTopicToActive()
    {
        // Arrange
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Topic Name",
            null,
            1);
        topic.Deactivate();
        // Act
        topic.Activate();
        // Assert
        Assert.True(topic.IsActive);
    }
    [Fact]
    public void Create_WithNegativeDisplayOrder_ThrowsDomainException()
    {
        // Act
        var action = () => Topic.Create(
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
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Topic Name",
            null,
            1);
        // Act
        topic.ChangeDisplayOrder(2);
        // Assert
        Assert.Equal(2, topic.DisplayOrder);
    }
    [Fact]
    public void ChangeDisplayOrder_WithInvalidValue_ThrowsDomainException()
    {
        // Arrange
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Topic Name",
            null,
            1);
        // Act
        var action = () => topic.ChangeDisplayOrder(0);
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
        var action = () => Topic.Create(
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
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Topic Name",
            null,
            1);

        // Act
        var action = () => topic.ChangeDisplayOrder(0);

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(1, topic.DisplayOrder);
    }
    [Fact]
    public void UpdateDescription_WithValidDescription_UpdatesDescription()
    {
        // Arrange
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Topic Name",
            null,
            1);
        // Act
        topic.UpdateDescription("New description");
        // Assert
        Assert.Equal("New description", topic.Description);
    }
    [Fact]
    public void UpdateDescription_WithNull_ClearsDescription()
    {
        // Arrange
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Topic Name",
            "Old description",
            1);
        // Act
        topic.UpdateDescription(null);
        // Assert
        Assert.Null(topic.Description);
    }
    [Fact]
    public void UpdateDescription_WithWhitespace_ClearsDescription()
    {
        // Arrange
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Topic Name",
            "Old description",
            1);
        // Act
        topic.UpdateDescription("   ");
        // Assert
        Assert.Null(topic.Description);
    }
    [Fact]
    public void UpdateDescription_WithSurroundingWhitespace_TrimsDescription()
    {
        // Arrange
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Topic Name",
            null,
            1);
        // Act
        topic.UpdateDescription("  New description  ");
        // Assert
        Assert.Equal("New description", topic.Description);
    }
    [Fact]
    public void UpdateDescription_WithDescriptionLongerThanMaximum_ThrowsDomainException()
    {
        // Arrange
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Topic Name",
            null,
            1);
        var longDescription = new string('A', 1001);
        // Act
        var action = () => topic.UpdateDescription(longDescription);
        // Assert
        var exception = Assert.Throws<DomainException>(action);
        Assert.Equal(
            "Topic description cannot exceed 1000 characters.",
            exception.Message);
    }
    [Fact]
    public void UpdateDescription_WithInvalidDescription_DoesNotChangeDescription()
    {
        // Arrange
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Topic Name",
            "Old description",
            1);
        var longDescription = new string('A', 1001);
        // Act
        var action = () => topic.UpdateDescription(longDescription);
        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal("Old description", topic.Description);
    }
    [Fact]
    public void Rename_WithNameAtMaximumLength_UpdatesName()
    {
        // Arrange
        var topic = Topic.Create(
            Guid.CreateVersion7(),
            "Existing name",
            null,
            1);

        var maximumLengthName = new string('A', 150);

        // Act
        topic.Rename(maximumLengthName);

        // Assert
        Assert.Equal(maximumLengthName, topic.Name);
    }
}
