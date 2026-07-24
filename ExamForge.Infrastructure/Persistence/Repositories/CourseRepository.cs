using ExamForge.Application.Common.Abstractions.Persistence;
using ExamForge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExamForge.Infrastructure.Persistence.Repositories;

public sealed class CourseRepository : ICourseRepository
{
    private readonly ExamForgeDbContext _dbContext;

    public CourseRepository(ExamForgeDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Course?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Courses.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task AddAsync(Course course, CancellationToken cancellationToken = default)
    {
        await _dbContext.Courses.AddAsync(course, cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return _dbContext.Courses.AnyAsync(c => c.Name == name, cancellationToken);
    }
}
