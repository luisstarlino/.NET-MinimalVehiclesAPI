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

#region Admin

app.MapPost("admin", ([FromBody] AdministratorDTO admDTO, IAdministratorService administratorService) =>
{

    // --- Handle
    var messagesError = new ErrorHandler();
    if (admDTO.Profile == null) messagesError.Messages.Add("Parameter 'Profile' as required!");
    if (String.IsNullOrEmpty(admDTO.Password)) messagesError.Messages.Add("Parameter 'Password' as required!");
    if (String.IsNullOrEmpty(admDTO.Mail)) messagesError.Messages.Add("Parameter 'Mail' as required!");


    if (messagesError.Messages.Count > 0) return Results.BadRequest(messagesError);

    var adm = new Administrator
    {
        Mail = admDTO.Mail,
        Password = admDTO.Password,
        Profile = admDTO.Profile.ToString() ?? ProfileENUM.editor.ToString(),
        
    };

    administratorService.Add(adm); 

    return Results.Created($"/vehicle/{adm}", adm);

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "adm" })
.WithTags("Admin");

app.MapGet("admin", ([FromQuery] int? page, IAdministratorService administratorService) =>
{

    var admsDB = administratorService.GetAll(page);
    var admsMV = new List<AdministratorMV>();

    if (admsDB.Count == 0) return Results.NoContent();

    foreach(Administrator adm in admsDB)
    {
        admsMV.Add(new AdministratorMV()
        {
            Id = adm.Id,
            Mail = adm.Mail,
            Profile = adm.Profile,
        });
    };

    return Results.Ok(admsMV);

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "adm"} )
.WithTags("Admin");

app.MapGet("admin/{id}", ([FromRoute] int id, IAdministratorService administratorService) =>
{

    var admDB = administratorService.GetUniqueById(id);

    if (admDB == null) return Results.NotFound();


    else return Results.Ok(new AdministratorMV()
    {
        Id=admDB.Id,
        Profile = admDB.Profile,
        Mail = admDB.Mail,
    });

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "adm" })
.WithTags("Admin");


#endregion

#region Vehicles
ErrorHandler validateVehicle(VehicleDTO vehicleDTO)
{
    // --- Handle
    var messagesError = new ErrorHandler();

    if (String.IsNullOrEmpty(vehicleDTO.Name)) messagesError.Messages.Add("Parameter 'Name' as required!");
    if (String.IsNullOrEmpty(vehicleDTO.Branch)) messagesError.Messages.Add("Parameter 'Brand' as required!");
    if (vehicleDTO.Year < 1950) messagesError.Messages.Add("This year is not valid. Only gets vehicles w/year after 1950!");

    return messagesError;
}
app.MapPost("vehicles", ([FromBody] VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
{

    var validate = validateVehicle(vehicleDTO);
    if (validate.Messages.Count > 0) return Results.BadRequest(validate);

    var vehicle = new Vehicle
    {
        Name = vehicleDTO.Name,
        Branch = vehicleDTO.Branch,
        Year = vehicleDTO.Year,
    };

    vehicleService.Add(vehicle);

    return Results.Created($"/vehicle/{vehicle.Id}", vehicle);

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "adm,editor" })
.WithTags("Vehicle");

app.MapGet("vehicles", ([FromQuery] int? page,IVehicleService vehicleService) =>
{

    var vehicles = vehicleService.GetAll(page);

    if (vehicles.Count == 0) return Results.NoContent();
    else return Results.Ok(vehicles);

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "adm,editor" })
.WithTags("Vehicle");

app.MapGet("vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{

    var vehicle = vehicleService.GetUniqueById(id);

    if (vehicle == null) return Results.NotFound();
    else return Results.Ok(vehicle);

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "adm,editor" })
.WithTags("Vehicle");

app.MapPut("vehicles/{id}", ([FromRoute] int id, VehicleDTO vehicleDTO, IVehicleService vehicleService) =>
{
    var vehicle = vehicleService.GetUniqueById(id);
    if (vehicle == null) return Results.NotFound();

    var validate = validateVehicle(vehicleDTO);
    if (validate.Messages.Count > 0) return Results.BadRequest(validate);

    vehicle.Name = vehicleDTO.Name;
    vehicle.Branch = vehicleDTO.Branch;
    vehicle.Year= vehicleDTO.Year;

    vehicleService.Update(vehicle);
    return Results.Ok(vehicle);

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "adm" })
.WithTags("Vehicle");

app.MapDelete("vehicles/{id}", ([FromRoute] int id, IVehicleService vehicleService) =>
{

    var vehicle = vehicleService.GetUniqueById(id);

    if (vehicle == null) return Results.NotFound();

    vehicleService.Delete(vehicle);
    return Results.Ok(vehicle);

})
.RequireAuthorization()
.RequireAuthorization(new AuthorizeAttribute { Roles = "adm" })
.WithTags("Vehicle");
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

