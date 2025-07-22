using ISTA.Portal.Application.Exceptions;
using ISTA.Portal.Application.Logger;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Reflection;

namespace ISTA.Portal.API.Helpers
{
    public static class ExceptionExtension
    {
        public static void AddProductionExceptionHandling(this IApplicationBuilder app, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (errorFeature is null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "text/plain";
                        await context.Response.WriteAsync("There was an error");
                        return;
                    }

                    if (errorFeature.Error is GeneralException)
                    {
                        var exception = (GeneralException)errorFeature.Error;

                        context.Response.StatusCode = (int)exception.HttpStatusCode;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsJsonAsync(
                              new
                              {
                                  HttpStatusCode = exception.HttpStatusCode,
                                  FieldName = new List<string> { exception.FieldName },
                                  Message = exception.Message
                              });
                        return;
                    }

                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "text/plain";
                    await context.Response.WriteAsync(errorFeature.Error.Message);
                    var logger = new Logger();
                    logger.LogError(errorFeature.Error.Message, errorFeature.Error);
                });
            });
        }
    }
}