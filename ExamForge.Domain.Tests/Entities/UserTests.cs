using ExamForge.Domain.Entities;
using ExamForge.Domain.Enums;
using ExamForge.Domain.Exceptions;

namespace ExamForge.Domain.Tests.Entities;

public class UserTests
{
    [Fact]
    public void Create_WithValidValues_CreatesPendingActiveUser()
    {
        // Arrange
        var externalIdentityId = Guid.CreateVersion7();

        // Act
        var user = User.Create(
            externalIdentityId,
            "Paul",
            "Chew",
            "paul@example.com",
            UserRole.Teacher);

        // Assert
        Assert.NotEqual(Guid.Empty, user.Id);
        Assert.Equal(externalIdentityId, user.ExternalIdentityId);
        Assert.Equal(externalIdentityId, user.ExternalIdentityId);
        Assert.Equal("Paul", user.FirstName);
        Assert.Equal("Chew", user.LastName);
        Assert.Equal("paul@example.com", user.Email);
        Assert.Equal(UserRole.Teacher, user.Role);
        Assert.Equal(UserStatus.Pending, user.Status);
        Assert.True(user.IsActive);
        Assert.NotEqual(default, user.CreatedAt);
    }

    [Fact]
    public void Create_WithEmptyIdentityProviderId_ThrowsDomainException()
    {
        // Act
        var action = () => User.Create(
            Guid.Empty,
            "Paul",
            "Chew",
            "paul@example.com",
            UserRole.Teacher);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Identity provider ID cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_TrimsNames()
    {
        // Act
        var user = User.Create(
            Guid.CreateVersion7(),
            "  Paul  ",
            "  Chew  ",
            "paul@example.com",
            UserRole.Teacher);

        // Assert
        Assert.Equal("Paul", user.FirstName);
        Assert.Equal("Chew", user.LastName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidFirstName_ThrowsDomainException(
        string? firstName)
    {
        // Act
        var action = () => User.Create(
            Guid.CreateVersion7(),
            firstName!,
            "Chew",
            "paul@example.com",
            UserRole.Teacher);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "First name cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithFirstNameAtMaximumLength_Succeeds()
    {
        // Arrange
        var firstName = new string(
            'A',
            User.MaxFirstNameLength);

        // Act
        var user = User.Create(
            Guid.CreateVersion7(),
            firstName,
            "Chew",
            "paul@example.com",
            UserRole.Teacher);

        // Assert
        Assert.Equal(firstName, user.FirstName);
    }

    [Fact]
    public void Create_WithFirstNameOverMaximumLength_ThrowsDomainException()
    {
        // Arrange
        var firstName = new string(
            'A',
            User.MaxFirstNameLength + 1);

        // Act
        var action = () => User.Create(
            Guid.CreateVersion7(),
            firstName,
            "Chew",
            "paul@example.com",
            UserRole.Teacher);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            $"First name cannot exceed {User.MaxFirstNameLength} characters.",
            exception.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithInvalidLastName_ThrowsDomainException(
        string? lastName)
    {
        // Act
        var action = () => User.Create(
            Guid.CreateVersion7(),
            "Paul",
            lastName!,
            "paul@example.com",
            UserRole.Teacher);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Last name cannot be empty.",
            exception.Message);
    }

    [Fact]
    public void Create_WithLastNameAtMaximumLength_Succeeds()
    {
        // Arrange
        var lastName = new string(
            'A',
            User.MaxLastNameLength);

        // Act
        var user = User.Create(
            Guid.CreateVersion7(),
            "Paul",
            lastName,
            "paul@example.com",
            UserRole.Teacher);

        // Assert
        Assert.Equal(lastName, user.LastName);
    }

    [Fact]
    public void Create_WithLastNameOverMaximumLength_ThrowsDomainException()
    {
        // Arrange
        var lastName = new string(
            'A',
            User.MaxLastNameLength + 1);

        // Act
        var action = () => User.Create(
            Guid.CreateVersion7(),
            "Paul",
            lastName,
            "paul@example.com",
            UserRole.Teacher);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            $"Last name cannot exceed {User.MaxLastNameLength} characters.",
            exception.Message);
    }

    [Fact]
    public void Create_TrimsAndNormalisesEmail()
    {
        // Act
        var user = User.Create(
            Guid.CreateVersion7(),
            "Paul",
            "Chew",
            "  PAUL@EXAMPLE.COM  ",
            UserRole.Teacher);

        // Assert
        Assert.Equal(
            "paul@example.com",
            user.Email);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Create_WithMissingEmail_ThrowsDomainException(
        string? email)
    {
        // Act
        var action = () => User.Create(
            Guid.CreateVersion7(),
            "Paul",
            "Chew",
            email!,
            UserRole.Teacher);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Email cannot be empty.",
            exception.Message);
    }

    [Theory]
    [InlineData("not-an-email")]
    [InlineData("paul@")]
    [InlineData("@example.com")]
    public void Create_WithInvalidEmail_ThrowsDomainException(
        string email)
    {
        // Act
        var action = () => User.Create(
            Guid.CreateVersion7(),
            "Paul",
            "Chew",
            email,
            UserRole.Teacher);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Email is not valid.",
            exception.Message);
    }

    [Fact]
    public void Create_WithEmailOverMaximumLength_ThrowsDomainException()
    {
        // Arrange
        var email = $"{new string('a', 309)}@example.com";

        // Act
        var action = () => User.Create(
            Guid.CreateVersion7(),
            "Paul",
            "Chew",
            email,
            UserRole.Teacher);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            $"Email cannot exceed {User.MaxEmailLength} characters.",
            exception.Message);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(4)]
    [InlineData(999)]
    public void Create_WithUndefinedRole_ThrowsDomainException(
        int roleValue)
    {
        // Act
        var action = () => User.Create(
            Guid.CreateVersion7(),
            "Paul",
            "Chew",
            "paul@example.com",
            (UserRole)roleValue);

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Invalid role.",
            exception.Message);
    }

    [Fact]
    public void Approve_WhenPending_ChangesStatusToActive()
    {
        // Arrange
        var user = CreateValidUser();

        // Act
        user.Approve();

        // Assert
        Assert.Equal(UserStatus.Active, user.Status);
    }

    [Fact]
    public void Approve_WhenAlreadyActive_ThrowsAndPreservesStatus()
    {
        // Arrange
        var user = CreateValidUser();
        user.Approve();

        // Act
        var action = user.Approve;

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Only pending users can be approved.",
            exception.Message);
        Assert.Equal(UserStatus.Active, user.Status);
    }

    [Fact]
    public void Approve_WhenSuspended_ThrowsAndPreservesStatus()
    {
        // Arrange
        var user = CreateActiveUser();
        user.Suspend();

        // Act
        var action = user.Approve;

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(UserStatus.Suspended, user.Status);
    }

    [Fact]
    public void Suspend_WhenActive_ChangesStatusToSuspended()
    {
        // Arrange
        var user = CreateActiveUser();

        // Act
        user.Suspend();

        // Assert
        Assert.Equal(UserStatus.Suspended, user.Status);
    }

    [Fact]
    public void Suspend_WhenPending_ThrowsAndPreservesStatus()
    {
        // Arrange
        var user = CreateValidUser();

        // Act
        var action = user.Suspend;

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Only active users can be suspended.",
            exception.Message);
        Assert.Equal(UserStatus.Pending, user.Status);
    }

    [Fact]
    public void Suspend_WhenAlreadySuspended_ThrowsAndPreservesStatus()
    {
        // Arrange
        var user = CreateActiveUser();
        user.Suspend();

        // Act
        var action = user.Suspend;

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(UserStatus.Suspended, user.Status);
    }

    [Fact]
    public void Reactivate_WhenSuspended_ChangesStatusToActive()
    {
        // Arrange
        var user = CreateActiveUser();
        user.Suspend();

        // Act
        user.Reactivate();

        // Assert
        Assert.Equal(UserStatus.Active, user.Status);
    }

    [Fact]
    public void Reactivate_WhenPending_ThrowsAndPreservesStatus()
    {
        // Arrange
        var user = CreateValidUser();

        // Act
        var action = user.Reactivate;

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Only suspended users can be reactivated.",
            exception.Message);
        Assert.Equal(UserStatus.Pending, user.Status);
    }

    [Fact]
    public void Reactivate_WhenActive_ThrowsAndPreservesStatus()
    {
        // Arrange
        var user = CreateActiveUser();

        // Act
        var action = user.Reactivate;

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(UserStatus.Active, user.Status);
    }

    [Fact]
    public void Deactivate_SetsIsActiveToFalseWithoutChangingStatus()
    {
        // Arrange
        var user = CreateValidUser();
        var originalStatus = user.Status;

        // Act
        user.Deactivate();

        // Assert
        Assert.False(user.IsActive);
        Assert.Equal(originalStatus, user.Status);
    }

    [Fact]
    public void Deactivate_WhenAlreadyInactive_RemainsInactive()
    {
        // Arrange
        var user = CreateValidUser();
        user.Deactivate();

        // Act
        user.Deactivate();

        // Assert
        Assert.False(user.IsActive);
    }

    [Fact]
    public void Activate_SetsIsActiveToTrueWithoutChangingStatus()
    {
        // Arrange
        var user = CreateValidUser();
        user.Deactivate();
        var originalStatus = user.Status;

        // Act
        user.Activate();

        // Assert
        Assert.True(user.IsActive);
        Assert.Equal(originalStatus, user.Status);
    }

    [Fact]
    public void Activate_WhenAlreadyActive_RemainsActive()
    {
        // Arrange
        var user = CreateValidUser();

        // Act
        user.Activate();

        // Assert
        Assert.True(user.IsActive);
    }

    [Fact]
    public void ChangeName_WithValidNames_UpdatesBothNames()
    {
        // Arrange
        var user = CreateValidUser();

        // Act
        user.ChangeName("Lisa", "Smith");

        // Assert
        Assert.Equal("Lisa", user.FirstName);
        Assert.Equal("Smith", user.LastName);
    }

    [Fact]
    public void ChangeName_TrimsBothNames()
    {
        // Arrange
        var user = CreateValidUser();

        // Act
        user.ChangeName("  Lisa  ", "  Smith  ");

        // Assert
        Assert.Equal("Lisa", user.FirstName);
        Assert.Equal("Smith", user.LastName);
    }

    [Fact]
    public void ChangeName_WithInvalidFirstName_ThrowsAndPreservesNames()
    {
        // Arrange
        var user = CreateValidUser();
        var originalFirstName = user.FirstName;
        var originalLastName = user.LastName;

        // Act
        var action = () => user.ChangeName("", "Smith");

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalFirstName, user.FirstName);
        Assert.Equal(originalLastName, user.LastName);
    }

    [Fact]
    public void ChangeName_WithInvalidLastName_ThrowsAndPreservesNames()
    {
        // Arrange
        var user = CreateValidUser();
        var originalFirstName = user.FirstName;
        var originalLastName = user.LastName;

        // Act
        var action = () => user.ChangeName("Lisa", "");

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalFirstName, user.FirstName);
        Assert.Equal(originalLastName, user.LastName);
    }

    [Fact]
    public void ChangeEmail_WithValidEmail_UpdatesAndNormalisesEmail()
    {
        // Arrange
        var user = CreateValidUser();

        // Act
        user.ChangeEmail("  NEW@EXAMPLE.COM  ");

        // Assert
        Assert.Equal("new@example.com", user.Email);
    }

    [Fact]
    public void ChangeEmail_WithInvalidEmail_ThrowsAndPreservesEmail()
    {
        // Arrange
        var user = CreateValidUser();
        var originalEmail = user.Email;

        // Act
        var action = () => user.ChangeEmail("invalid");

        // Assert
        Assert.Throws<DomainException>(action);
        Assert.Equal(originalEmail, user.Email);
    }

    [Fact]
    public void PromoteToTeacher_WhenStudent_ChangesRoleToTeacher()
    {
        // Arrange
        var user = CreateValidUser(UserRole.Student);

        // Act
        user.PromoteToTeacher();

        // Assert
        Assert.Equal(UserRole.Teacher, user.Role);
    }

    [Theory]
    [InlineData(UserRole.Teacher)]
    [InlineData(UserRole.Administrator)]
    public void PromoteToTeacher_WhenNotStudent_ThrowsAndPreservesRole(
        UserRole initialRole)
    {
        // Arrange
        var user = CreateValidUser(initialRole);

        // Act
        var action = user.PromoteToTeacher;

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Only students can be promoted to teachers.",
            exception.Message);
        Assert.Equal(initialRole, user.Role);
    }

    [Theory]
    [InlineData(UserRole.Student)]
    [InlineData(UserRole.Teacher)]
    [InlineData(UserRole.Administrator)]
    public void PromoteToAdministrator_FromAnyRole_SetsAdministratorRole(
        UserRole initialRole)
    {
        // Arrange
        var user = CreateValidUser(initialRole);

        // Act
        user.PromoteToAdministrator();

        // Assert
        Assert.Equal(UserRole.Administrator, user.Role);
    }

    [Fact]
    public void DemoteToTeacher_WhenAdministrator_ChangesRoleToTeacher()
    {
        // Arrange
        var user = CreateValidUser(UserRole.Administrator);

        // Act
        user.DemoteToTeacher();

        // Assert
        Assert.Equal(UserRole.Teacher, user.Role);
    }

    [Theory]
    [InlineData(UserRole.Student)]
    [InlineData(UserRole.Teacher)]
    public void DemoteToTeacher_WhenNotAdministrator_ThrowsAndPreservesRole(
        UserRole initialRole)
    {
        // Arrange
        var user = CreateValidUser(initialRole);

        // Act
        var action = user.DemoteToTeacher;

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Only administrators can be demoted to teachers.",
            exception.Message);
        Assert.Equal(initialRole, user.Role);
    }

    [Fact]
    public void DemoteToStudent_WhenTeacher_ChangesRoleToStudent()
    {
        // Arrange
        var user = CreateValidUser(UserRole.Teacher);

        // Act
        user.DemoteToStudent();

        // Assert
        Assert.Equal(UserRole.Student, user.Role);
    }

    [Theory]
    [InlineData(UserRole.Student)]
    [InlineData(UserRole.Administrator)]
    public void DemoteToStudent_WhenNotTeacher_ThrowsAndPreservesRole(
        UserRole initialRole)
    {
        // Arrange
        var user = CreateValidUser(initialRole);

        // Act
        var action = user.DemoteToStudent;

        // Assert
        var exception = Assert.Throws<DomainException>(action);

        Assert.Equal(
            "Only teachers can be demoted to students.",
            exception.Message);
        Assert.Equal(initialRole, user.Role);
    }

    private static User CreateValidUser(
        UserRole role = UserRole.Teacher)
    {
        return User.Create(
            Guid.CreateVersion7(),
            "Paul",
            "Chew",
            "paul@example.com",
            role);
    }

    private static User CreateActiveUser()
    {
        var user = CreateValidUser();
        user.Approve();

        return user;
    }
}