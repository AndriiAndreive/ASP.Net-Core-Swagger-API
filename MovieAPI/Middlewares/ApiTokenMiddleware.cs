using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace MovieAPI.Middlewares
{
    /// <summary>
    /// Represents an ApiTokenMiddleware entity.
    /// </summary>
    public class ApiTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _apiToken;

        public ApiTokenMiddleware(RequestDelegate next, string apiToken)
        {
            _next = next;
            _apiToken = apiToken;
        }

        public async Task Invoke(HttpContext context)
        {
            // Check if the request is a POST request
            bool isGetRequest = context.Request.Method.Equals("GET", StringComparison.OrdinalIgnoreCase);

            // Exclude certain endpoints from token validation
            if (isGetRequest)
            {
                await _next(context);
                return;
            }

            // Validate API token
            string? providedToken = context.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(providedToken) || !providedToken.Equals($"Bearer {_apiToken}"))
            {
                context.Response.StatusCode = 401; // Unauthorized
                await context.Response.WriteAsync("Invalid API token.");
                return;
            }

            await _next(context);
        }
    }

    public static class ApiTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiTokenMiddleware(this IApplicationBuilder builder, string apiToken)
        {
            return builder.UseMiddleware<ApiTokenMiddleware>(apiToken);
        }
    }

}

