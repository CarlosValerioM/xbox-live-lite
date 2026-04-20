using Microsoft.Azure.Cosmos;
using XboxLiveLite.Api.Models;

namespace XboxLiveLite.Api.Data;

public class SessionRepository
{
    private readonly Container _container;

    public SessionRepository(IConfiguration config)
    {
        var client = new CosmosClient(config["CosmosDb:ConnectionString"]);
        var database = client.GetDatabase(config["CosmosDb:DatabaseName"]);
        _container = database.GetContainer(config["CosmosDb:SessionsContainer"]);
    }

    public async Task<Session> CreateAsync(Session session)
    {
        var response = await _container.CreateItemAsync(session, new PartitionKey(session.Id));
        return response.Resource;
    }

    public async Task<Session?> GetByIdAsync(string id)
    {
        try
        {
            var response = await _container.ReadItemAsync<Session>(id, new PartitionKey(id));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
    }

    public async Task<Session> UpdateAsync(Session session)
    {
        var response = await _container.UpsertItemAsync(session, new PartitionKey(session.Id));
        return response.Resource;
    }
}