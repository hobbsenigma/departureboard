using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DepartureBoard.Models
{
    public class Schedule
    {
        public string Id { get; set; }

        public DateTime DepartureTime;
        [JsonProperty(propertyName: "departure_time")] //uses standard Json.NET attributes to control serialization
        public string DepartureTimeSource {             
            set { DepartureTime = DateTime.Parse(value); } 
        }
        public Prediction Prediction { get; set; }
        public Route Route { get; set; }
        public Trip Trip { get; set; }        
        public Stop Stop { get; set; }
    }
}
