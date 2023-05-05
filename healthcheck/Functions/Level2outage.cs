using System;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Turbine
{
    public static class Level2outage
    {
        [FunctionName("Level2outage")]
        public static void Run(
            [ServiceBusTrigger("%TOPIC_NAME%", "%LEVEL2_SUBSCRIPTION_NAME%", Connection = "LEVEL2_SERVICE_BUS_CONNECTION_STRING")] string telemetry,
            ILogger log)
        {
            log.LogInformation($"ServiceBusTrigger executed at: {DateTime.UtcNow}");
            var turbineData = JsonConvert.DeserializeObject<TurbineData>(telemetry);

            var cosmosDbService = new CosmosDbService();
            var timeThresholdInMinutes = int.Parse(Environment.GetEnvironmentVariable("LEVEL2_TIME_THRESHOLD_IN_MINUTES"));
            var earlistAmountOfSeconds = timeThresholdInMinutes + 1;
            var voltageThreshold = float.Parse(Environment.GetEnvironmentVariable("VOLTAGE_THRESHOLD"));

            var documentsWithinRange = cosmosDbService.GetLatestDocumentsWihtinRangeByTurbineId(turbineData.TurbineId, earlistAmountOfSeconds);
            var isEveryThingAnOutage = !documentsWithinRange.Any(e => e.Volt >= voltageThreshold);

            if (isEveryThingAnOutage)
            {
                var outageWithinTimeFrame = documentsWithinRange.Any(doc => Math.Abs((turbineData.TimeStamp - doc.TimeStamp).TotalMinutes) >= timeThresholdInMinutes);
                if (outageWithinTimeFrame)
                {
                    log.LogWarning("ALERT: Level 2 power outage detected for turbine {0}", turbineData.TurbineId);
                }
            }
        }
    }
}
