namespace ExamForge.Application.Courses.CreateCourse;

public record CreateCourseResponse
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }

    public string? Description { get; init; }
}
