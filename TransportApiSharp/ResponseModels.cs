using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TransportAPISharp
{
    /// <summary>
    /// The return class for <c>TransportApiClient.BusStopsNear</c>
    /// </summary>
    public class BusStopsNearResponse
    {
        public float minlon { get; set; }
        public float minlat { get; set; }
        public float maxlon { get; set; }
        public float maxlat { get; set; }
        public float searchlon { get; set; }
        public float searchlat { get; set; }
        public int page { get; set; }
        public int rpp { get; set; }
        public int total { get; set; }
        public DateTime request_time { get; set; }
        public Stop[] stops { get; set; }
    }

    public class Stop
    {
        public string atcocode { get; set; }
        public string smscode { get; set; }
        public string name { get; set; }
        public string mode { get; set; }
        public string bearing { get; set; }
        public string locality { get; set; }
        public string indicator { get; set; }
        public float longitude { get; set; }
        public float latitude { get; set; }
        public int distance { get; set; }
    }

    public class BusTimetableResponse
    {
        [JsonProperty("atcocode")]
        public string Atcocode { get; set; }

        [JsonProperty("smscode")]
        public string Smscode { get; set; }

        [JsonProperty("request_time")]
        public DateTime RequestTime { get; set; }

        [JsonProperty("bearing")]
        public string Bearing { get; set; }

        [JsonProperty("stop_name")]
        public string StopName { get; set; }

        [JsonProperty("departures")]
        public Dictionary<string, List<BusDeparture>> Departures { get; set; }
    }

    public class BusDeparture
    {
        [JsonProperty("mode")]
        public string Mode { get; set; }

        [JsonProperty("line")]
        public string Line { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("operator")]
        public string Operator { get; set; }

        [JsonProperty("aimed_departure_time")]
        public TimeSpan AimedDepartureTime { get; set; }

        [JsonProperty("dir")]
        public string Dir { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonIgnore]
        public DateTime AimedDepartureDateTime
        {
            get
            {
                return new DateTime(Date.Year, Date.Month, Date.Day, AimedDepartureTime.Hours, AimedDepartureTime.Minutes, AimedDepartureTime.Seconds);
            }
        }
    }
}