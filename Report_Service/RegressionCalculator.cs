using MathNet.Numerics.LinearRegression;
using MathNet.Numerics;
using Report_Service.Interfaces;
using Report_Service.Models;

namespace Report_Service
{
    internal class RegressionCalculator : IRegressionCalculator
    {
        public double CalculateSingleRegression(double[] healthState, double[] weatherParam)
        {
            var model = SimpleRegression.Fit(weatherParam, healthState);
            double intercept = model.A;
            double coefficient = model.B;
            var predictedHealthStates = new double[weatherParam.Length];
            for (int i = 0; i < weatherParam.Length; i++)
            {
                predictedHealthStates[i] = intercept + coefficient * weatherParam[i];
            }
            double rSquarred = GoodnessOfFit.RSquared(predictedHealthStates, healthState);

            return rSquarred;
        }

        public double CalculateMultipleRegression(ConvertedWeatherArrayModel convertedWeatherArrayModel, double[] healthStates)
        {
            var weathersMatrix = new double[convertedWeatherArrayModel.humidityArray.Length][];
            for (int i = 0; i < weathersMatrix.Length; i++)
            {
                weathersMatrix[i] = new double[] {  convertedWeatherArrayModel.temperatureArray[i],
                                                convertedWeatherArrayModel.humidityArray[i],
                                                convertedWeatherArrayModel.pressureArray[i],
                                                convertedWeatherArrayModel.precipitationProbArray[i],
                                                convertedWeatherArrayModel.precipitationVolArray[i],
                                                convertedWeatherArrayModel.windSpeedArray[i]};
            }
            var multiModel = MultipleRegression.QR(weathersMatrix, healthStates, intercept: true);
            double interceptMulti = multiModel[0];
            var tempCoeff = multiModel[1];
            var humidityCoeff = multiModel[2];
            var pressCoeff = multiModel[3];
            var precProbCoeff = multiModel[4];
            var precVolCoeff = multiModel[5];
            var windSpeedCoeff = multiModel[6];

            var predictecMultiHealth = new double[healthStates.Length];
            for (int i = 0; i < healthStates.Length; i++)
            {
                predictecMultiHealth[i] = interceptMulti + tempCoeff * convertedWeatherArrayModel.temperatureArray[i]
                                                        + humidityCoeff * convertedWeatherArrayModel.humidityArray[i]
                                                        + pressCoeff * convertedWeatherArrayModel.pressureArray[i]
                                                        + precProbCoeff * convertedWeatherArrayModel.precipitationProbArray[i]
                                                        + precVolCoeff * convertedWeatherArrayModel.precipitationVolArray[i]
                                                        + windSpeedCoeff * convertedWeatherArrayModel.windSpeedArray[i];
            }
            var rSquared = GoodnessOfFit.RSquared(predictecMultiHealth, healthStates);
            return rSquared;
        }
    }
}
