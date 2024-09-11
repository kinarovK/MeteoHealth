using SQLite_Database_service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Service.Models
{
    public class ConvertedWeatherArrayModel
    {
        public double[] temperatureArray;
        public double[] humidityArray;
        public double[] precipitationProbArray;
        public double[] precipitationVolArray;
        public double[] pressureArray;
        public double[] windSpeedArray;

    }
}
