using System;
using Newtonsoft.Json;

public class TurbineData
{
    [JsonProperty("id")]
    public string Id
    {
        get { return Guid.NewGuid().ToString(); }
    }

    [JsonProperty("turbineId")]
    public string TurbineId { get; set; }

    [JsonProperty("volt")]
    public float Volt { get; set; }

    [JsonProperty("amp")]
    public float Amp { get; set; }

    [JsonProperty("rpm")]
    public int RPM { get; set; }

    [JsonProperty("timeStamp")]
    public DateTime TimeStamp { get; set; }
}