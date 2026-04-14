using Newtonsoft.Json;

namespace XboxLiveLite.Api.Models;

public class Session
{
    [JsonProperty("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string HostPlayerId { get; set; } = string.Empty;

    public List<string> PlayerIds { get; set; } = new();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}