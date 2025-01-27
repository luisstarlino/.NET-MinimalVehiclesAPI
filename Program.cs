var builder = WebApplication.CreateBuilder(args);
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


public class LoginDTO
{
    public string Mail { get; set; } = default!;
    public string Password { get; set; } = default!;

}