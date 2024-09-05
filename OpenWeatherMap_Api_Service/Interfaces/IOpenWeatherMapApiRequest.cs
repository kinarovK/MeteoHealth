using OpenWeatherMap_Api_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenWeatherMap_Api_Service.Interfaces
{
    public interface IOpenWeatherMapApiRequest
    {
        public Task<WeatherApiResponse> GetWeatherByCity(string city);
        public Task<WeatherApiResponse> GetWeatherByCoordinates(string lat, string lon, CancellationToken token);
    }
}
