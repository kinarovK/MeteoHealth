using OpenWeatherMap_Api_Service.Models;
using SQLite_Database_service;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoHealth.Services
{
    public class WeatherApiService : IWeatherApiService //maybe naming not to good
    {
        
        //maybe make it async
        public List<WeatherModel> ConvertToModel(WeatherApiResponse response) //Add Some custom exception 
        {
            var result = new List<WeatherModel>();

            foreach (var item in response.list)
            {
                result.Add(new WeatherModel
                {
                    RequestData = DateTime.Today.ToString("yyyy.MM.DD"),
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
