using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;

namespace Utilities.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var isWebApiRequest = context.GetEndpoint()?.Metadata.GetMetadata<ControllerActionDescriptor>()?.ControllerTypeInfo.Namespace
                ?.StartsWith("WebAPI") ?? false;
            
            var prefix = isWebApiRequest ? "[WebAPI]" : "[MVC]";

            _logger.LogInformation($"{prefix} {context.Request.Method} {context.Request.Path} {context.Request.QueryString}");

            await _next(context);
        }
    }
}