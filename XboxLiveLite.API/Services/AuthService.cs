using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using XboxLiveLite.Api.Data;
using XboxLiveLite.Api.Models;

namespace XboxLiveLite.Api.Services;

public class AuthService
{
    private readonly string _secret = "THIS_IS_SUPER_SECRET_KEY_12345";

    public Player Login(string gamertag)
    {
        var player = InMemoryDb.Players
            .FirstOrDefault(p => p.Gamertag == gamertag);

        if (player == null)
        {
            player = new Player { Gamertag = gamertag };
            InMemoryDb.Players.Add(player);
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