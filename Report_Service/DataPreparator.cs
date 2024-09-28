using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Report_Service.Interfaces;
using Report_Service.Models;
using SQLite_Database_service.Models;

namespace Report_Service
{
    internal class DataPreparator : IDataPreparator
    {

        public ConvertedWeatherArrayModel ConvertWatherModelsToArray(List<WeatherModel> weatherModels)
        {
            var convertedWeather = new ConvertedWeatherArrayModel();
            convertedWeather.humidityArray = new double[weatherModels.Count];
            convertedWeather.temperatureArray = new double[weatherModels.Count];
            convertedWeather.pressureArray = new double[weatherModels.Count];
            convertedWeather.precipitationProbArray = new double[weatherModels.Count];
            convertedWeather.precipitationVolArray = new double[weatherModels.Count];
            convertedWeather.windSpeedArray = new double[weatherModels.Count];
            for (int i = 0; i < weatherModels.Count; i++)
            {

                convertedWeather.humidityArray[i] = weatherModels[i].Humidity;
                convertedWeather.temperatureArray[i] = weatherModels[i].Temperature;
                convertedWeather.pressureArray[i] = weatherModels[i].Pressure;
                convertedWeather.precipitationProbArray[i] = weatherModels[i].PrecipitationProbability;
                convertedWeather.precipitationVolArray[i] = weatherModels[i].PrecipitationVolume;
                convertedWeather.windSpeedArray[i] = weatherModels[i].WindSpeed;
            }
            return convertedWeather;
        }
        public double[] ConvertHealthStatesToArray(List<HealthStateModel> healthStates)
        {
            double[] healthArray = new double[healthStates.Count];
            for (int i = 0; i < healthArray.Length; i++)
            {
                healthArray[i] = healthStates[i].HealthLevel;
            }
            return healthArray;
        }

        public List<WeatherModel> TrimTheWeatherModels(List<WeatherModel> weatherModels, List<HealthStateModel> healthStateModels)
        {
            var parsedFirstDate = DateTime.Parse(healthStateModels.First().Date);
            var parsedLastDate = DateTime.Parse(healthStateModels.Last().Date).AddDays(1);
            //var newList = weatherModels;
            var trimmedWeatherModels = weatherModels.Where(x =>
            {
                DateTime parsed = DateTime.Parse(x.DateTime);
                return parsed >= parsedFirstDate && parsed < parsedLastDate;
            }).ToList();

            foreach (var item in trimmedWeatherModels)
            {
                //Debug.WriteLine(item.DateTime);
            }
            //SyncHealthLenghtWithWeather(healthStateModels, trimmedWeatherModels);
            return trimmedWeatherModels;

        }
        public List<HealthStateModel> TrimTheHealthStatesModels(List<HealthStateModel> healthState)
        {
            healthState.RemoveAt(0);
            return healthState;
        }

        public async Task<double[]> ExtendHealthStateModels(List<HealthStateModel> healthStates, List<WeatherModel> weatherModels)
        {
            return await Task.Run(() =>
            { 

            double[] healthLevels = new double[weatherModels.Count];
            var healthIndex = 0;
            DateTime previousDate = DateTime.Parse(weatherModels.FirstOrDefault().DateTime).Date;

            for (int i = 0; i < weatherModels.Count - 1; i++)
            {
                DateTime currentWeatherDate = DateTime.Parse(weatherModels[i].DateTime).Date;

                if (previousDate.Date != DateTime.Parse(weatherModels[i].DateTime).Date/*dateTimes[i].Date.Date*/)
                {
                    healthIndex++;
                    previousDate = currentWeatherDate;

                }
                if (currentWeatherDate == DateTime.Parse(healthStates[healthIndex].Date).Date)
                {
                    healthLevels[i] = healthStates[healthIndex].HealthLevel;

                }
            }

            return healthLevels;
            });
        }
        public double[] ExtendHealthLenght(int targertLength, double[] healthState)
        {
            double[] extendedHealth = new double[targertLength];
            int step = 8; //Daily weather records number
            int counter = 0;




            for (int i = 0; i < healthState.Length; i++)
            {
                for (int j = 0; j < step; j++)
                {
                    extendedHealth[counter] = healthState[i];
                    if (counter == targertLength)
                    {
                        break;
                    }
                    counter++;
                }
            }

            return extendedHealth;
        }
        #region for future implementation
        public double[] CalculateDailyMin(int healthStateLength, double[] weatherParam)
        {
            double[] dailyMin = new double[healthStateLength];
            double min = double.MaxValue;
            int counter = 0;
            int index = 0;
            min = weatherParam[0];

            for (int i = 0; i < weatherParam.Length; i++)
            {
                if (weatherParam[i] < min)
                {
                    min = weatherParam[i];
                    dailyMin[index] = min;
                }
                counter++;
                if (counter == 8)
                {
                    counter = 0;
                    index++;
                    min = double.MaxValue;
                }
            }
            return dailyMin;
        }

        public double[] CalculateDailyMax(int healthStateLength, double[] weatherParam)
        {
            double[] dailyMax = new double[healthStateLength];
            double max = double.MaxValue;
            int counter = 0;
            int index = 0;
            max = weatherParam[0];

            for (int i = 0; i < weatherParam.Length; i++)
            {
                if (weatherParam[i] > max)
                {
                    max = weatherParam[i];
                    dailyMax[index] = max;
                }
                counter++;
                if (counter == 8)
                {
                    counter = 0;
                    index++;
                    max = double.MinValue;
                }
            }
            return dailyMax;
        }
        public double[] CalculateDailyAvg(int healthStateLength, double[] weatherParam)
        {
            double[] dailyAvg = new double[healthStateLength];
            double sum = 0;
            int counter = 0;
            int index = 0;


            for (int i = 0; i < weatherParam.Length; i++)
            {
                sum += weatherParam[i];
                counter++;
                if (counter == 8)
                {
                    dailyAvg[index] = sum / 8;
                    counter = 0;
                    index++;
                    sum = 0;
                }
            }
            return dailyAvg;
        }
        public double[] CalculateDailyDelta(double[] max, double[] min)
        {
            double[] delta = new double[max.Length];

            for (int i = 0; i < max.Length; i++)
            {
                delta[i] = Math.Abs(max[i] - min[i]);
            }
            return delta;
        }
        #endregion
    }
}
