using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Services;
using CoursesManager.Infrastructure.Data;
using CoursesManager.Infrastructure.Persistence.Repositories;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDbFile")).UseExceptionProcessor());

//Services
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<CourseSessionService>();


//Repos
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseSessionRepository, CourseSessionRepository>();


builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddValidation();

builder.Services.AddProblemDetails(options =>
{
    options.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Extensions["requestId"] = context.HttpContext.TraceIdentifier;
        context.ProblemDetails.Extensions["support"] = "support@domain.com";
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//Cors

app.UseExceptionHandler();

#region Courses
var courses = app.MapGroup("/api/courses").WithTags("Courses");

courses.MapGet("/", async (CourseService courseService, CancellationToken ct) =>
{
    var result = await courseService.GetAllCoursesAsync(ct);
    return Results.Ok(result);
});





app.Run();

