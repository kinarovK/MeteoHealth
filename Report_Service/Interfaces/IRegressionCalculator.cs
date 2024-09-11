using MathNet.Numerics.LinearRegression;
using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Report_Service.Models;

namespace Report_Service.Interfaces
{
    internal interface IRegressionCalculator
    {
        public double CalculateSingleRegression(double[] healthState, double[] weatherParam);
        public double CalculateMultipleRegression(ConvertedWeatherArrayModel convertedWeatherArrayModel, double[] healthStates);
    }
}
