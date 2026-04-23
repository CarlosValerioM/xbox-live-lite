using System.Collections.Concurrent;

namespace XboxLiveLite.Api.Data;

public static class MatchmakingQueue
{
    public static ConcurrentQueue<string> Queue = new();
}