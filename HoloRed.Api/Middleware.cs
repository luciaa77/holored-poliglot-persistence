using HoloRed.Domain.Exceptions;

namespace HoloRed.Api.Middleware
{
    public class ExternalServiceExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExternalServiceExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ExternalServiceUnavailableException ex)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    error = ex.ErrorCode,
                    service = ex.ServiceName,
                    message = ex.Message
                });
            }
        }
    }
}