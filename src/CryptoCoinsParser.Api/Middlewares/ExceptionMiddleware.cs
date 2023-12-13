namespace CryptoCoinsParser.Api.Middlewares;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BadHttpRequestException ex) when (ex.Message is "Unexpected end of request content.")
        {
            // Do nothing
        }
        catch (IOException ex) when (ex.Message is "The client reset the request stream."
                                         or "The request stream was aborted.")
        {
            // Do nothing
        }
        catch (ConnectionResetException)
        {
            // Do nothing
        }
        catch (CryptographicException ex)
        {
            await WriteGenericErrorAsync(ex, context,
                HttpStatusCode.BadRequest,
                ApplicationErrorCode.BadRequest,
                ex.Message);
        }
        catch (FormatException ex)
        {
            await WriteGenericErrorAsync(ex, context,
                HttpStatusCode.BadRequest,
                ApplicationErrorCode.BadRequest,
                ex.Message);
        }
        catch (Exception e)
        {
            await WriteGenericErrorAsync(e, context,
                HttpStatusCode.InternalServerError,
                ApplicationErrorCode.InternalServerError,
                ApplicationErrorCode.InternalServerError.DisplayName());
        }
    }

    private Task WriteGenericErrorAsync(Exception e,
        HttpContext context,
        HttpStatusCode statusCode,
        ApplicationErrorCode applicationErrorCode,
        string errorMessage,
        bool shouldLogError = true)
    {
        if (shouldLogError)
        {
            _logger.LogCritical(e, "Controller call failed");
        }

        context.Response.ContentType = MediaTypeNames.Application.Json;

        context.Response.StatusCode = (int)statusCode;

        var error = new ApplicationError(applicationErrorCode, errorMessage).ToString();

        return context.Response.WriteAsync(error);
    }
}
