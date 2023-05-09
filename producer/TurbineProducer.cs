using System;
using System.IO;
using System.Text.Json;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Turbine.Producer
{
    public class TurbineProducer
    {
        [FunctionName("TurbineProducer")]
        [return: ServiceBus("%TOPIC_NAME%", Connection = "SERVICE_BUS_CONNECTION_STRING")]
        // public static async Task<string> RunAsync(
        public static string Run(
            // [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            [TimerTrigger("*/10 * * * * *")] TimerInfo myTimer,
            ILogger log)
        {
            log.LogInformation($"HttpTrigger executed at: {DateTime.Now}");

            var serializedEvent = string.Empty;

            // try
            // {
            //     string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //     dynamic data = JsonSerializer.Deserialize<TurbineData>(requestBody);
            //     serializedEvent = JsonSerializer.Serialize(data);
            // }
            // catch (Exception ex)
            // {
            var random = new Random();
            var turbineData = new TurbineData()
            {
                TurbineId = "eaa0ff86-12a9-496e-9cd5-38ce609dce17",
                // Volt = (float)random.NextDouble() * 1000,
                Volt = 89,
                Amp = (float)random.NextDouble() * 100,
                RPM = random.Next(1000, 2000),
                TimeStamp = DateTime.UtcNow
            };

            serializedEvent = JsonSerializer.Serialize(turbineData);

            //     log.LogError(ex, "Error parsing event!");
            // }

            log.LogInformation($"Pushig event: {serializedEvent}");

            return serializedEvent;
        }
    }

    public class TurbineData
    {
        public string TurbineId { get; set; }
        public float Volt { get; set; }
        public float Amp { get; set; }
        public int RPM { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
