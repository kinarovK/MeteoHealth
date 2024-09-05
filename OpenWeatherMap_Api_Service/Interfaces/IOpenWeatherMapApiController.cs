using OpenWeatherMap_Api_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenWeatherMap_Api_Service.Interfaces
{
    public interface IOpenWeatherMapApiController
    {
        Task<WeatherApiResponse> ExecuteApiRequest(string city);
        Task<WeatherApiResponse> ExecuteApiRequest(string lat, string lon, CancellationToken token);

    }
}
