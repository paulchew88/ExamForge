using ExamForge.Application.Common.Abstractions.Persistence;
using ExamForge.Application.Courses;
using ExamForge.Infrastructure.Persistence;
using ExamForge.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUnitOfWork>(sp =>
    sp.GetRequiredService<ExamForgeDbContext>());

builder.Services.AddDbContext<ExamForgeDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ICourseService, CourseService>();
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();