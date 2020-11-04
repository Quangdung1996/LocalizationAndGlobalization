using Microsoft.AspNetCore.Builder;

namespace Customer.API.Middleware
{
    public static class RequestCorrelationMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestCorrelation(this IApplicationBuilder app) => app.UseMiddleware<RequestCorrelationMiddleware>();
    }
}