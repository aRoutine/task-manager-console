using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Configuration;
using TaskManager.Api.ExceptionHandling;
using TaskManager.Api.Services;
using TaskManager.Data;
using TaskManager.Interfaces;
using TaskManager.Models;
using TaskManager.Services;
using TaskManager.Storage;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TaskManagerDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

builder.Services.AddScoped<ITaskRepository, EfTaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.Configure<JwtOptions>(
    builder.Configuration.GetSection("Jwt")
);

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    JwtOptions jwtOptions = 
    builder.Configuration.GetSection("Jwt")
    .Get<JwtOptions>() ?? throw new InvalidOperationException("JWT configuration is missing");

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.Key)
        ),

        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseAuthentication();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program
{
}