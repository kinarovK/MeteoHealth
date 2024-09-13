using OpenWeatherMap_Api_Service.Models;
using SQLite_Database_service.Models;
using System;
using System.Collections.Generic;

namespace MeteoHealth.Services
{
    public class OpenWeatherMapConverter : IOpenWeatherMapConverter 
    {
        
       
        public List<WeatherModel> ConvertToModel(WeatherApiResponse response) 
        {
            var result = new List<WeatherModel>();

            foreach (var item in response.list)
            {
                result.Add(new WeatherModel
                {
                    RequestDate = DateTime.Today.ToString("yyyy.MM.dd"),
                    DateTime = item.dt_txt,
                    Humidity = item.main.humidity,
                    PrecipitationProbability = (int)item.pop * 100,
                    PrecipitationVolume = (item.rain?.ThreeHours ?? 0) + (item.snow?.ThreeHours ?? 0),
                    Pressure = item.main.pressure,
                    Temperature = item.main.temp,
                    WindSpeed = item.wind.speed
                });
            }
            return result;
        }
    }
}
