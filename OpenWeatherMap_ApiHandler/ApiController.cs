using OpenWeatherMap_ApiHandler_Service.Interfaces;
using OpenWeatherMap_ApiHandler_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_ApiHandler_Service
{
    internal class ApiController : IApiController
    {
        private readonly IApiRequest apiRequest;

        public ApiController(IApiRequest apiRequest)
        {
            this.apiRequest = apiRequest;
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
