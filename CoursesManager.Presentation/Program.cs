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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();










app.Run();

