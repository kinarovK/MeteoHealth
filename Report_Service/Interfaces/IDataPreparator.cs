using Report_Service.Models;
using SQLite_Database_service.Models;
using System.Collections.Generic;

namespace Report_Service.Interfaces
{
    public interface IDataPreparator
    {
        public ConvertedWeatherArrayModel ConvertWatherModelsToArray(List<WeatherModel> weatherModels);

        public double[] ExtendHealthLenght(int targetLength, double[] healthState);
        public double[] ConvertHealthStatesToArray(List<HealthStateModel> healthStates);



        public List<WeatherModel> TrimTheWeatherModels(List<WeatherModel> weatherModels, List<HealthStateModel> healthStateModels);

        public List<HealthStateModel> TrimTheHealthStatesModels(List<HealthStateModel> weatherModels);
        public double[] CalculateDailyMin(int healthStateLength, double[] weatherParam);
        public double[] CalculateDailyMax(int healthStateLength, double[] weatherParam);
        public double[] CalculateDailyAvg(int healthStateLength, double[] weatherParam);
        public double[] CalculateDailyDelta(double[] max, double[] min);
        //public double[] InterpolateHealthStates(int targetLentght, double[] healthStateData);

    }
}