using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_Api_Service.Models
{
    public class WeatherData
    {
        public long dt { get; set; }
        public Main main { get; set; }
        //public List<Weather> weather { get; set; }
        //public Clouds clouds { get; set; }
        public Wind wind { get; set; }
        //public int visibilty { get; set; }
        //probability of precipitation
        public double pop { get; set; }
        public Rain rain { get; set; }
        public Snow snow { get; set; }
        //public Sys sys { get; set; }
        public string dt_txt { get; set; }
    }
}
