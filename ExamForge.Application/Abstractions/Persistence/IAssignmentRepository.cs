using ExamForge.Domain.Entities;

namespace ExamForge.Application.Abstractions.Persistence;

public interface IAssignmentRepository
{
    Task<Assignment?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Assignment assignment,
        CancellationToken cancellationToken = default);
}