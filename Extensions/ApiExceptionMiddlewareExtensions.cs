namespace CatalogoApi.Extensions;

using System.Net;
using CatalogoApi.Models;
using Microsoft.AspNetCore.Diagnostics;

public static class ApiExceptionMiddlewareExtensions
{
    // Criar um método de extensão
    public static void ConfigureExceptionHandle(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError => 
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                        Trace = contextFeature.Error.StackTrace
                    }.ToString());
                }
            });
        });
    }
}