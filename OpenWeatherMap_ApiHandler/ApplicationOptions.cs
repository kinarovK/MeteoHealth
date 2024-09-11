using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_ApiHandler_Service
{
    internal class ApplicationOptions
    {
        public const string AppSettings = "AppSettings";
        public string ApiUrl { get; set; }
        public string ApiKey { get; set; }
    }
}
