using Microsoft.Azure.Cosmos;
using XboxLiveLite.Api.Models;

namespace XboxLiveLite.Api.Data;

public class PlayerRepository
{
    private readonly Container _container;

    public PlayerRepository(IConfiguration config)
    {
        var client = new CosmosClient(config["CosmosDb:ConnectionString"]);
        var database = client.GetDatabase(config["CosmosDb:DatabaseName"]);
        _container = database.GetContainer(config["CosmosDb:PlayersContainer"]);
    }

    public async Task<Player> CreatePlayerAsync(Player player)
    {
        var response = await _container.CreateItemAsync(player, new PartitionKey(player.Id));
        return response.Resource;
    }

    public async Task<Player?> GetByGamertagAsync(string gamertag)
    {
        var query = new QueryDefinition("SELECT * FROM c WHERE c.Gamertag = @gamertag")
            .WithParameter("@gamertag", gamertag);

        var iterator = _container.GetItemQueryIterator<Player>(query);

        while (iterator.HasMoreResults)
        {
            var results = await iterator.ReadNextAsync();
            return results.FirstOrDefault();
        }

        return null;
    }
}