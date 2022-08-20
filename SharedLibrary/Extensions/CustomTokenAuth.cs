using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.Configurations;
using SharedLibrary.Services;

namespace SharedLibrary.Extensions
{
    public static class CustomTokenAuth
    {
        // IServiceCollection için extension metot
        public static void AddCustomTokenAuth(this IServiceCollection services, CustomTokenOption tokenOptions)
        {

            // Token Doğrulama
            services.AddAuthentication(options =>
            {
                // Bir şemam var
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // iki şema birbirine bağlanıyor

            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {

                // Token geldiğinde ömrü, issue, bu api'ye istek yapabilir kontrol edilecek
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidIssuer = tokenOptions.Issuer, // tokendan gelen issue ile aynı  ise geçerlilik adımlarından bir tanesi
                    ValidAudience = tokenOptions.Audience[0], // Bu apiden istek yapabilir mi
                    IssuerSigningKey = SignService.GetSecurityKey(tokenOptions.SecurityKey), // hangi key ile doğrulama yapacak
                    ValidateIssuerSigningKey = true, // imzasını kontrol et
                    ValidateAudience = true, // Audience doğrula
                    ValidateIssuer = true, // Issue kontrol et,
                    ValidateLifetime = true, // Token ömrü kontrol et doğrula
                    ClockSkew = TimeSpan.Zero
                };

            });
        }
    }
}
