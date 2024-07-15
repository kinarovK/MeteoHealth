using OpenWeatherMap_Api_Service.Interfaces;
using OpenWeatherMap_Api_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_Api_Service
{
    internal class ApiController : IApiController
    {
        private readonly IApiRequest apiRequest;

        public ApiController(IApiRequest apiRequest)
        {
            this.apiRequest = apiRequest;
        }
        public async Task<WeatherApiResponse> ExecuteApiRequest(string lat, string lon)
        {
            var apiGetResult = await apiRequest.GetWeatherByCoordinates(lat, lon);

            if (apiGetResult is null)
            {
                return null;
            }
            return apiGetResult;

        }
        public async Task<WeatherApiResponse> ExecuteApiRequest(string city)
        {
            var apiGetResult = await apiRequest.GetWeatherByCity(city);

            if (apiGetResult is null)
            {
                return null;
            }
            return apiGetResult;

        }
    }
}
