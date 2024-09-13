using Newtonsoft.Json;

namespace OpenWeatherMap_Api_Service.Models
{
    public class Snow
    {
        [JsonProperty("3h")]
        public double ThreeHours { get; set; }
    }
}
