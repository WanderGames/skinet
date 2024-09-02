using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.MiddleWare;

public class ExceptionMiddleWare(IHostEnvironment env, RequestDelegate next)
{
    //has to be named this way it will be used as middleware. It expects a method with this name
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            //send logic to next middleware
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex, env);
        }
    }

    private static Task HandleException(HttpContext context, Exception ex, IHostEnvironment env)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        //check if we are in development mode, if we are we want to return a more detailed error
        var response = env.IsDevelopment() 
            ? new APIErrorResponse(context.Response.StatusCode, ex.Message, ex.StackTrace)
            : new APIErrorResponse(context.Response.StatusCode, ex.Message, "Internal Server Error");

        //Json serializer options set it to use camel case
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(response, options);

        return context.Response.WriteAsync(json);
    }
}
