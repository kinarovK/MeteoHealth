﻿using OpenWeatherMap_Api_Service.Models;
using System.Threading;
using System.Threading.Tasks;

namespace OpenWeatherMap_Api_Service.Interfaces
{
    public interface IOpenWeatherMapApiController
    {
     
        Task<WeatherApiResponse> ExecuteApiRequest(string lat, string lon, CancellationToken token);

    }
}
