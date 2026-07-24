using ExamForge.Domain.Entities;

namespace ExamForge.Application.Abstractions.Persistence;

public interface ICourseRepository
{
    Task<Course?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Course course,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByNameAsync(
        string name,
        CancellationToken cancellationToken = default);
}