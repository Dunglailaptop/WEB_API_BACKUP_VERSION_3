using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class CustomHeaderMiddleware
{
    private readonly RequestDelegate _next;

    public CustomHeaderMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Set your custom headers here
        context.Response.Headers.Add("API-Token", "Basic 30ff0d44-741d-434c-ad2c-ac7ab80f51d1");
        context.Response.Headers.Add("Project-ID", "8090");
        context.Response.Headers.Add("Method", context.Request.Method);

        await _next(context);
    }
}
