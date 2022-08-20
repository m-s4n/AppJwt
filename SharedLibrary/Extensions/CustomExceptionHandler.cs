using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SharedLibrary.Dtos;
using SharedLibrary.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SharedLibrary.Extensions
{
    public static class CustomExceptionHandler
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            // .net core bu middlerware ile tüm hatalar yakalanır
            app.UseExceptionHandler(config =>
            {
                // Run middleware akışına devam ettirir. (sonlandırıcı) istek bir sonraki middleware e geçmez
                // Use middleware akışına devam ettirir. (normal middleware) istek bir sonraki middleware e geçer
                config.Run(async context =>
                {
                    // sistemsel hata kodu
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    // Bu interface üzerinden hatalar yakalanır
                    var errors = context.Features.Get<IExceptionHandlerFeature>();
                    if (errors != null)
                    {
                        var exceptions = errors.Error;

                        ErrorDto errorDto = null;
                        if (exceptions is CustomException)
                        {
                            errorDto = new ErrorDto(exceptions.Message, true);
                        }
                        else
                        {
                            errorDto = new ErrorDto(exceptions.Message, false);
                        }

                        // response return edilir
                        var response = Response<NoDataDto>.Fail(errorDto, 500);

                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                    }
                });
            });
        }
    }
}
