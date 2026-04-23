using XboxLiveLite.Api.Data;

namespace XboxLiveLite.Api.Services;

public class MatchmakingService
{
    public void EnqueuePlayer(string playerId)
    {
        MatchmakingQueue.Queue.Enqueue(playerId);
    }

    public int GetQueueCount()
    {
        return MatchmakingQueue.Queue.Count;
    }

    public List<string> TryMatchPlayers(int playersNeeded = 2)
    {
        var players = new List<string>();

        while (players.Count < playersNeeded && MatchmakingQueue.Queue.TryDequeue(out var player))
        {
            players.Add(player);
        }

        if (players.Count == playersNeeded)
            return players;

        // si no alcanzan, devolverlos a la cola
        foreach (var p in players)
            MatchmakingQueue.Queue.Enqueue(p);

        return new List<string>();
    }
}