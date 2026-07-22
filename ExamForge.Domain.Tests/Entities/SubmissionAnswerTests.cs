using ExamForge.Domain.Entities;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Tests.Entities;

public class SubmissionAnswerTests
{
    private static SubmissionAnswer CreateValidAnswer()
    {
        return SubmissionAnswer.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            "Initial answer");
    }

    private static SubmissionAnswer CreateMarkedAnswer()
    {
        var answer = CreateValidAnswer();

        answer.Mark(
            awardedMarks: 3,
            feedback: "Good attempt.");

        return answer;
    }
    [Fact]
    public void Create_WithValidValues_CreatesUnmarkedAnswer()
    {
        // Arrange
        var submissionId = Guid.CreateVersion7();
        var assignmentQuestionId = Guid.CreateVersion7();

        var beforeCreation = DateTimeOffset.UtcNow;

        // Act
        var answer = SubmissionAnswer.Create(
            submissionId,
            assignmentQuestionId,
            "Binary search repeatedly divides the search space.");

        var afterCreation = DateTimeOffset.UtcNow;

        // Assert
        Assert.NotEqual(Guid.Empty, answer.Id);
        Assert.Equal(submissionId, answer.SubmissionId);
        Assert.Equal(
            assignmentQuestionId,
            answer.AssignmentQuestionId);

        Assert.Equal(
            "Binary search repeatedly divides the search space.",
            answer.Answer);

        Assert.Null(answer.AwardedMarks);
        Assert.Null(answer.Feedback);
        Assert.Null(answer.MarkedAt);

        Assert.InRange(
            answer.CreatedAt,
            beforeCreation,
            afterCreation);

        Assert.Equal(
            answer.CreatedAt,
            answer.LastUpdatedAt);
    }

    [Fact]
    public void Create_WithEmptySubmissionId_ThrowsDomainException()
    {
        // Act
        var action = () => SubmissionAnswer.Create(
            Guid.Empty,
            Guid.CreateVersion7(),
            "Valid answer");

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Submission ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithEmptyAssignmentQuestionId_ThrowsDomainException()
    {
        // Act
        var action = () => SubmissionAnswer.Create(
            Guid.CreateVersion7(),
            Guid.Empty,
            "Valid answer");

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Assignment question ID cannot be empty.",
            exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithEmptyAnswer_ThrowsDomainException(
        string? answerText)
    {
        // Act
        var action = () => SubmissionAnswer.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            answerText!);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Answer cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_TrimsAnswer()
    {
        // Act
        var answer = SubmissionAnswer.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            "  Valid answer  ");

        // Assert
        Assert.Equal("Valid answer", answer.Answer);
    }

    [Fact]
    public void Create_WithAnswerAtMaximumLength_Succeeds()
    {
        // Arrange
        var answerText = new string(
            'A',
            SubmissionAnswer.MaxAnswerLength);

        // Act
        var answer = SubmissionAnswer.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            answerText);

        // Assert
        Assert.Equal(answerText, answer.Answer);
    }

    [Fact]
    public void Create_WithAnswerOverMaximumLength_ThrowsDomainException()
    {
        // Arrange
        var answerText = new string(
            'A',
            SubmissionAnswer.MaxAnswerLength + 1);

        // Act
        var action = () => SubmissionAnswer.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            answerText);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            $"Answer cannot exceed {SubmissionAnswer.MaxAnswerLength} characters.",
            exception.Message);
    }

    [Fact]
    public void Create_TrimsAnswerBeforeCheckingMaximumLength()
    {
        // Arrange
        var answerText =
            $"  {new string('A', SubmissionAnswer.MaxAnswerLength)}  ";

        // Act
        var answer = SubmissionAnswer.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            answerText);

        // Assert
        Assert.Equal(
            SubmissionAnswer.MaxAnswerLength,
            answer.Answer.Length);
    }

    [Fact]
    public void UpdateAnswer_WithValidValue_ReplacesAnswer()
    {
        // Arrange
        var answer = CreateValidAnswer();

        // Act
        answer.UpdateAnswer("Updated response");

        // Assert
        Assert.Equal("Updated response", answer.Answer);
    }

    [Fact]
    public void UpdateAnswer_TrimsAnswer()
    {
        // Arrange
        var answer = CreateValidAnswer();

        // Act
        answer.UpdateAnswer("  Updated response  ");

        // Assert
        Assert.Equal("Updated response", answer.Answer);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void UpdateAnswer_WithEmptyAnswer_ThrowsAndPreservesState(
        string? newAnswer)
    {
        // Arrange
        var answer = CreateValidAnswer();
        var originalAnswer = answer.Answer;
        var originalLastUpdatedAt = answer.LastUpdatedAt;

        // Act
        var action = () => answer.UpdateAnswer(newAnswer!);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Answer cannot be empty.",
            exception.Message);

        Assert.Equal(originalAnswer, answer.Answer);
        Assert.Equal(
            originalLastUpdatedAt,
            answer.LastUpdatedAt);
    }

    [Fact]
    public void UpdateAnswer_WithAnswerOverMaximumLength_ThrowsAndPreservesState()
    {
        // Arrange
        var answer = CreateValidAnswer();

        var originalAnswer = answer.Answer;
        var originalLastUpdatedAt = answer.LastUpdatedAt;

        var newAnswer = new string(
            'A',
            SubmissionAnswer.MaxAnswerLength + 1);

        // Act
        var action = () => answer.UpdateAnswer(newAnswer);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            $"Answer cannot exceed {SubmissionAnswer.MaxAnswerLength} characters.",
            exception.Message);

        Assert.Equal(originalAnswer, answer.Answer);
        Assert.Equal(
            originalLastUpdatedAt,
            answer.LastUpdatedAt);
    }

    [Fact]
    public void UpdateAnswer_UpdatesLastUpdatedAt()
    {
        // Arrange
        var answer = CreateValidAnswer();

        var beforeUpdate = DateTimeOffset.UtcNow;

        // Act
        answer.UpdateAnswer("Updated response");

        var afterUpdate = DateTimeOffset.UtcNow;

        // Assert
        Assert.InRange(
            answer.LastUpdatedAt,
            beforeUpdate,
            afterUpdate);
    }

    [Fact]
    public void UpdateAnswer_DoesNotChangeCreatedAt()
    {
        // Arrange
        var answer = CreateValidAnswer();
        var originalCreatedAt = answer.CreatedAt;

        // Act
        answer.UpdateAnswer("Updated response");

        // Assert
        Assert.Equal(originalCreatedAt, answer.CreatedAt);
    }


    [Fact]
    public void Mark_WithNullFeedback_StoresNull()
    {
        // Arrange
        var answer = CreateValidAnswer();

        // Act
        answer.Mark(
            awardedMarks: 3,
            feedback: null);

        // Assert
        Assert.Equal(3, answer.AwardedMarks);
        Assert.Null(answer.Feedback);
        Assert.NotNull(answer.MarkedAt);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Mark_WithEmptyFeedback_StoresNull(string feedback)
    {
        // Arrange
        var answer = CreateValidAnswer();

        // Act
        answer.Mark(
            awardedMarks: 3,
            feedback);

        // Assert
        Assert.Equal(3, answer.AwardedMarks);
        Assert.Null(answer.Feedback);
        Assert.NotNull(answer.MarkedAt);
    }

    [Fact]
    public void Mark_TrimsFeedback()
    {
        // Arrange
        var answer = CreateValidAnswer();

        // Act
        answer.Mark(
            awardedMarks: 3,
            feedback: "  Good explanation.  ");

        // Assert
        Assert.Equal(
            "Good explanation.",
            answer.Feedback);
    }

    [Fact]
    public void Mark_WithFeedbackAtMaximumLength_Succeeds()
    {
        // Arrange
        var answer = CreateValidAnswer();

        var feedback = new string(
            'A',
            SubmissionAnswer.MaxFeedbackLength);

        // Act
        answer.Mark(
            awardedMarks: 3,
            feedback);

        // Assert
        Assert.Equal(feedback, answer.Feedback);
    }

    [Fact]
    public void Mark_WithFeedbackOverMaximumLength_ThrowsAndPreservesState()
    {
        // Arrange
        var answer = CreateValidAnswer();

        var feedback = new string(
            'A',
            SubmissionAnswer.MaxFeedbackLength + 1);

        // Act
        var action = () => answer.Mark(
            awardedMarks: 3,
            feedback);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            $"Feedback cannot exceed {SubmissionAnswer.MaxFeedbackLength} characters.",
            exception.Message);

        Assert.Null(answer.AwardedMarks);
        Assert.Null(answer.Feedback);
        Assert.Null(answer.MarkedAt);
    }
    [Fact]
    public void Mark_SetsMarkedAt()
    {
        // Arrange
        var answer = CreateValidAnswer();
        var beforeMarking = DateTimeOffset.UtcNow;

        // Act
        answer.Mark(
            awardedMarks: 3,
            feedback: null);

        var afterMarking = DateTimeOffset.UtcNow;

        // Assert
        Assert.NotNull(answer.MarkedAt);

        Assert.InRange(
            answer.MarkedAt.Value,
            beforeMarking,
            afterMarking);
    }
    [Fact]
    public void Mark_WhenAlreadyMarked_ReplacesPreviousMark()
    {
        // Arrange
        var answer = CreateValidAnswer();

        answer.Mark(
            awardedMarks: 3,
            feedback: "Initial feedback.");

        // Act
        answer.Mark(
            awardedMarks: 5,
            feedback: "Updated feedback.");

        // Assert
        Assert.Equal(5, answer.AwardedMarks);

        Assert.Equal(
            "Updated feedback.",
            answer.Feedback);

        Assert.NotNull(answer.MarkedAt);
    }

    [Fact]
    public void Mark_WhenAlreadyMarked_UpdatesMarkedAt()
    {
        // Arrange
        var answer = CreateValidAnswer();

        answer.Mark(
            awardedMarks: 3,
            feedback: "Initial feedback.");

        var originalMarkedAt = answer.MarkedAt;
        var beforeRemarking = DateTimeOffset.UtcNow;

        // Act
        answer.Mark(
            awardedMarks: 5,
            feedback: "Updated feedback.");

        var afterRemarking = DateTimeOffset.UtcNow;

        // Assert
        Assert.NotNull(answer.MarkedAt);

        Assert.InRange(
            answer.MarkedAt.Value,
            beforeRemarking,
            afterRemarking);

        Assert.True(
            answer.MarkedAt >= originalMarkedAt);
    }

    [Fact]
    public void UpdateAnswer_WhenMarked_ClearsMarkingData()
    {
        // Arrange
        var answer = CreateValidAnswer();

        answer.Mark(
            awardedMarks: 3,
            feedback: "Good attempt.");

        // Act
        answer.UpdateAnswer("Updated response");

        // Assert
        Assert.Null(answer.AwardedMarks);
        Assert.Null(answer.Feedback);
        Assert.Null(answer.MarkedAt);
    }

    [Fact]
    public void Mark_WhenReplacementValidationFails_PreservesExistingMark()
    {
        // Arrange
        var answer = CreateValidAnswer();

        answer.Mark(
            awardedMarks: 3,
            feedback: "Initial feedback.");

        var originalAwardedMarks = answer.AwardedMarks;
        var originalFeedback = answer.Feedback;
        var originalMarkedAt = answer.MarkedAt;

        // Act
        var action = () => answer.Mark(
            awardedMarks: -1,
            feedback: "Invalid replacement.");

        // Assert
        Assert.Throws<DomainException>(action);

        Assert.Equal(
            originalAwardedMarks,
            answer.AwardedMarks);

        Assert.Equal(
            originalFeedback,
            answer.Feedback);

        Assert.Equal(
            originalMarkedAt,
            answer.MarkedAt);
    }
    [Fact]
    public void Mark_DoesNotChangeAnswerOrLastUpdatedAt()
    {
        // Arrange
        var answer = CreateValidAnswer();

        var originalAnswer = answer.Answer;
        var originalLastUpdatedAt = answer.LastUpdatedAt;

        // Act
        answer.Mark(
            awardedMarks: 3,
            feedback: null);

        // Assert
        Assert.Equal(
            originalAnswer,
            answer.Answer);

        Assert.Equal(
            originalLastUpdatedAt,
            answer.LastUpdatedAt);
    }

    [Fact]
    public void Mark_TrimsFeedbackBeforeCheckingMaximumLength()
    {
        // Arrange
        var answer = CreateValidAnswer();

        var feedback =
            $"  {new string('A', SubmissionAnswer.MaxFeedbackLength)}  ";

        // Act
        answer.Mark(
            awardedMarks: 3,
            feedback);

        // Assert
        Assert.Equal(
            SubmissionAnswer.MaxFeedbackLength,
            answer.Feedback!.Length);
    }










    [Fact]
    public void ClearMark_DoesNotChangeAnswerOrLastUpdatedAt()
    {
        // Arrange
        var assignmentQuestion = CreateAssignmentQuestion();

        var answer = CreateValidAnswer(
            assignmentQuestion.Id);

        answer.Mark(
            awardedMarks: 3,

            feedback: "Good attempt.");

        var originalAnswer = answer.Answer;
        var originalLastUpdatedAt = answer.LastUpdatedAt;

        // Act
        answer.ClearMark();

        // Assert
        Assert.Equal(originalAnswer, answer.Answer);

        Assert.Equal(
            originalLastUpdatedAt,
            answer.LastUpdatedAt);
    }

    [Fact]
    public void ClearMark_WhenAlreadyUnmarked_RemainsUnmarked()
    {
        // Arrange
        var answer = CreateValidAnswer();

        // Act
        answer.ClearMark();

        // Assert
        Assert.Null(answer.AwardedMarks);
        Assert.Null(answer.Feedback);
        Assert.Null(answer.MarkedAt);
    }

    [Fact]
    public void CompleteWorkflow_CreateUpdateMarkClearAndRemark_Works()
    {
        // Arrange
        var assignmentQuestion = CreateAssignmentQuestion(
            maximumMarks: 5);

        var answer = CreateValidAnswer(
            assignmentQuestion.Id);

        // Act and assert: update
        answer.UpdateAnswer("Updated answer");

        Assert.Equal("Updated answer", answer.Answer);
        Assert.Null(answer.AwardedMarks);
        Assert.Null(answer.MarkedAt);

        // Act and assert: mark
        answer.Mark(
            awardedMarks: 3,

            feedback: "Mostly correct.");

        Assert.Equal(3, answer.AwardedMarks);
        Assert.Equal("Mostly correct.", answer.Feedback);
        Assert.NotNull(answer.MarkedAt);

        // Act and assert: clear
        answer.ClearMark();

        Assert.Null(answer.AwardedMarks);
        Assert.Null(answer.Feedback);
        Assert.Null(answer.MarkedAt);

        // Act and assert: remark
        answer.Mark(
            awardedMarks: 4,

            feedback: "Award increased after review.");

        Assert.Equal(4, answer.AwardedMarks);

        Assert.Equal(
            "Award increased after review.",
            answer.Feedback);

        Assert.NotNull(answer.MarkedAt);
    }

    private static SubmissionAnswer CreateValidAnswer(
        Guid? assignmentQuestionId = null)
    {
        return SubmissionAnswer.Create(
            Guid.CreateVersion7(),
            assignmentQuestionId ?? Guid.CreateVersion7(),
            "Initial answer");
    }

    private static AssignmentQuestion CreateAssignmentQuestion(
        int maximumMarks = 5)
    {
        return AssignmentQuestion.Create(
            Guid.CreateVersion7(),
            Guid.CreateVersion7(),
            order: 1,
            maximumMarks);
    }
}