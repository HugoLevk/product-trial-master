using AltenTest.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System.Net;

namespace AltenTest.API.MiddleWares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (NotFoundException notFound)
        {
            logger.LogError(notFound, "Resource not found");
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            await context.Response.WriteAsync(notFound.Message);
        }
        catch (ForbidException forbid)
        {
            logger.LogError(forbid, "Access forbidden");
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await context.Response.WriteAsync(forbid.Message);
        }
        catch (System.UnauthorizedAccessException unauthorized)
        {
            logger.LogError(unauthorized, "Unauthorized access");
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            await context.Response.WriteAsync(unauthorized.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}
