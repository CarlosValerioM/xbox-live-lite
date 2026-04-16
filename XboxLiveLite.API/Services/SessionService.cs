using XboxLiveLite.Api.Data;
using XboxLiveLite.Api.Models;

namespace XboxLiveLite.Api.Services;

public interface ISessionService
{
    Session CreateSession(string playerId);
    Session? GetSession(string id);
    Session? JoinSession(string sessionId, string playerId);
}

public class SessionService : ISessionService
{
    public Session CreateSession(string playerId)
    {
        var session = new Session
        {
            HostPlayerId = playerId,
            PlayerIds = new List<string> { playerId }
        };

        SessionStore.Sessions.Add(session);

        return session;
    }

    public Session? GetSession(string id)
    {
        return SessionStore.Sessions
            .FirstOrDefault(s => s.Id == id);
    }
    public Session? JoinSession(string sessionId, string playerId)
    {
        var session = SessionStore.Sessions
            .FirstOrDefault(s => s.Id == sessionId);

        if (session == null)
            return null;

        if (session.Status != SessionStatus.Lobby)
            return null;

        if (session.PlayerIds.Contains(playerId))
            return session;

        session.PlayerIds.Add(playerId);

        return session;
    }
}
