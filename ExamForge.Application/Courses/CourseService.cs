using ExamForge.Application.Common;
using ExamForge.Application.Common.Abstractions.Persistence;
using ExamForge.Application.Courses.Dto;
using ExamForge.Application.Courses.Errors;
using ExamForge.Application.Courses.Request;
using ExamForge.Domain.Entities;

namespace ExamForge.Application.Courses;

public sealed class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CourseService(
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CourseDto>> CreateAsync(
        CreateCourseRequest request,
        CancellationToken cancellationToken)
    {
        var courseExists = await _courseRepository.ExistsByNameAsync(
            request.Name,
            cancellationToken);

        if (courseExists)
        {
            return CourseErrors.NameAlreadyExists;
        }

        var course = Course.Create(
            request.Name,
            request.Description);

        await _courseRepository.AddAsync(
            course,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return MapToDto(course);
    }

    public async Task<Result<CourseDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (course is null)
        {
            return CourseErrors.NotFound(id);
        }

        return MapToDto(course);
    }

    public async Task<Result> DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(
            id,
            cancellationToken);

        if (course is null)
        {
            return Result.Failure(CourseErrors.NotFound(id));
        }

        course.Deactivate();

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return Result.Success();
    }

    private static CourseDto MapToDto(Course course)
    {
        return new CourseDto(
            course.Id,
            course.Name,
            course.Description,
            course.IsActive,
            course.CreatedAt);
    }
}