using XboxLiveLite.Api.Models;
using XboxLiveLite.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<AuthService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapPost("/auth/login", (LoginRequest request, AuthService authService) =>
{
    if (string.IsNullOrWhiteSpace(request.Gamertag))
        return Results.BadRequest("Gamertag is required");

    var player = authService.Login(request.Gamertag);
    var token = authService.GenerateToken(player);

    return Results.Ok(new
    {
        player.Id,
        player.Gamertag,
        Token = token
    });
});

app.Run();