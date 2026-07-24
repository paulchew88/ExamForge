using ExamForge.Application.Common;

namespace ExamForge.Application.Tests.Common;

public sealed class ResultOfTTests
{
    [Fact]
    public void Success_Should_Create_Successful_Result_With_Value()
    {
        // Arrange
        const string value = "Hello";

        // Act
        var result = Result<string>.Success(value);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);
        Assert.Equal(value, result.Value);
    }

    [Fact]
    public void Failure_Should_Create_Failed_Result()
    {
        // Arrange
        var error = new Error(
            "Course.NotFound",
            "The requested course could not be found.");

        // Act
        var result = Result<string>.Failure(error);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }

    [Fact]
    public void Failure_Should_Throw_When_Error_Is_None()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() =>
            Result<string>.Failure(Error.None));
    }

    [Fact]
    public void Value_Should_Throw_When_Result_Is_Failure()
    {
        // Arrange
        var result = Result<string>.Failure(
            new Error(
                "Course.NotFound",
                "The requested course could not be found."));

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() =>
        {
            _ = result.Value;
        });
    }

    [Fact]
    public void Implicit_Conversion_From_Value_Should_Create_Success()
    {
        // Arrange
        Result<string> result = "Hello";

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Hello", result.Value);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Implicit_Conversion_From_Error_Should_Create_Failure()
    {
        // Arrange
        var error = new Error(
            "Course.NotFound",
            "The requested course could not be found.");

        // Act
        Result<string> result = error;

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsFailure);
        Assert.Equal(error, result.Error);
    }
}