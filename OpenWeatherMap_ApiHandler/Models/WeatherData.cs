using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_ApiHandler_Service.Models
{
    public class WeatherData
    {
        public long Dt { get; set; }
        public Main Main { get; set; }
        public List<Weather> Weather { get; set; }
        public Clouds Clouds { get; set; }
        public Wind Wind { get; set; }
        public int Visibilty { get; set; }
        public double Pop { get; set; }
        public Sys Sys { get; set; }
        public string DtTxt { get; set; }
    }
}
