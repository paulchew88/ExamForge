namespace ExamForge.Api.Contracts.Courses;

public sealed record CreateCourseRequest
{
    public required string Name { get; init; }

    public string? Description { get; init; }
    public CreateCourseRequest() { }
    public CreateCourseRequest(string name, string? description)
    {
        Name = name;
        Description = description;
    }
}
