using ExamForge.Application.Common;
namespace ExamForge.Application.Courses.Errors;

public static class CourseErrors
{
    public static readonly Error NameAlreadyExists = new("Course.NameAlreadyExists", "A course with the same name already exists.");
    public static Error NotFound(Guid courseId) => new("Course.NotFound", $"Course '{courseId}' could not be found.");

}
