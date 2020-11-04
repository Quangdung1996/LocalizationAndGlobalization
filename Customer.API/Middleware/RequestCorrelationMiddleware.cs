using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Customer.API.Middleware
{
    internal class RequestCorrelationMiddleware
    {
        public const string CorrelationHeaderName = "X-Correlation-ID";
        private readonly RequestDelegate _next;

        public RequestCorrelationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<RequestCorrelationMiddleware> logger)
        {
            if (!context.Request.Headers.Any(h => h.Key.Equals(CorrelationHeaderName, StringComparison.OrdinalIgnoreCase)))
            {
                var correlationId = Guid.NewGuid().ToString();
                logger.LogInformation($"Request has no correlation header. Adding new correlation ID: {correlationId}");
                context.Request.Headers.Add(CorrelationHeaderName, new StringValues(correlationId));
            }

            var correlationHeader = context.Request.Headers.First(h => h.Key.Equals(CorrelationHeaderName, StringComparison.OrdinalIgnoreCase));
            logger.LogInformation($"Request correlation ID: {correlationHeader.Value.First()}");

            context.Response.Headers.Add(correlationHeader);

            using (logger.BeginScope("CorrelationId:{CorrelationId}", correlationHeader.Value.First()))
            {
                await _next.Invoke(context);
            }
        }
    }
}