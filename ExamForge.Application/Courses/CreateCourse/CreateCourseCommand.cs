using ExamForge.Application.Abstractions.Requests;
using ExamForge.Application.Common;

namespace ExamForge.Application.Courses.CreateCourse;

public record CreateCourseCommand : ICommand<Result<CreateCourseResponse>>
{
    public required string Name { get; init; }

    public string? Description { get; init; }
}

