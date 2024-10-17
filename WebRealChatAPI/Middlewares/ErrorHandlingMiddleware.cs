using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace WebRealChatAPI.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        // Constructor to inject the next request delegate
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // Middleware invoke method to handle exceptions
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context); // Pass request to the next middleware
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); // Handle exceptions
            }
        }

        // Method to handle exceptions and set response details
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Write error details to response
            return context.Response.WriteAsync(new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An internal error occurred.",
                Detail = exception.Message // Error details
            }.ToString());
        }
    }
}
