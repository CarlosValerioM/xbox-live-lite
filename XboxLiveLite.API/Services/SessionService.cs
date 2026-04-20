using XboxLiveLite.Api.Data;
using XboxLiveLite.Api.Models;

namespace XboxLiveLite.Api.Services;

public interface ISessionService
{
    Task<Session> CreateSession(string playerId);
    Task<Session?> GetSession(string id);
    Task<Session?> JoinSession(string sessionId, string playerId);
}

public class SessionService : ISessionService
{
    private readonly SessionRepository _repo;

    public SessionService(SessionRepository repo)
    {
        _repo = repo;
    }

    public async Task<Session> CreateSession(string playerId)
    {
        var session = new Session
        {
            HostPlayerId = playerId,
            PlayerIds = new List<string> { playerId }
        };

        return await _repo.CreateAsync(session);
    }

    public async Task<Session?> GetSession(string id)
    {
        return await _repo.GetByIdAsync(id);
    }

    public async Task<Session?> JoinSession(string sessionId, string playerId)
    {
        var session = await _repo.GetByIdAsync(sessionId);

        if (session == null)
            return null;

        if (session.Status != SessionStatus.Lobby)
            return null;

        if (!session.PlayerIds.Contains(playerId))
            session.PlayerIds.Add(playerId);

        return await _repo.UpdateAsync(session);
    }
}