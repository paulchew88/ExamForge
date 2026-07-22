using ExamForge.Domain.Entities;

namespace ExamForge.Domain.Repositories;

public interface IQuestionRepository
{
    Task<Question?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Question question,
        CancellationToken cancellationToken = default);
}