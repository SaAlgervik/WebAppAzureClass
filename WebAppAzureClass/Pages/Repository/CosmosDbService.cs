using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

public class CosmosDbService : ICosmosDbService
{
    private Container _container;
    public CosmosDbService(
    CosmosClient cosmosDbClient,
    string databaseName,
    string containerName)
    {
        _container = cosmosDbClient.GetContainer(databaseName, containerName);
    }
    public Task AddAsync(User item)
    {
        throw new System.NotImplementedException();
    }
}


