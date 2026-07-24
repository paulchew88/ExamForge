using ExamForge.Application.Common;

namespace ExamForge.Application.Tests.Common;

public sealed class ResultTests
{
    [Fact]
    public void Success_Should_Create_Successful_Result()
    {
        // Act
        var result = Result.Success();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(Error.None, result.Error);
    }

    [Fact]
    public void Failure_Should_Create_Failed_Result()
    {
        // Arrange
        var error = new Error(
            "Course.NotFound",
            "The requested course could not be found.");

        // Act
        var result = Result.Failure(error);

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
            Result.Failure(Error.None));
    }
}