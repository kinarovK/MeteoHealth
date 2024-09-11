using OpenWeatherMap_ApiHandler_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_ApiHandler_Service.Interfaces
{
    public interface IApiController
    {
        Task<WeatherApiResponse> ExecuteApiRequest(string city);
    }
}
