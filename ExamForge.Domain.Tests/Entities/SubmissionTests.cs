using ExamForge.Domain.Entities;
using ExamForge.Domain.Enums;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Tests.Entities;

public class SubmissionTests
{
    [Fact]
    public void Create_WithValidValues_CreatesDraftSubmission()
    {
        // Arrange
        var assignmentId = Guid.CreateVersion7();
        var studentId = Guid.CreateVersion7();

        var beforeCreation = DateTimeOffset.UtcNow;

        // Act
        var submission = Submission.Create(
            assignmentId,
            studentId);

        var afterCreation = DateTimeOffset.UtcNow;

        // Assert
        Assert.NotEqual(Guid.Empty, submission.Id);
        Assert.Equal(assignmentId, submission.AssignmentId);
        Assert.Equal(studentId, submission.StudentId);
        Assert.Equal(SubmissionStatus.Draft, submission.Status);

        Assert.Null(submission.SubmittedAt);
        Assert.Null(submission.MarkedAt);
        Assert.Null(submission.ReleasedAt);

        Assert.InRange(
            submission.CreatedAt,
            beforeCreation,
            afterCreation);
    }

    [Fact]
    public void Create_WithEmptyAssignmentId_ThrowsDomainException()
    {
        // Act
        var action = () => Submission.Create(
            Guid.Empty,
            Guid.CreateVersion7());

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Assignment ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithEmptyStudentId_ThrowsDomainException()
    {
        // Act
        var action = () => Submission.Create(
            Guid.CreateVersion7(),
            Guid.Empty);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Student ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Submit_WhenDraft_ChangesStatusToSubmitted()
    {
        // Arrange
        var submission = CreateValidSubmission();

        // Act
        submission.Submit();

        // Assert
        Assert.Equal(
            SubmissionStatus.Submitted,
            submission.Status);
    }

    [Fact]
    public void Submit_WhenDraft_SetsSubmittedAt()
    {
        // Arrange
        var submission = CreateValidSubmission();
        var beforeSubmission = DateTimeOffset.UtcNow;

        // Act
        submission.Submit();

        var afterSubmission = DateTimeOffset.UtcNow;

        // Assert
        Assert.NotNull(submission.SubmittedAt);

        Assert.InRange(
            submission.SubmittedAt.Value,
            beforeSubmission,
            afterSubmission);
    }

    [Fact]
    public void Submit_WhenDraft_DoesNotSetLaterTimestamps()
    {
        // Arrange
        var submission = CreateValidSubmission();

        // Act
        submission.Submit();

        // Assert
        Assert.Null(submission.MarkedAt);
        Assert.Null(submission.ReleasedAt);
    }

    [Theory]
    [InlineData(SubmissionStatus.Submitted)]
    [InlineData(SubmissionStatus.Marked)]
    [InlineData(SubmissionStatus.Released)]
    public void Submit_WhenNotDraft_ThrowsDomainException(
        SubmissionStatus status)
    {
        // Arrange
        var submission = CreateSubmissionWithStatus(status);
        var originalSubmittedAt = submission.SubmittedAt;
        var originalMarkedAt = submission.MarkedAt;
        var originalReleasedAt = submission.ReleasedAt;

        // Act
        var action = submission.Submit;

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Only draft submissions can be submitted.",
            exception.Message);

        Assert.Equal(status, submission.Status);
        Assert.Equal(originalSubmittedAt, submission.SubmittedAt);
        Assert.Equal(originalMarkedAt, submission.MarkedAt);
        Assert.Equal(originalReleasedAt, submission.ReleasedAt);
    }

    [Fact]
    public void Mark_WhenSubmitted_ChangesStatusToMarked()
    {
        // Arrange
        var submission = CreateSubmittedSubmission();

        // Act
        submission.Mark();

        // Assert
        Assert.Equal(
            SubmissionStatus.Marked,
            submission.Status);
    }

    [Fact]
    public void Mark_WhenSubmitted_SetsMarkedAt()
    {
        // Arrange
        var submission = CreateSubmittedSubmission();
        var beforeMarking = DateTimeOffset.UtcNow;

        // Act
        submission.Mark();

        var afterMarking = DateTimeOffset.UtcNow;

        // Assert
        Assert.NotNull(submission.MarkedAt);

        Assert.InRange(
            submission.MarkedAt.Value,
            beforeMarking,
            afterMarking);
    }

    [Fact]
    public void Mark_WhenSubmitted_PreservesSubmittedAt()
    {
        // Arrange
        var submission = CreateSubmittedSubmission();
        var originalSubmittedAt = submission.SubmittedAt;

        // Act
        submission.Mark();

        // Assert
        Assert.Equal(
            originalSubmittedAt,
            submission.SubmittedAt);
    }

    [Fact]
    public void Mark_WhenSubmitted_DoesNotSetReleasedAt()
    {
        // Arrange
        var submission = CreateSubmittedSubmission();

        // Act
        submission.Mark();

        // Assert
        Assert.Null(submission.ReleasedAt);
    }

    [Theory]
    [InlineData(SubmissionStatus.Draft)]
    [InlineData(SubmissionStatus.Marked)]
    [InlineData(SubmissionStatus.Released)]
    public void Mark_WhenNotSubmitted_ThrowsDomainException(
        SubmissionStatus status)
    {
        // Arrange
        var submission = CreateSubmissionWithStatus(status);
        var originalSubmittedAt = submission.SubmittedAt;
        var originalMarkedAt = submission.MarkedAt;
        var originalReleasedAt = submission.ReleasedAt;

        // Act
        var action = submission.Mark;

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Only submitted submissions can be marked.",
            exception.Message);

        Assert.Equal(status, submission.Status);
        Assert.Equal(originalSubmittedAt, submission.SubmittedAt);
        Assert.Equal(originalMarkedAt, submission.MarkedAt);
        Assert.Equal(originalReleasedAt, submission.ReleasedAt);
    }

    [Fact]
    public void Release_WhenMarked_ChangesStatusToReleased()
    {
        // Arrange
        var submission = CreateMarkedSubmission();

        // Act
        submission.Release();

        // Assert
        Assert.Equal(
            SubmissionStatus.Released,
            submission.Status);
    }

    [Fact]
    public void Release_WhenMarked_SetsReleasedAt()
    {
        // Arrange
        var submission = CreateMarkedSubmission();
        var beforeRelease = DateTimeOffset.UtcNow;

        // Act
        submission.Release();

        var afterRelease = DateTimeOffset.UtcNow;

        // Assert
        Assert.NotNull(submission.ReleasedAt);

        Assert.InRange(
            submission.ReleasedAt.Value,
            beforeRelease,
            afterRelease);
    }

    [Fact]
    public void Release_WhenMarked_PreservesEarlierTimestamps()
    {
        // Arrange
        var submission = CreateMarkedSubmission();
        var originalSubmittedAt = submission.SubmittedAt;
        var originalMarkedAt = submission.MarkedAt;

        // Act
        submission.Release();

        // Assert
        Assert.Equal(
            originalSubmittedAt,
            submission.SubmittedAt);

        Assert.Equal(
            originalMarkedAt,
            submission.MarkedAt);
    }

    [Theory]
    [InlineData(SubmissionStatus.Draft)]
    [InlineData(SubmissionStatus.Submitted)]
    [InlineData(SubmissionStatus.Released)]
    public void Release_WhenNotMarked_ThrowsDomainException(
        SubmissionStatus status)
    {
        // Arrange
        var submission = CreateSubmissionWithStatus(status);
        var originalSubmittedAt = submission.SubmittedAt;
        var originalMarkedAt = submission.MarkedAt;
        var originalReleasedAt = submission.ReleasedAt;

        // Act
        var action = submission.Release;

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Only marked submissions can be released.",
            exception.Message);

        Assert.Equal(status, submission.Status);
        Assert.Equal(originalSubmittedAt, submission.SubmittedAt);
        Assert.Equal(originalMarkedAt, submission.MarkedAt);
        Assert.Equal(originalReleasedAt, submission.ReleasedAt);
    }

    [Fact]
    public void ReturnToDraft_WhenSubmitted_ChangesStatusToDraft()
    {
        // Arrange
        var submission = CreateSubmittedSubmission();

        // Act
        submission.ReturnToDraft();

        // Assert
        Assert.Equal(
            SubmissionStatus.Draft,
            submission.Status);
    }

    [Fact]
    public void ReturnToDraft_WhenSubmitted_ClearsSubmittedAt()
    {
        // Arrange
        var submission = CreateSubmittedSubmission();

        Assert.NotNull(submission.SubmittedAt);

        // Act
        submission.ReturnToDraft();

        // Assert
        Assert.Null(submission.SubmittedAt);
    }

    [Fact]
    public void ReturnToDraft_WhenSubmitted_LeavesLaterTimestampsNull()
    {
        // Arrange
        var submission = CreateSubmittedSubmission();

        // Act
        submission.ReturnToDraft();

        // Assert
        Assert.Null(submission.MarkedAt);
        Assert.Null(submission.ReleasedAt);
    }

    [Theory]
    [InlineData(SubmissionStatus.Draft)]
    [InlineData(SubmissionStatus.Marked)]
    [InlineData(SubmissionStatus.Released)]
    public void ReturnToDraft_WhenNotSubmitted_ThrowsDomainException(
        SubmissionStatus status)
    {
        // Arrange
        var submission = CreateSubmissionWithStatus(status);
        var originalSubmittedAt = submission.SubmittedAt;
        var originalMarkedAt = submission.MarkedAt;
        var originalReleasedAt = submission.ReleasedAt;

        // Act
        var action = submission.ReturnToDraft;

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Only submitted submissions can be returned to draft.",
            exception.Message);

        Assert.Equal(status, submission.Status);
        Assert.Equal(originalSubmittedAt, submission.SubmittedAt);
        Assert.Equal(originalMarkedAt, submission.MarkedAt);
        Assert.Equal(originalReleasedAt, submission.ReleasedAt);
    }

    [Fact]
    public void ReturnedSubmission_CanBeSubmittedAgain()
    {
        // Arrange
        var submission = CreateSubmittedSubmission();

        submission.ReturnToDraft();

        var beforeResubmission = DateTimeOffset.UtcNow;

        // Act
        submission.Submit();

        var afterResubmission = DateTimeOffset.UtcNow;

        // Assert
        Assert.Equal(
            SubmissionStatus.Submitted,
            submission.Status);

        Assert.NotNull(submission.SubmittedAt);

        Assert.InRange(
            submission.SubmittedAt.Value,
            beforeResubmission,
            afterResubmission);
    }

    [Fact]
    public void CompleteWorkflow_TransitionsThroughAllExpectedStates()
    {
        // Arrange
        var submission = CreateValidSubmission();

        // Act and assert: Draft → Submitted
        submission.Submit();

        Assert.Equal(
            SubmissionStatus.Submitted,
            submission.Status);

        Assert.NotNull(submission.SubmittedAt);

        // Act and assert: Submitted → Marked
        submission.Mark();

        Assert.Equal(
            SubmissionStatus.Marked,
            submission.Status);

        Assert.NotNull(submission.MarkedAt);

        // Act and assert: Marked → Released
        submission.Release();

        Assert.Equal(
            SubmissionStatus.Released,
            submission.Status);

        Assert.NotNull(submission.ReleasedAt);
    }

    [Fact]
    public void CompleteWorkflow_SetsTimestampsInChronologicalOrder()
    {
        // Arrange
        var submission = CreateValidSubmission();

        // Act
        submission.Submit();
        submission.Mark();
        submission.Release();

        // Assert
        Assert.NotNull(submission.SubmittedAt);
        Assert.NotNull(submission.MarkedAt);
        Assert.NotNull(submission.ReleasedAt);

        Assert.True(
            submission.SubmittedAt <= submission.MarkedAt);

        Assert.True(
            submission.MarkedAt <= submission.ReleasedAt);
    }

    private static Submission CreateValidSubmission()
    {
        return Submission.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7());
    }

    private static Submission CreateSubmittedSubmission()
    {
        var submission = CreateValidSubmission();
        submission.Submit();

        return submission;
    }

    private static Submission CreateMarkedSubmission()
    {
        var submission = CreateSubmittedSubmission();
        submission.Mark();

        return submission;
    }

    private static Submission CreateReleasedSubmission()
    {
        var submission = CreateMarkedSubmission();
        submission.Release();

        return submission;
    }

    private static Submission CreateSubmissionWithStatus(
        SubmissionStatus status)
    {
        return status switch
        {
            SubmissionStatus.Draft =>
                CreateValidSubmission(),

            SubmissionStatus.Submitted =>
                CreateSubmittedSubmission(),

            SubmissionStatus.Marked =>
                CreateMarkedSubmission(),

            SubmissionStatus.Released =>
                CreateReleasedSubmission(),

            _ => throw new ArgumentOutOfRangeException(
                nameof(status),
                status,
                "Unsupported submission status.")
        };
    }
}