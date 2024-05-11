using Microsoft.AspNetCore.Http;

namespace Utilities.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    // random jwt token
    private const string Token = "123";

    public AuthenticationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.TryGetValue("Authorization", out var header))
        {
            var token = header.ToString();//.Split(" ")[1];
            if (token == Token)
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized");
            }
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
        }
    }
}