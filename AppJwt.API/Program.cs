using AppJwt.Core.Configurations;
using AppJwt.Core.Entities;
using AppJwt.Core.Repositories;
using AppJwt.Core.Services;
using AppJwt.Core.UnitOfWork;
using AppJwt.Data.Database;
using AppJwt.Data.Repositories;
using AppJwt.Data.UnitOfWork;
using AppJwt.Service.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Configurations;
using SharedLibrary.Services;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssemblyContaining<Program>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOptions"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

// DI Register
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
//Genericler
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IService<,>), typeof(Service<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Db Context
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DB"), sqlOptions =>
    {
        // Migration hangi assemblt'de olacak
        sqlOptions.MigrationsAssembly("AppJwt.Data");
    });
});

// Db Identity
builder.Services.AddIdentity<User, IdentityRole>(opt =>
{
    opt.User.RequireUniqueEmail = true;
    opt.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

// Token Doðrulama
// appsettingden token bilgileri alýnýr token doðrulamada kullanýlacak
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOption>();
builder.Services.AddCustomTokenAuth(tokenOptions);

// Validation ve bizim hatalarýmýzý yakalanýr custom response ile cevap verilir
builder.Services.UseCustomValidationResponse();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCustomExceptionHandler();
app.UseCors();
app.UseHttpsRedirection();

// Sýralama önemli kimlik doðrulama ve yetki
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
