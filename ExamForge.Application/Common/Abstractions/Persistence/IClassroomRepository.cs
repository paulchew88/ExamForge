using ExamForge.Domain.Entities;

namespace ExamForge.Application.Common.Abstractions.Persistence;

public interface IClassroomRepository
{
    Task<Classroom?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Classroom classroom,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByJoinCodeAsync(
        string joinCode,
        CancellationToken cancellationToken = default);
}