using ExamForge.Domain.Entities;
using ExamForge.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ExamForge.Infrastructure.Persistence;

public sealed class ExamForgeDbContext
    : DbContext, IUnitOfWork
{
    public ExamForgeDbContext(
        DbContextOptions<ExamForgeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Topic> Topics => Set<Topic>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<QuestionOption> QuestionOptions => Set<QuestionOption>();

    public DbSet<User> Users => Set<User>();

    public DbSet<Classroom> Classrooms => Set<Classroom>();
    public DbSet<ClassroomStudent> ClassroomStudents => Set<ClassroomStudent>();

    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<AssignmentQuestion> AssignmentQuestions => Set<AssignmentQuestion>();
    public DbSet<AssignmentClassroom> AssignmentClassrooms => Set<AssignmentClassroom>();

    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<SubmissionAnswer> SubmissionAnswers => Set<SubmissionAnswer>();


    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ExamForgeDbContext).Assembly);
    }
}