using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Extensions
{
    // Tüm hatalar yaklanır ve bu response döner
    public static class CustomValidationResponse
    {
        public static void UseCustomValidationResponse(this IServiceCollection services)
        {
            // fluentten gelen çıktıyı override edicem (api nin davranışı değiştiricem)
            services.Configure<ApiBehaviorOptions>(options =>
            {
                // Model state invalid olduğunda bir response oluşturulur
                options.InvalidModelStateResponseFactory = (context) =>
                {
                    // hata mesjları yakalanır
                    var errors = context.ModelState.Values.Where(e => e.Errors.Count > 0).SelectMany(e => e.Errors).Select(m => m.ErrorMessage).ToList();

                    ErrorDto error = new ErrorDto(errors, true);

                    // response oluşturulur
                    var response = Response<NoContentResult>.Fail(error, 400);

                    return new BadRequestObjectResult(response);
                };
            });
        }
    }
}
