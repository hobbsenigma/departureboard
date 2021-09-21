using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DepartureBoard.Models
{
    public class Vehicle
    {
        public string Id { get; set; }

        [JsonProperty(propertyName: "label")]
        public string Number { get; set; }
        public Trip Trip { get; set; }
    }

}
