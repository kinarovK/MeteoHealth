using Report_Service.Exceptions;
using Report_Service.Interfaces;
using Report_Service.Models;
using SQLite_Database_service.Interfaces;
using SQLite_Database_service.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Report_Service
{
    internal class ReportMaker : IReportMaker
    {
        private readonly IMeteoHealthRepository meteoHealthRepository;
        private readonly IRegressionCalculator regressionCalculator;
        private readonly IDataPreparator dataPreparator;

        public ReportMaker(IMeteoHealthRepository meteoHealthRepository, IRegressionCalculator regressionCalculator, IDataPreparator dataPreparator)
        {
            this.meteoHealthRepository = meteoHealthRepository;
            this.regressionCalculator = regressionCalculator;
            this.dataPreparator = dataPreparator;
        }

        public async Task<ReportModel> GetReportAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var healthStates = await GetHealthFromDb(cancellationToken);
            var weathers  =await GetWeathersFromDb(cancellationToken);
        
            if (healthStates.Count < 10)
            {
                throw new NotEnoughDataToReportException();
            }
            healthStates = dataPreparator.TrimTheHealthStatesModels(healthStates);
            weathers = dataPreparator.TrimTheWeatherModels(weathers, healthStates);
            var convertedWeathers = dataPreparator.ConvertWatherModelsToArray(weathers);

            var convertedHealthStates = dataPreparator.ConvertHealthStatesToArray(healthStates);
            var extendedHealthStates = dataPreparator.ExtendHealthLenght(weathers.Count, convertedHealthStates);

            var result = new ReportModel();
            result.TemperatureRelation = regressionCalculator.CalculateSingleRegression(extendedHealthStates, convertedWeathers.temperatureArray);
            result.HumidityRelation = regressionCalculator.CalculateSingleRegression(extendedHealthStates, convertedWeathers.humidityArray);
            result.PressureRelation = regressionCalculator.CalculateSingleRegression(extendedHealthStates, convertedWeathers.pressureArray);
            result.PrecProbabilityRelation = regressionCalculator.CalculateSingleRegression(extendedHealthStates, convertedWeathers.precipitationProbArray);
            result.PrecVolRelation = regressionCalculator.CalculateSingleRegression(extendedHealthStates, convertedWeathers.precipitationVolArray);
            result.WindRelation = regressionCalculator.CalculateSingleRegression(extendedHealthStates, convertedWeathers.windSpeedArray);
            result.FullRelation = regressionCalculator.CalculateMultipleRegression(convertedWeathers, extendedHealthStates);
            result.FirstDate = healthStates.FirstOrDefault().Date;
            result.LastDate = healthStates.LastOrDefault().Date;
            return result;
        }

        internal async Task<List<HealthStateModel>> GetHealthFromDb(CancellationToken cancellationToken)
        {
            return await meteoHealthRepository.GetHealthStatesAsync(cancellationToken);
        }
        internal async Task<List<WeatherModel>> GetWeathersFromDb(CancellationToken cancellationToken)
        {
            return await  meteoHealthRepository.GetWeatherModelAsync(cancellationToken);
        }
    }
}
