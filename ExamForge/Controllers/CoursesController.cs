using ExamForge.Api.Contracts.Courses;
using Microsoft.AspNetCore.Mvc;


namespace ExamForge.Api.Controllers;

[ApiController]
[Route("api/courses")]
public sealed class CourseController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CourseController(ICourseService courseService)
    {
        _courseService = courseService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest request, CancellationToken cancellationToken)
    {
        var result = await _courseService.CreateAsync(
    new ExamForge.Application.Courses.Request.CreateCourseRequest(
        request.Name,
        request.Description),
        cancellationToken);


        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                "CourseAlreadyExists" => Conflict(result.Error.Message),
                _ => StatusCode(500, result.Error.Message)
            };
        }
        return Ok(result.Value);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseById(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid course ID.");
        }
        var result = await _courseService.GetByIdAsync(id, cancellationToken);

        if (result.IsFailure)
        {
            return result.Error.Code switch
            {
                "CourseNotFound" => NotFound(result.Error.Message),
                _ => StatusCode(500, result.Error.Message)
            };
        }
        return Ok(result.Value);


    }
}
