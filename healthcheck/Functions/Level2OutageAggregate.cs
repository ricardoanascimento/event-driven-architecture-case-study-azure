using System;
using System.Collections.Generic;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Turbine
{
    public static class Level2OutageAggregate
    {
        [FunctionName("Level2OutageAggregate")]
        public static void Run(
            [ServiceBusTrigger("%TOPIC_NAME%", "%LEVEL2_AGGREGATE_SUBSCRIPTION_NAME%", Connection = "LEVEL2_AGGREGATE_SERVICE_BUS_CONNECTION_STRING")] string telemetry,
            [CosmosDB("%DATABASE_NAME%", "%LEVEL2_AGGREGATE_CONTAINER_NAME%", Connection = "COSMOS_DB_CONNECTION_STRING")] out dynamic document,
            ILogger log)
        {
            log.LogInformation($"ServiceBusTrigger executed at: {DateTime.UtcNow}");
            var turbineData = JsonConvert.DeserializeObject<TurbineData>(telemetry);

            var containerName = Environment.GetEnvironmentVariable("LEVEL2_AGGREGATE_CONTAINER_NAME");

            var cosmosDbService = new CosmosDbService(containerName);
            var timeThresholdInMinutes = int.Parse(Environment.GetEnvironmentVariable("LEVEL2_TIME_THRESHOLD_IN_MINUTES"));
            var voltageThreshold = float.Parse(Environment.GetEnvironmentVariable("VOLTAGE_THRESHOLD"));

            var earliestDate = DateTime.UtcNow.AddMinutes(-timeThresholdInMinutes);
            var latestAggregateDocument = cosmosDbService.GetLatestByTurbineId(turbineData.TurbineId, earliestDate);

            if (latestAggregateDocument != null)
            {
                latestAggregateDocument.Telemetries?.Add(turbineData);
                latestAggregateDocument.TotalVolts += turbineData.Volt;
                latestAggregateDocument.UpdatedAt = turbineData.TimeStamp;
                document = latestAggregateDocument;
            }
            else
            {
                var turbineDataAggregate = new TurbineDataAggregate()
                {
                    Id = Guid.NewGuid().ToString(),
                    TurbineId = turbineData.TurbineId,
                    TotalVolts = turbineData.Volt,
                    Telemetries = new List<TurbineData>() { turbineData },
                    CreatedAt = turbineData.TimeStamp,
                    UpdatedAt = turbineData.TimeStamp
                };
                document = turbineDataAggregate;
            }
        }
    }
}
