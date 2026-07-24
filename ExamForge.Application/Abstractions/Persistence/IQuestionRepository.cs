using ExamForge.Domain.Entities;

namespace ExamForge.Application.Abstractions.Persistence;

public interface IQuestionRepository
{
    Task<Question?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Question question,
        CancellationToken cancellationToken = default);
}