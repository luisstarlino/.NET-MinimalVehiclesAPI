using _NET_MinimalAPI.Domain.Interfaces;
using _NET_MinimalAPI.Domain.ModelViews;
using _NET_MinimalAPI.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministratorService, AdministratorService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBContext>(options => {

    var connectionString = builder.Configuration.GetConnectionString("mysql");
    options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString));

});

var app = builder.Build();

#region Home
app.MapGet("/", () => Results.Json(new Home()));
#endregion

#region Admin
app.MapPost("admin/login", ([FromBody] LoginDTO loginDTO, IAdministratorService administratorService) =>
{

    if (administratorService.Login(loginDTO) != null)
    {
        return Results.Ok("Login Successufully! Welcome back!");
    }
    else
    {
        return Results.Unauthorized();
    }

});
#endregion

app.UseSwagger();
app.UseSwaggerUI();

app.Run();


