namespace ExamForge.Application.Courses.Dto;

public sealed record CourseDto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedAt);
