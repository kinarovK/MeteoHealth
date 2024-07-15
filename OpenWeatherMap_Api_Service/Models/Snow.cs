using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace OpenWeatherMap_Api_Service.Models
{
    public class Snow
    {
        [JsonProperty("3h")]
        public double ThreeHours { get; set; }
    }
}
