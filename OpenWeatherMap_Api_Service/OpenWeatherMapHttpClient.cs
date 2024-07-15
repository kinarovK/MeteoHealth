using OpenWeatherMap_Api_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_Api_Service
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
