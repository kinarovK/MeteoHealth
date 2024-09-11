using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_ApiHandler_Service.Models
{
    public class WeatherApiResponse
    {
        public string Cod { get; set; }
        public int Message { get; set; }
        public int Cnt { get; set; }
        public List<WeatherData> List { get; set; }
    }
}
