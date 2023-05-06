using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Turbine
{
    public static class Level1OutageAggregate
    {
        [FunctionName("Level1OutageAggregate")]
        public static void Run(
            [ServiceBusTrigger("%TOPIC_NAME%", "%LEVEL1_AGGREGATE_SUBSCRIPTION_NAME%", Connection = "LEVEL1_AGGREGATE_SERVICE_BUS_CONNECTION_STRING")] string telemetry,
            [CosmosDB("%DATABASE_NAME%", "%LEVEL1_AGGREGATE_CONTAINER_NAME%", Connection = "COSMOS_DB_CONNECTION_STRING")] out dynamic document,
            ILogger log)
        {
            log.LogInformation($"ServiceBusTrigger executed at: {DateTime.UtcNow}");

            var timeThresholdInMinutes = int.Parse(Environment.GetEnvironmentVariable("LEVEL1_TIME_THRESHOLD_IN_MINUTES"));
            var earliestDate = DateTime.UtcNow.AddMinutes(-timeThresholdInMinutes);

            var turbineService = new TurbineService(telemetry, "LEVEL1", earliestDate);
            var turbineDataAggregate = turbineService.GetMostRecentTurbineDataAggregate();
            document = turbineDataAggregate;
        }
    }
}
