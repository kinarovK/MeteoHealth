using System.Collections.Generic;

namespace OpenWeatherMap_Api_Service.Models
{
    public class Wind
    {
        public double speed { get; set; }
        public int deg { get; set; }
        public double gust { get; set; }
    }
}
