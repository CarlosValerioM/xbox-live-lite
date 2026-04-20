using XboxLiveLite.Api.Models;
using XboxLiveLite.Api.Services;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using XboxLiveLite.Api.Data;

var builder = WebApplication.CreateBuilder(args);
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new Exception("JWT Key is missing in configuration");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PlayerRepository>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<SessionRepository>();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer {your token}",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
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
            Array.Empty<string>()
        }
    });
});
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "XboxLiveLite",
            ValidAudience = "XboxLiveLiteUsers",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();

app.MapPost("/auth/login", async (LoginRequest request, AuthService authService) =>
{
    if (string.IsNullOrWhiteSpace(request.Gamertag))
        return Results.BadRequest("Gamertag is required");

    var player = await authService.LoginAsync(request.Gamertag);
    var token = authService.GenerateToken(player);

    return Results.Ok(new
    {
        player.Id,
        player.Gamertag,
        Token = token
    });
});

app.MapPost("/sessions", async (string playerId, ISessionService sessionService) =>
{
    if (string.IsNullOrWhiteSpace(playerId))
        return Results.BadRequest("playerId is required");

    var session = await sessionService.CreateSession(playerId);

    return Results.Ok(session);
});

app.MapGet("/sessions/{id}", async (string id, ISessionService sessionService) =>
{
    var session = await sessionService.GetSession(id);

    if (session == null)
        return Results.NotFound("Session not found");

    return Results.Ok(session);
});

app.MapPost("/sessions/{id}/join", async (string id, string playerId, ISessionService sessionService) =>
{
    if (string.IsNullOrWhiteSpace(playerId))
        return Results.BadRequest("playerId is required");

    var session = await sessionService.JoinSession(id, playerId);

    if (session == null)
        return Results.NotFound("Session not found or not joinable");

    return Results.Ok(session);
});

app.MapGet("/secure", () =>
{
    return Results.Ok("You are authenticated!");
})
.RequireAuthorization();

app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    timestamp = DateTime.UtcNow
}));
app.Run();