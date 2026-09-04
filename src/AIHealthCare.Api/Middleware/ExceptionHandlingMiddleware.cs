namespace AIHealthCare.Api.Middleware;
using System.Text.Json;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try { await next(context); }
        catch (KeyNotFoundException ex) { await Write(context, 404, ex.Message); }
        catch (ArgumentException ex) { await Write(context, 400, ex.Message); }
        catch (UnauthorizedAccessException ex) { await Write(context, 401, ex.Message); }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled application error.");
            await Write(context, 500, "An unexpected error occurred.");
        }
    }

    private static async Task Write(HttpContext context, int status, string message)
    {
        context.Response.StatusCode = status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            status,
            error = message,
            traceId = context.TraceIdentifier
        }));
    }
}
