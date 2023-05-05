using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;

public class CosmosDbService
{
    private readonly CosmosClient _cosmosClient;
    private readonly string _databaseName;
    private readonly string _containerName;

    public CosmosDbService()
    {
        var connectionString = Environment.GetEnvironmentVariable("COSMOS_DB_CONNECTION_STRING");
        _cosmosClient = new CosmosClient(connectionString);
        _databaseName = Environment.GetEnvironmentVariable("DATABASE_NAME");
        _containerName = Environment.GetEnvironmentVariable("CONTAINER_NAME");
    }

    public List<TurbineData> GetLatestDocumentsWihtinRangeByTurbineId(string turbineId, int timeThresholdInMinutes)
    {
        var container = _cosmosClient.GetContainer(_databaseName, _containerName);
        var query = new QueryDefinition("SELECT * FROM c WHERE c.turbineId = @turbineId AND c.timeStamp >= @timeThreshold ORDER BY c.timeStamp DESC")
            .WithParameter("@turbineId", turbineId)
            .WithParameter("@timeThreshold", DateTime.UtcNow.AddMinutes(-timeThresholdInMinutes).ToString("o"));
        var iterator = container.GetItemQueryIterator<TurbineData>(query);

        var documents = new List<TurbineData>();

        while (iterator.HasMoreResults)
        {
            var response = iterator.ReadNextAsync().Result;
            foreach (var document in response)
            {
                documents.Add(document);
            }
        }

        return documents;
    }
}