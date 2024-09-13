namespace OpenWeatherMap_Api_Service.Models
{
    public class WeatherData
    {
        public long dt { get; set; }
        public Main main { get; set; }
        public Wind wind { get; set; }
        public double pop { get; set; }
        public Rain rain { get; set; }
        public Snow snow { get; set; }
        public string dt_txt { get; set; }
    }
}
