using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace Turbine
{
    public static class Level2Outage
    {
        [FunctionName("Level2Outage")]
        public static void Run(
            [ServiceBusTrigger("%TOPIC_NAME%", "%LEVEL2_SUBSCRIPTION_NAME%", Connection = "LEVEL2_SERVICE_BUS_CONNECTION_STRING")] string telemetry,
            ILogger log)
        {
            log.LogInformation($"ServiceBusTrigger executed at: {DateTime.UtcNow}");

            var turbineService = new TurbineService(telemetry, "LEVEL2");
            var turbineStoppedWorking = turbineService.HasTheTurbineStoppedWorking();

            if (turbineStoppedWorking)
                log.LogError("ALERT: Level 2 power outage detected for turbine {0}", turbineService.latestTelemetry.TurbineId);
        }
    }
}
