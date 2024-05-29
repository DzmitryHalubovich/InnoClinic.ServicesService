using Microsoft.AspNetCore.Diagnostics;
using Services.Presentation.Validators;

namespace Services.API.Extentions;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetailsService;

    public GlobalExceptionHandler(IProblemDetailsService problemDetailsService)
    {
        _problemDetailsService = problemDetailsService;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
        Exception exception, CancellationToken cancellationToken)
    {
        httpContext.Response.ContentType = "application/json";

        var excDetails = exception switch
        {
            ValidationAppException => (Detail: exception.Message, StatusCode: StatusCodes.Status422UnprocessableEntity),
            _ => (Detail: exception.Message, StatusCode: StatusCodes.Status500InternalServerError)
        };

        httpContext.Response.StatusCode = excDetails.StatusCode;

        if (exception is ValidationAppException validationException)
        {
            await httpContext.Response.WriteAsJsonAsync(new { validationException.Errors });

            return true;
        }

        return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails =
            {
                Title = "An error occured",
                Detail = excDetails.Detail,
                Type = exception.GetType().Name,
                Status = excDetails.StatusCode
            },
            Exception = exception
        });
    }
}
