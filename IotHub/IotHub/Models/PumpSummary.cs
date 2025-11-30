using System;
using Newtonsoft.Json;

namespace IotHub.Models
{
    public class PumpSummary
    {
        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("lastSeen")]
        public DateTime LastSeen { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("avgTemperature")]
        public double AvgTemperature { get; set; }

        [JsonProperty("avgPressure")]
        public double AvgPressure { get; set; }

        [JsonProperty("alertCount")]
        public int AlertCount { get; set; }
    }
}