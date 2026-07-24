using ExamForge.Application.Common;

namespace ExamForge.Application.Tests.Common;

public sealed class ErrorTests
{
    [Fact]
    public void Constructor_Should_Set_Code_And_Message()
    {
        // Arrange
        const string code = "Course.NotFound";
        const string message = "The requested course could not be found.";

        // Act
        var error = new Error(code, message);

        // Assert
        Assert.Equal(code, error.Code);
        Assert.Equal(message, error.Message);
    }

    [Fact]
    public void None_Should_Have_Empty_Code_And_Message()
    {
        // Arrange
        var error = Error.None;

        // Assert
        Assert.Equal(string.Empty, error.Code);
        Assert.Equal(string.Empty, error.Message);
    }

    [Fact]
    public void ToString_Should_Return_Code_And_Message()
    {
        // Arrange
        var error = new Error(
            "Course.NotFound",
            "The requested course could not be found.");

        // Act
        var result = error.ToString();

        // Assert
        Assert.Equal(
            "Course.NotFound: The requested course could not be found.",
            result);
    }
}