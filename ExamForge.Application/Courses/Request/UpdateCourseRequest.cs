namespace ExamForge.Application.Courses.Request;

public sealed record UpdateCourseRequest(
    string Name,
    string? Description);