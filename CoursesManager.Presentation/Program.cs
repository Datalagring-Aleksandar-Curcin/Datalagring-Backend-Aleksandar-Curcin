using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Dtos.CourseRegistrations;
using CoursesManager.Application.Dtos.Courses;
using CoursesManager.Application.Dtos.CourseSessions;
using CoursesManager.Application.Dtos.CourseStatus;
using CoursesManager.Application.Dtos.InstanceTeachers;
using CoursesManager.Application.Dtos.Locations;
using CoursesManager.Application.Dtos.Participants;
using CoursesManager.Application.Dtos.Teachers;
using CoursesManager.Application.Services;
using CoursesManager.Infrastructure.Data;
using CoursesManager.Infrastructure.Persistence.Repositories;
using CoursesManager.Presentation.Extensions;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("LocalDbFile")).UseExceptionProcessor());

//Services
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<CourseSessionService>();
builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<TeacherService>();
builder.Services.AddScoped<ParticipantService>();
builder.Services.AddScoped<CourseStatusService>();
builder.Services.AddScoped<CourseRegistrationService>();
builder.Services.AddScoped<InstanceTeacherService>();

//Repos
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseSessionRepository, CourseSessionRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IParticipantRepository, ParticipantRepository>();
builder.Services.AddScoped<ICourseStatusRepository, CourseStatusRepository>();
builder.Services.AddScoped<ICourseRegistrationRepository, CourseRegistrationRepository>();
builder.Services.AddScoped<IInstanceTeacherRepository, InstanceTeacherRepository>();


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
courses.MapGet("/{courseCode}", async (string courseCode, CourseService courseService, CancellationToken ct) =>
{
    var result = await courseService.GetOneCourseAsync(courseCode, ct);
    return result.Match(
        course => Results.Ok(course),
        errors => errors.ToProblemDetails()
    );
});

courses.MapPost("/", async (CreateCourseDto dto, CourseService courseService, CancellationToken ct) =>
{
    var result = await courseService.CreateCourseAsync(dto, ct);
    return result.Match(
        course => Results.Created($"/api/courses/{course.CourseCode}", course),
        errors => errors.ToProblemDetails()
    );
});

courses.MapPut("/{courseCode}", async (string courseCode, UpdateCourseDto dto, CourseService courseService, CancellationToken ct) =>
{
    var result = await courseService.UpdateCourseAsync(courseCode, dto, ct);
    return result.Match(
        course => Results.Ok(course),
        errors => errors.ToProblemDetails()
    );
});

courses.MapDelete("/{courseCode}", async (string courseCode, CourseService courseService, CancellationToken ct) =>
{
    var result = await courseService.DeleteCourseAsync(courseCode, ct);
    return result.Match(
        _ => Results.NoContent(),
        errors => errors.ToProblemDetails()
    );
});

#endregion


#region CourseSessions

var courseSessions = app.MapGroup("/api/course-sessions").WithTags("Course Sessions");


courseSessions.MapGet("/", async (CourseSessionService service, CancellationToken ct) =>
{
    var result = await service.GetAllCourseSessionsAsync(ct);
    return Results.Ok(result);
});

courseSessions.MapGet("/{id:int}", async (int id, CourseSessionService service, CancellationToken ct) =>
{
    var result = await service.GetOneCourseSessionAsync(id, ct);
    return result.Match(
        cs => Results.Ok(cs),
        errors => errors.ToProblemDetails()
    );
});

courseSessions.MapPost("/", async (CreateCourseSessionDto dto, CourseSessionService service, CancellationToken ct) =>
{
    var result = await service.CreateCourseSessionAsync(dto, ct);
    return result.Match(
        cs => Results.Created($"/api/course-sessions/{cs.Id}", cs),
        errors => errors.ToProblemDetails()
    );
});

courseSessions.MapPut("/{id:int}", async (int id, UpdateCourseSessionDto dto, CourseSessionService service, CancellationToken ct) =>
{
    var result = await service.UpdateCourseSessionAsync(id, dto, ct);
    return result.Match(
        cs => Results.Ok(cs),
        errors => errors.ToProblemDetails()
    );
});

courseSessions.MapDelete("/{id:int}", async (int id, CourseSessionService service, CancellationToken ct) =>
{
    var result = await service.DeleteCourseSessionAsync(id, ct);
    return result.Match(
        _ => Results.NoContent(),
        errors => errors.ToProblemDetails()
    );
});



#endregion


#region Locations

var locations = app.MapGroup("/api/locations").WithTags("Locations");

locations.MapGet("/", async (LocationService service, CancellationToken ct) =>
{
    var result = await service.GetAllLocationsAsync(ct);
    return Results.Ok(result);
});

locations.MapGet("/{id:int}", async (int id, LocationService service, CancellationToken ct) =>
{
    var result = await service.GetOneLocationAsync(id, ct);
    return result.Match(
        location => Results.Ok(location),
        errors => errors.ToProblemDetails()
    );
});

locations.MapPost("/", async (CreateLocationDto dto, LocationService service, CancellationToken ct) =>
{
    var result = await service.CreateLocationAsync(dto, ct);
    return result.Match(
        location => Results.Created($"/api/locations/{location.Id}", location),
        errors => errors.ToProblemDetails()
    );
});

locations.MapPut("/{id:int}", async (int id, UpdateLocationDto dto, LocationService service, CancellationToken ct) =>
{
    var result = await service.UpdateLocationAsync(id, dto, ct);
    return result.Match(
        location => Results.Ok(location),
        errors => errors.ToProblemDetails()
    );
});

locations.MapDelete("/{id:int}", async (int id, LocationService service, CancellationToken ct) =>
{
    var result = await service.DeleteLocationAsync(id, ct);
    return result.Match(
        _ => Results.NoContent(),
        errors => errors.ToProblemDetails()
    );
});



#endregion

#region Teachers

var teachers = app.MapGroup("/api/teachers").WithTags("Teachers");

teachers.MapGet("/", async (TeacherService service, CancellationToken ct) =>
{
    var result = await service.GetAllTeachersAsync(ct);
    return Results.Ok(result);
});

teachers.MapGet("/{id:int}", async (int id, TeacherService service, CancellationToken ct) =>
{
    var result = await service.GetOneTeacherAsync(id, ct);
    return result.Match(
        teacher => Results.Ok(teacher),
        errors => errors.ToProblemDetails()
    );
});

teachers.MapPost("/", async (CreateTeacherDto dto, TeacherService service, CancellationToken ct) =>
{
    var result = await service.CreateTeacherAsync(dto, ct);
    return result.Match(
        teacher => Results.Created($"/api/teachers/{teacher.Id}", teacher),
        errors => errors.ToProblemDetails()
    );
});

teachers.MapPut("/{id:int}", async (int id, UpdateTeacherDto dto, TeacherService service, CancellationToken ct) =>
{
    var result = await service.UpdateTeacherAsync(id, dto, ct);
    return result.Match(
        teacher => Results.Ok(teacher),
        errors => errors.ToProblemDetails()
    );
});

teachers.MapDelete("/{id:int}", async (int id, TeacherService service, CancellationToken ct) =>
{
    var result = await service.DeleteTeacherAsync(id, ct);
    return result.Match(
        _ => Results.NoContent(),
        errors => errors.ToProblemDetails()
    );
});


#endregion

#region Participants

var participants = app.MapGroup("/api/participants").WithTags("Participants");

participants.MapGet("/", async (ParticipantService service, CancellationToken ct) =>
{
    var result = await service.GetAllParticipantsAsync(ct);
    return Results.Ok(result);
});

participants.MapGet("/{id:int}", async (int id, ParticipantService service, CancellationToken ct) =>
{
    var result = await service.GetOneParticipantAsync(id, ct);
    return result.Match(
        p => Results.Ok(p),
        errors => errors.ToProblemDetails()
    );
});

participants.MapPost("/", async (CreateParticipantDto dto, ParticipantService service, CancellationToken ct) =>
{
    var result = await service.CreateParticipantAsync(dto, ct);
    return result.Match(
        p => Results.Created($"/api/participants/{p.Id}", p),
        errors => errors.ToProblemDetails()
    );
});

participants.MapPut("/{id:int}", async (int id, UpdateParticipantDto dto, ParticipantService service, CancellationToken ct) =>
{
    var result = await service.UpdateParticipantAsync(id, dto, ct);
    return result.Match(
        p => Results.Ok(p),
        errors => errors.ToProblemDetails()
    );
});

participants.MapDelete("/{id:int}", async (int id, ParticipantService service, CancellationToken ct) =>
{
    var result = await service.DeleteParticipantAsync(id, ct);
    return result.Match(
        _ => Results.NoContent(),
        errors => errors.ToProblemDetails()
    );
});

#endregion

#region CourseStatus

var courseStatuses = app.MapGroup("/api/course-statuses").WithTags("Course Statuses");

courseStatuses.MapGet("/", async (CourseStatusService service, CancellationToken ct) =>
{
    var result = await service.GetAllCourseStatusesAsync(ct);
    return Results.Ok(result);
});

courseStatuses.MapGet("/{id:int}", async (int id, CourseStatusService service, CancellationToken ct) =>
{
    var result = await service.GetOneCourseStatusAsync(id, ct);
    return result.Match(
        s => Results.Ok(s),
        errors => errors.ToProblemDetails()
    );
});

courseStatuses.MapPost("/", async (CreateCourseStatusDto dto, CourseStatusService service, CancellationToken ct) =>
{
    var result = await service.CreateCourseStatusAsync(dto, ct);
    return result.Match(
        s => Results.Created($"/api/course-statuses/{s.Id}", s),
        errors => errors.ToProblemDetails()
    );
});

courseStatuses.MapPut("/{id:int}", async (int id, UpdateCourseStatusDto dto, CourseStatusService service, CancellationToken ct) =>
{
    var result = await service.UpdateCourseStatusAsync(id, dto, ct);
    return result.Match(
        s => Results.Ok(s),
        errors => errors.ToProblemDetails()
    );
});

courseStatuses.MapDelete("/{id:int}", async (int id, CourseStatusService service, CancellationToken ct) =>
{
    var result = await service.DeleteCourseStatusAsync(id, ct);
    return result.Match(
        _ => Results.NoContent(),
        errors => errors.ToProblemDetails()
    );
});

#endregion

#region CourseRegistrations

var registrations = app.MapGroup("/api/course-registrations").WithTags("Course Registrations");

registrations.MapGet("/", async (CourseRegistrationService service, CancellationToken ct) =>
{
    var result = await service.GetAllCourseRegistrationsAsync(ct);
    return Results.Ok(result);
});

registrations.MapGet("/{id:int}", async (int id, CourseRegistrationService service, CancellationToken ct) =>
{
    var result = await service.GetOneCourseRegistrationAsync(id, ct);
    return result.Match(
        r => Results.Ok(r),
        errors => errors.ToProblemDetails()
    );
});

registrations.MapPost("/", async (CreateCourseRegistrationDto dto, CourseRegistrationService service, CancellationToken ct) =>
{
    var result = await service.CreateCourseRegistrationAsync(dto, ct);
    return result.Match(
        r => Results.Created($"/api/course-registrations/{r.Id}", r),
        errors => errors.ToProblemDetails()
    );
});

registrations.MapPut("/{id:int}", async (int id, UpdateCourseRegistrationDto dto, CourseRegistrationService service, CancellationToken ct) =>
{
    var result = await service.UpdateCourseRegistrationAsync(id, dto, ct);
    return result.Match(
        r => Results.Ok(r),
        errors => errors.ToProblemDetails()
    );
});

registrations.MapDelete("/{id:int}", async (int id, CourseRegistrationService service, CancellationToken ct) =>
{
    var result = await service.DeleteCourseRegistrationAsync(id, ct);
    return result.Match(
        _ => Results.NoContent(),
        errors => errors.ToProblemDetails()
    );
});

#endregion

#region InstanceTeachers

var instanceTeachers = app.MapGroup("/api/instance-teachers").WithTags("Instance Teachers");

instanceTeachers.MapGet("/", async (InstanceTeacherService service, CancellationToken ct) =>
{
    var rows = await service.GetAllAsync(ct);
    return Results.Ok(rows);
});

instanceTeachers.MapGet("/by-course-session/{courseSessionId:int}", async (int courseSessionId, InstanceTeacherService service, CancellationToken ct) =>
{
    var rows = await service.GetByCourseSessionAsync(courseSessionId, ct);
    return Results.Ok(rows);
});

instanceTeachers.MapPost("/", async (CreateInstanceTeacherDto dto, InstanceTeacherService service, CancellationToken ct) =>
{
    var result = await service.AssignTeacherAsync(dto, ct);
    return result.Match(
        row => Results.Created($"/api/instance-teachers/{row.CourseSessionId}/{row.TeacherId}", row),
        errors => errors.ToProblemDetails()
    );
});

instanceTeachers.MapDelete("/{courseSessionId:int}/{teacherId:int}", async (int courseSessionId, int teacherId, InstanceTeacherService service, CancellationToken ct) =>
{
    var result = await service.UnassignTeacherAsync(courseSessionId, teacherId, ct);
    return result.Match(
        _ => Results.NoContent(),
        errors => errors.ToProblemDetails()
    );
});

#endregion

app.Run();

