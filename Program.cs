using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DBContext>(options => {

    var connectionString = builder.Configuration.GetConnectionString("mysql");
    options.UseMySql(connectionString,ServerVersion.AutoDetect(connectionString));

});

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapPost("/login", (LoginDTO loginDTO) =>
{

    if (loginDTO.Mail.Equals("adm@test") && loginDTO.Password.Equals("123"))
    {
        return Results.Ok("Login Successufully! Welcome back!");
    }
    else
    {
        return Results.Unauthorized();
    }

});

app.Run();


