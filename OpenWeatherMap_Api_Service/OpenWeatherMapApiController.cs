using OpenWeatherMap_Api_Service.Interfaces;
using OpenWeatherMap_Api_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OpenWeatherMap_Api_Service
{
    internal class OpenWeatherMapApiController : IOpenWeatherMapApiController
    {
        private readonly IOpenWeatherMapApiRequest apiRequest;

        public OpenWeatherMapApiController(IOpenWeatherMapApiRequest apiRequest)
        {
            this.apiRequest = apiRequest;
        }
        public async Task<WeatherApiResponse> ExecuteApiRequest(string lat, string lon, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var apiGetResult = await apiRequest.GetWeatherByCoordinates(lat, lon, token);

            if (apiGetResult is null)
            {
                return null;
            }
            return apiGetResult;

        }
     
    }
}
