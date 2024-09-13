using SQLite_Database_service.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SQLite_Database_service.Interfaces
{
    public interface IMeteoHealthRepository
    {
        public Task<List<WeatherModel>> GetWeatherModelAsync(CancellationToken cancellationToken);
        public Task<List<HealthStateModel>> GetHealthStatesAsync(CancellationToken cancellationToken);
        public Task<int> SaveWeatherModelAsync(List<WeatherModel> model);
        public Task<int> SaveHealtStateModel(HealthStateModel model);
        public Task<int> UpdateWeatherModelAsync(List<WeatherModel> model);
        public Task<int> UpsertWeatherModelAsync(List<WeatherModel> models );
        public Task<int> SaveGeolocationModelAsync(GeolocationModel model);
        public  Task<List<GeolocationModel>> GetGeolocationModelsAsync(CancellationToken cancellationToken);
        public  Task<GeolocationModel> GetLastGeolocationModelAsync(CancellationToken cancellationToken);

        public Task<WeatherModel> GetLastWeatherModelAsync(CancellationToken cancellationToken);
        public Task<HealthStateModel> GetLastHealthStateModelAsync(CancellationToken cancellationToken);


        public Task ClearDatabase(CancellationToken cancellationToken);
      

    }
}
