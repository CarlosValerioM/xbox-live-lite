namespace XboxLiveLite.Api.Models;

public class Player
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Gamertag { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}