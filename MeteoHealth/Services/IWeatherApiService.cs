using OpenWeatherMap_Api_Service.Models;
using SQLite_Database_service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoHealth.Services
{
    public interface IWeatherApiService
    {
        public List<WeatherModel> ConvertToModel(WeatherApiResponse response);
    }
}
