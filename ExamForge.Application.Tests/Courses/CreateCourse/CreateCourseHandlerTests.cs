using ExamForge.Application.Common.Abstractions.Persistence;
using ExamForge.Application.Courses.CreateCourse;
using ExamForge.Application.Courses.Errors;
using ExamForge.Domain.Entities;
using NSubstitute;

namespace ExamForge.Application.Tests.Courses.CreateCourse;

public class CreateCourseHandlerTests
{
    private readonly ICourseRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    private readonly CreateCourseHandler _handler;

    public CreateCourseHandlerTests()
    {
        _repository = Substitute.For<ICourseRepository>();
        _unitOfWork = Substitute.For<IUnitOfWork>();

        _handler = new CreateCourseHandler(
            _repository,
            _unitOfWork);
    }

    [Fact]
    public async Task HandleAsync_ShouldCreateCourse_WhenNameIsUnique()
    {
        // Arrange
        var cancellationToken = new CancellationTokenSource().Token;
        var command = new CreateCourseCommand
        {
            Name = "Computer Science",
            Description = "OCR GCSE Computer Science."
        };
        _repository.ExistsByNameAsync(command.Name).Returns(false);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.False(result.IsFailure);
        Assert.Equal(command.Name, result.Value.Name);
        Assert.Equal(command.Description, result.Value.Description);
        Assert.NotEqual(Guid.Empty, result.Value.Id);
        await _repository.Received(1).AddAsync(Arg.Is<Course>(c => c.Name == command.Name && c.Description == command.Description));
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
    [Fact]
    public async Task HandleAsync_ShouldReturnFailure_WhenCourseNameAlreadyExists()
    {
        // Arrange
        var command = new CreateCourseCommand
        {
            Name = "Computer Science",
            Description = "OCR GCSE Computer Science."
        };

        _repository
            .ExistsByNameAsync(command.Name, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act
        var result = await _handler.HandleAsync(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.False(result.IsSuccess);
        Assert.Equal(CourseErrors.NameAlreadyExists, result.Error);

        await _repository
            .DidNotReceive()
            .AddAsync(Arg.Any<Course>(), Arg.Any<CancellationToken>());

        await _unitOfWork
            .DidNotReceive()
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}