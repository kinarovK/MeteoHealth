using Newtonsoft.Json;

namespace OpenWeatherMap_Api_Service.Models
{
    public class Rain
    {
        [JsonProperty("3h")]
        public double ThreeHours { get; set; }
    }
}
