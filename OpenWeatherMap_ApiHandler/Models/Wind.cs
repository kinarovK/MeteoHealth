using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_ApiHandler_Service.Models
{
    public class Wind
    {
        public double Speed { get; set; }
        public int Deg { get; set; }
        public double Gust { get; set; }
    }
}
