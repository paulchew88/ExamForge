using ExamForge.Domain.Common;
using ExamForge.Domain.Enums;
using ExamForge.Domain.Exceptions;
using System.Net.Mail;

namespace ExamForge.Domain.Entities;

public class User : Entity
{
    public const int MaxFirstNameLength = 100;
    public const int MaxLastNameLength = 100;
    public const int MaxEmailLength = 320;


    public Guid ExternalIdentityId { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public UserRole Role { get; private set; }
    public UserStatus Status { get; private set; }
    public bool IsActive { get; private set; }

    private User()
    {
        FirstName = string.Empty;
        LastName = string.Empty;
        Email = string.Empty;
    }

    private User(
        Guid id,
        Guid externalIdentityId,
        string firstName,
        string lastName,
        string email,
        UserRole role,
        UserStatus status,
        bool isActive,
        DateTimeOffset createdAt)
        : base(id, createdAt)
    {
        ExternalIdentityId = externalIdentityId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Role = role;
        Status = status;
        IsActive = isActive;
    }

    public static User Create(
        Guid externalIdentityId,
        string firstName,
        string lastName,
        string email,
        UserRole role)
    {
        return new User(
            Guid.CreateVersion7(),
            ValidateExternalIdentityId(externalIdentityId),
            ValidateFirstName(firstName),
            ValidateLastName(lastName),
            ValidateEmail(email),
            ValidateRole(role),
            UserStatus.Pending, // Default to pending
            true, // Default to active
            DateTimeOffset.UtcNow);
    }

    private static Guid ValidateExternalIdentityId(Guid externalIdentityId)
    {
        if (externalIdentityId == Guid.Empty)
        {
            throw new DomainException("Identity provider ID cannot be empty.");
        }
        return externalIdentityId;

    }
    private static string ValidateFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new DomainException("First name cannot be empty.");
        }
        var trimmedFirstName = firstName.Trim();
        if (trimmedFirstName.Length > MaxFirstNameLength)
        {
            throw new DomainException($"First name cannot exceed {MaxFirstNameLength} characters.");
        }
        return trimmedFirstName;
    }
    private static string ValidateLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new DomainException("Last name cannot be empty.");
        }
        var trimmedLastName = lastName.Trim();
        if (trimmedLastName.Length > MaxLastNameLength)
        {
            throw new DomainException($"Last name cannot exceed {MaxLastNameLength} characters.");
        }
        return trimmedLastName;
    }


    private static string ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new DomainException("Email cannot be empty.");
        }

        var trimmedEmail = email.Trim().ToLowerInvariant();

        if (trimmedEmail.Length > MaxEmailLength)
        {
            throw new DomainException(
                $"Email cannot exceed {MaxEmailLength} characters.");
        }

        try
        {
            _ = new MailAddress(trimmedEmail);
        }
        catch
        {
            throw new DomainException("Email is not valid.");
        }

        return trimmedEmail;
    }
    private static UserRole ValidateRole(UserRole role)
    {
        if (!Enum.IsDefined(typeof(UserRole), role))
        {
            throw new DomainException("Invalid role.");
        }
        return role;
    }

    public void Approve()
    {
        if (Status != UserStatus.Pending)
        {
            throw new DomainException(
                "Only pending users can be approved.");
        }

        Status = UserStatus.Active;
    }

    public void Suspend()
    {
        if (Status != UserStatus.Active)
        {
            throw new DomainException(
                "Only active users can be suspended.");
        }

        Status = UserStatus.Suspended;
    }

    public void Reactivate()
    {
        if (Status != UserStatus.Suspended)
        {
            throw new DomainException(
                "Only suspended users can be reactivated.");
        }

        Status = UserStatus.Active;
    }

    public void Deactivate()
    {
        if (!IsActive)
        {
            return;
        }

        IsActive = false;
    }

    public void Activate()
    {
        if (IsActive)
        {
            return;
        }

        IsActive = true;
    }

    public void ChangeName(
        string newFirstName,
        string newLastName)
    {
        var validatedFirstName =
            ValidateFirstName(newFirstName);

        var validatedLastName =
            ValidateLastName(newLastName);

        FirstName = validatedFirstName;
        LastName = validatedLastName;
    }
    public void PromoteToTeacher()
    {
        if (Role == UserRole.Student)
        {
            Role = UserRole.Teacher;
        }
        else { throw new DomainException("Only students can be promoted to teachers."); }
    }
    public void PromoteToAdministrator()
    {
        Role = UserRole.Administrator;
    }
    public void DemoteToTeacher()
    {
        if (Role == UserRole.Administrator)
        {
            Role = UserRole.Teacher;
        }
        else { throw new DomainException("Only administrators can be demoted to teachers."); }
    }
    public void DemoteToStudent()
    {
        if (Role == UserRole.Teacher)
        {
            Role = UserRole.Student;
        }
        else { throw new DomainException("Only teachers can be demoted to students."); }
    }
    public void ChangeEmail(string newEmail)
    {
        Email = ValidateEmail(newEmail);
    }


}
