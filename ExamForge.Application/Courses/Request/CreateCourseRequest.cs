namespace ExamForge.Application.Courses.Request;

public sealed record CreateCourseRequest(
    string Name,
    string? Description);