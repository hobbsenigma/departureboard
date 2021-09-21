using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DepartureBoard.Models
{
    public class Prediction
    {
        public string Id { get; set; }

        [JsonProperty(propertyName: "departure_time")]
        public string DepartureTime { get; set; }
        public string Status { get; set; }        
        public Schedule Schedule { get; set; }
        public Vehicle Vehicle { get; set; }
        public Route Route { get; set; }
        public Trip Trip { get; set; }
        public Stop Stop { get; set; }
    }
}
