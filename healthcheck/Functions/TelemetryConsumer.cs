using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Turbine
{
    public static class TelemetryConsumer
    {
        [FunctionName("TelemetryConsumer")]
        public static void Run(
            [ServiceBusTrigger("%TOPIC_NAME%", "%CONSUMER_SUBSCRIPTION_NAME%", Connection = "CONSUMER_SERVICE_BUS_CONNECTION_STRING")] string telemetry,
            [CosmosDB("%DATABASE_NAME%", "%CONTAINER_NAME%", Connection = "COSMOS_DB_CONNECTION_STRING")] out dynamic document,
            ILogger log)
        {
            log.LogInformation($"ServiceBusTrigger executed at: {DateTime.UtcNow}");
            var data = JsonConvert.DeserializeObject<TurbineData>(telemetry);
            var serializedEvent = JsonConvert.SerializeObject(data);
            log.LogInformation($"Reading event: {serializedEvent}");
            document = serializedEvent;
        }
    }
}
