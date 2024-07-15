using SQLite_Database_service;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLite_Database_service.Interfaces
{
    public interface IMeteoHealthRepository
    {
        public Task<List<WeatherModel>> GetWeatherModelAsync();
        public Task<List<HealthStateModel>> GetHealthStatesAsync();
        public Task<int> SaveWeatherModelAsync(List<WeatherModel> model);
        public Task<int> SaveHealtStateModel(HealthStateModel model);
        public Task<int> UpdateWeatherModelAsync(List<WeatherModel> model);
        public Task<int> UpsertWeatherModelAsync(List<WeatherModel> models);
        public Task<int> SaveGeolocationModelAsync(GeolocationModel model);

        public  Task<List<GeolocationModel>> GetGeolocationModelsAsync();



    }
}
