using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

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

            var turbineService = new TurbineService(telemetry, "LEVEL2");
            var turbineDataAggregate = turbineService.GetMostRecentTurbineDataAggregate();
            document = turbineDataAggregate;
        }
    }
}
