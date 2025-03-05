using Microsoft.AspNetCore.Authentication.JwtBearer;
using _NET_MinimalAPI.Domain.Interfaces;
using _NET_MinimalAPI.Domain.Services;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text;

namespace _NET_MinimalAPI
{
    public class Setup
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            var key = builder.Configuration["Jwt:Key"];

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

            // --- Injeção de dependências
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

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            builder.Services.AddDbContext<DBContext>(options =>
            {
                var connectionString = builder.Configuration.GetConnectionString("MySql");
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });

            builder.Services.AddControllers();
        }

        public static void ConfigureApp(WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseRouting();           // 1 - configure Routing
            app.UseAuthentication();    // 2 - Add Authentication
            app.UseAuthorization();     // 3 - So, add authorization

            app.MapControllers();       // 4 - Mapping Controllers
            app.Run();
        }
    }
}
