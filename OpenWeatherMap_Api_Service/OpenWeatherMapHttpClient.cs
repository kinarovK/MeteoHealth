using OpenWeatherMap_Api_Service.Interfaces;
using System.Net.Http;

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
