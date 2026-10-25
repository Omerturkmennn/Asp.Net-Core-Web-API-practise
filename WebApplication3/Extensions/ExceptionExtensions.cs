using Entities1.ErrorModel;
using Entities1.Exceptions1;
using Microsoft.AspNetCore.Diagnostics;
using Services.Contracts;
using System.Net;

namespace WebApplication3.Extensions
{
    public static class ExceptionExtensions
    {
        public static void ConfigureExceptionHandler(this WebApplication app,ILoggerService logger)
        {
            //Uygulamanın en dışına Middleware koyuldu
            //Uygulamada nerede hata olursa olsun, o hatayı buraya yönlendir(app.UseExceptionHandler)
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    
                    context.Response.ContentType = "application/json";

                    //Hata yakalama
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        context.Response.StatusCode = contextFeature.Error switch
                        {
                            NotFoundException => StatusCodes.Status404NotFound,
                           _  => StatusCodes.Status500InternalServerError
                        };

                        logger.LogError($"Something went wrong {contextFeature.Error}");
                        await context.Response.WriteAsync(new ErrorDetails()
                        {
                            StatusCode = context.Response.StatusCode,
                            Message=contextFeature.Error.Message
                        }.ToString());
                    }
                });
            });
        }
    }
}
