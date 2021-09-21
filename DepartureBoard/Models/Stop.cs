using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DepartureBoard.Models
{
    public class Stop
    {
        public string Id { get; set; }
        [JsonProperty(propertyName: "vehicle_type")]
        public string VehicleType { get; set; }
        [JsonProperty(propertyName: "platform_name")]
        public string PlatformName { get; set; }
        [JsonProperty(propertyName: "platform_code")]
        public string PlatformCode { get; set; }
        public string Name { get; set; }
        [JsonProperty(propertyName: "child_stops")]
        public Stop[] ChildStops { get; set; }
    }
}
