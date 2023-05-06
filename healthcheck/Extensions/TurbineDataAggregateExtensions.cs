using System;
using System.Collections.Generic;

public static class TurbineExtensions
{
    public static TurbineDataAggregate AddTelemetry(this TurbineDataAggregate turbineDataAggregate, TurbineData turbineData)
    {
        if (turbineDataAggregate == null)
        {
            turbineDataAggregate = new TurbineDataAggregate();
            turbineDataAggregate.Id = Guid.NewGuid().ToString();
            turbineDataAggregate.TurbineId = turbineData.TurbineId;
            turbineDataAggregate.Telemetries = new List<TurbineData>();
            turbineDataAggregate.CreatedAt = turbineData.TimeStamp;
        }

        turbineDataAggregate.Telemetries.Add(turbineData);
        turbineDataAggregate.TotalVolts += turbineData.Volt;
        turbineDataAggregate.OldestRecordAt = turbineDataAggregate.OldestRecordAt > turbineData.TimeStamp ? turbineDataAggregate.OldestRecordAt : turbineData.TimeStamp;
        return turbineDataAggregate;
    }
}