using ExamForge.Domain.Entities;

namespace ExamForge.Application.Abstractions.Persistence;

public interface ISubmissionRepository
{
    Task<Submission?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Submission submission,
        CancellationToken cancellationToken = default);

    Task<Submission?> GetByAssignmentAndStudentAsync(
        Guid assignmentId,
        Guid studentId,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid assignmentId,
        Guid studentId,
        CancellationToken cancellationToken = default);
}