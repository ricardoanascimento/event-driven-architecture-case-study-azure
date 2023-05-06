using System;
using System.Linq;
using Newtonsoft.Json;

public class TurbineService
{
    public readonly TurbineData latestTelemetry;

    private readonly CosmosDbService _cosmosDbService;
    private readonly int _timeThresholdInSeconds;
    private readonly float _voltageThreshold;
    private readonly DateTime _earliestDate;

    public TurbineService(string telemetry, string environmentPreFix, DateTime earliestDate)
    {
        latestTelemetry = JsonConvert.DeserializeObject<TurbineData>(telemetry);

        var containerName = Environment.GetEnvironmentVariable(environmentPreFix + "_AGGREGATE_CONTAINER_NAME");
        _cosmosDbService = new CosmosDbService(containerName);

        _timeThresholdInSeconds = int.Parse(Environment.GetEnvironmentVariable(environmentPreFix + "_TIME_THRESHOLD_IN_SECONDS"));

        _voltageThreshold = float.Parse(Environment.GetEnvironmentVariable("VOLTAGE_THRESHOLD"));

        _earliestDate = earliestDate;
    }

    public bool HasTheTurbineStoppedWorking()
    {
        var latestAggregateDocument = GetMostRecentTurbineDataAggregate();

        var isEveryThingAnOutage = !latestAggregateDocument.Telemetries.Any(e => e.Volt >= _voltageThreshold);
        var isWhitinAcceptableRange = (latestAggregateDocument.CreatedAt.AddSeconds(_timeThresholdInSeconds) - latestAggregateDocument.OldestRecordAt).TotalSeconds <= 10;

        return isEveryThingAnOutage && isWhitinAcceptableRange;
    }

    public TurbineDataAggregate GetMostRecentTurbineDataAggregate()
    {
        var latestAggregateDocument = _cosmosDbService.GetLatestTurbineDataAggregateByTurbineId(latestTelemetry.TurbineId, _earliestDate);
        return latestAggregateDocument.AddTelemetry(latestTelemetry);
    }
}