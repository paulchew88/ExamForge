using ExamForge.Application.Common;
using ExamForge.Application.Courses.Dto;
using ExamForge.Application.Courses.Request;

public interface ICourseService
{
    Task<Result<CourseDto>> CreateAsync(
        CreateCourseRequest request,
        CancellationToken cancellationToken);

    Task<Result<CourseDto>> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<Result> DeactivateAsync(
        Guid id,
        CancellationToken cancellationToken);
}