using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_Api_Service
{
    public class ApplicationOptions
    {
        public const string AppSettings = "AppSettings";
        public string ApiUrlBase { get; set; }
        public string ApiKey { get; set; }
    }
}
