namespace XboxLiveLite.Api.Models;
using Newtonsoft.Json;

public class Player
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Gamertag { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}