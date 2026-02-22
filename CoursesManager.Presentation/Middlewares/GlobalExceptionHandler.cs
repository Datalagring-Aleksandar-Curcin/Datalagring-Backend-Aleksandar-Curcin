using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CoursesManager.Presentation.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var (status, title, detail) = exception switch
        {
            DbUpdateException { InnerException: SqlException sql } when sql.Number == 547
                => (StatusCodes.Status409Conflict, "Conflict", "Cannot delete this resource because it is referenced by other data."),

            _ => (StatusCodes.Status500InternalServerError, "Server Error", "An unexpected error occurred. Please try again.")
        };

        httpContext.Response.StatusCode = status;

        return await httpContext.RequestServices
            .GetRequiredService<IProblemDetailsService>()
            .TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = new ProblemDetails
                {
                    Title = title,
                    Detail = detail,
                    Status = status
                }
            });
    }
}