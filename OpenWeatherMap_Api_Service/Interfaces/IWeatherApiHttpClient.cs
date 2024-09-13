using System.Net.Http;

namespace OpenWeatherMap_Api_Service.Interfaces
{
    public interface IWeatherApiHttpClient
    {
        HttpClient HttpClient { get; }
    }
}
