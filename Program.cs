using _NET_MinimalAPI.Domain.DTOs;
using _NET_MinimalAPI.Domain.Entities;
using _NET_MinimalAPI.Domain.Interfaces;
using _NET_MinimalAPI.Domain.ModelViews;
using _NET_MinimalAPI.Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

#region Buider
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAdministratorService, AdministratorService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DBContext>(options => {

    var connectionString = builder.Configuration.GetConnectionString("mysql");
    options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString));

});

var app = builder.Build();
#endregion

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

#region Vehicles
app.MapPost("vehicles", ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
{

    var vehicle = new Vehicle
    {
        Name = vehicleDTO.Name,
        Branch = vehicleDTO.Branch,
        Year = vehicleDTO.Year,
    };

    vehicleService.Add(vehicle);

    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);

});

app.MapGet("vehicles", ([FromQuery] int? pagina,IVehicleService vehicleService) =>
{

    var vehicles = vehicleService.GetAll(pagina);

    if (vehicles.Count == 0) return Results.NoContent();
    else return Results.Ok(vehicles);

    

});
#endregion

#region app
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
#endregion

