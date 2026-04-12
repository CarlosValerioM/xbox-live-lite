using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using XboxLiveLite.Api.Data;
using XboxLiveLite.Api.Models;

namespace XboxLiveLite.Api.Services;

public class AuthService
{
    private readonly string _secret = "THIS_IS_A_VERY_SECURE_SECRET_KEY_XBOXLIVELITE";

    private readonly PlayerRepository _repo;

    public AuthService(PlayerRepository repo)
    {
        _repo = repo;
    }

    public async Task<Player> LoginAsync(string gamertag)
    {
        var player = await _repo.GetByGamertagAsync(gamertag);

        if (player == null)
        {
            player = new Player { Gamertag = gamertag };
            player = await _repo.CreatePlayerAsync(player);
        }

        return player;
    }

    public string GenerateToken(Player player)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, player.Id),
            new Claim(ClaimTypes.Name, player.Gamertag)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: "XboxLiveLite",
            audience: "XboxLiveLiteUsers",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}