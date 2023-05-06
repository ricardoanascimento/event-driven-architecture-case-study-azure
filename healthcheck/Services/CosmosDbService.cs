using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq;

public class CosmosDbService
{
    private readonly CosmosClient _cosmosClient;
    private readonly string _databaseName;
    private readonly string _containerName;

    public CosmosDbService(string containerName)
    {
        var connectionString = Environment.GetEnvironmentVariable("COSMOS_DB_CONNECTION_STRING");
        _cosmosClient = new CosmosClient(connectionString);
        _databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
        _containerName = containerName;
    }

    public TurbineDataAggregate GetLatestTurbineDataAggregateByTurbineId(string turbineId, DateTime earliestDate)
    {
        var container = _cosmosClient.GetContainer(_databaseName, _containerName);
        var query = new QueryDefinition("SELECT TOP 1 * FROM t WHERE t.turbineId = @turbineId AND t.createdAt >= @timeThreshold ORDER BY t.createdAt DESC")
            .WithParameter("@turbineId", turbineId)
            .WithParameter("@timeThreshold", earliestDate.ToString("o"));
        var iterator = container.GetItemQueryIterator<TurbineDataAggregate>(query);

        var documents = new List<TurbineDataAggregate>();

        while (iterator.HasMoreResults)
        {
            var response = iterator.ReadNextAsync().Result;
            foreach (var document in response)
            {
                documents.Add(document);
            }
        }

        return documents.FirstOrDefault();
    }
}