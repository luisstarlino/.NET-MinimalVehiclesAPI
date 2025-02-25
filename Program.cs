using _NET_MinimalAPI.Domain.DTOs;
using _NET_MinimalAPI.Domain.Entities;
using _NET_MinimalAPI.Domain.Enuns;
using _NET_MinimalAPI.Domain.Interfaces;
using _NET_MinimalAPI.Domain.ModelViews;
using _NET_MinimalAPI.Domain.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

#region Buider
var builder = WebApplication.CreateBuilder(args);

var key = builder.Configuration.GetSection("Jwt").ToString();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
        ValidateAudience = false,
        ValidateIssuer = false
    };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Put your token here"
    });

    options.AddSecurityRequirement (new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
         }
    });
});

builder.Services.AddDbContext<DBContext>(options => {

    var connectionString = builder.Configuration.GetConnectionString("MySql");
    options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString));

});

// --- Allow Controllers
builder.Services.AddControllers();

var app = builder.Build();
#endregion

#region app
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

// --- Mapping Controllers
app.MapControllers();
app.UseRouting();
app.Run();
#endregion

