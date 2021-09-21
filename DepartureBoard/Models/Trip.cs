using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DepartureBoard.Models
{
    public class Trip
    {
        public string Id { get; set; }

        [JsonProperty(propertyName: "headsign")]
        public string Destination { get; set; }

        public string Name { get; set; }
    }
}
