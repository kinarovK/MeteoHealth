using OpenWeatherMap_ApiHandler_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_ApiHandler_Service
{
    internal class OpenWeatherMapHttpClient : IWeatherApiHttpClient
    {
        public virtual HttpClient HttpClient { get; }
        public OpenWeatherMapHttpClient(HttpClient client)
        {
            HttpClient = client;
        }
    }
}
