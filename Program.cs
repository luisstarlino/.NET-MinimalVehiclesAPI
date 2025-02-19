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
app.MapGet("/", () => Results.Json(new Home())).WithTags("Home");
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

}).WithTags("Admin");
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

}).WithTags("Vehicle");

app.MapGet("vehicles", ([FromQuery] int? page,IVehicleService vehicleService) =>
{

    var vehicles = vehicleService.GetAll(page);

    if (vehicles.Count == 0) return Results.NoContent();
    else return Results.Ok(vehicles);

}).WithTags("Vehicle");

app.MapGet("vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{

    var vehicle = vehicleService.GetUniqueById(id);

    if (vehicle == null) return Results.NotFound();
    else return Results.Ok(vehicle);

}).WithTags("Vehicle");

app.MapPut("vehicles/{id}", ([FromRoute] int id, VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.GetUniqueById(id);
    if (vehicle == null) return Results.NotFound();

    vehicle.Name = vehicleDTO.Name;
    vehicle.Branch = vehicleDTO.Branch;
    vehicle.Year= vehicleDTO.Year;

    vehicleService.Update(vehicle);
    return Results.Ok(vehicle);

}).WithTags("Vehicle");

app.MapDelete("vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{

    var vehicle = vehicleService.GetUniqueById(id);

    if (vehicle == null) return Results.NotFound();

    vehicleService.Delete(vehicle);
    return Results.Ok(vehicle);

}).WithTags("Vehicle");
#endregion

#region app
app.UseSwagger();
app.UseSwaggerUI();
app.Run();
#endregion

