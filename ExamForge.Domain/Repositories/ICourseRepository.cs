using ExamForge.Domain.Entities;

namespace ExamForge.Domain.Repositories;

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