using System;
using Newtonsoft.Json;
using System.Collections.Generic;

public class TurbineDataAggregate
{
    [JsonProperty("id")]
    public string Id { get; set; }

    [JsonProperty("turbineId")]
    public string TurbineId { get; set; }

    [JsonProperty("totalVolts")]
    public float TotalVolts { get; set; }

    [JsonProperty("telemetries")]
    public List<TurbineData> Telemetries { get; set; }

    [JsonProperty("createdAt")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updatedAt")]
    public DateTime UpdatedAt { get; set; }
}