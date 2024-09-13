using Report_Service.Models;

namespace Report_Service.Interfaces
{
    internal interface IRegressionCalculator
    {
        public double CalculateSingleRegression(double[] healthState, double[] weatherParam);
        public double CalculateMultipleRegression(ConvertedWeatherArrayModel convertedWeatherArrayModel, double[] healthStates);
    }
}
