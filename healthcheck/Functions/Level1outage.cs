using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using static AlertService;

namespace Turbine
{
    public static class Level1Outage
    {
        [FunctionName("Level1Outage")]
        public static void Run(
            [ServiceBusTrigger("%TOPIC_NAME%", "%LEVEL1_SUBSCRIPTION_NAME%", Connection = "LEVEL1_SERVICE_BUS_CONNECTION_STRING")] string telemetry,
            ILogger log)
        {
            log.LogInformation($"ServiceBusTrigger executed at: {DateTime.UtcNow}");

            var timeThresholdInSeconds = int.Parse(Environment.GetEnvironmentVariable("LEVEL1_TIME_THRESHOLD_IN_SECONDS"));
            var earliestDate = GetEarliestDateToProccessAlert(timeThresholdInSeconds);

            var turbineService = new TurbineService(telemetry, "LEVEL1", earliestDate);
            var turbineStoppedWorking = turbineService.HasTheTurbineStoppedWorking();

            if (turbineStoppedWorking)
                log.LogError("ALERT: Level 1 power outage detected for turbine {0}", turbineService.latestTelemetry.TurbineId);
        }
    }
}
