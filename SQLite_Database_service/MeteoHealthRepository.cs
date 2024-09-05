using SQLite_Database_service.Interfaces;
using SQLite_Database_service.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SQLite_Database_service
{
    internal class MeteoHealthRepository : IMeteoHealthRepository
    {
        private readonly MeteoHealthDbContext _context;

        public MeteoHealthRepository(MeteoHealthDbContext dbContext)
        {
            _context = dbContext;
        }

        public Task<List<WeatherModel>> GetWeatherModelAsync(CancellationToken cancellationToken)
        {
            return _context.GetWeatherModelsAsync(cancellationToken);
        }

        public async Task<List<HealthStateModel>> GetHealthStatesAsync(CancellationToken cancellationToken)
        {
            return await _context.GetHealthStatesAsync(cancellationToken);
            
        }
        public Task<List<GeolocationModel>> GetGeolocationModelsAsync(CancellationToken cancellationToken)
        {
            return _context.GetGeolocationModelsAsync(cancellationToken);
        }
        public async Task<GeolocationModel> GetLastGeolocationModelAsync(CancellationToken cancellationToken)
        {
            return await _context.GetLastGeolocationModelAsync(cancellationToken);
        }

        public async Task<WeatherModel> GetLastWeatherModelAsync(CancellationToken cancellationToken)
        {
            return await _context.GetLastWeatherModelAsync(cancellationToken);
        }
        public async Task<HealthStateModel> GetLastHealthStateModelAsync(CancellationToken cancellationToken)
        {
            return await _context.GetLastHealthStateModelAsync(cancellationToken);
        }
        //
        public Task<int> SaveWeatherModelAsync(List<WeatherModel> model )
        {
            return _context.SaveWeatherModelAsync(model);
        }
        public Task<int> SaveHealtStateModel(HealthStateModel model )
        {

            return _context.SaveHealtStateModel(model);
        }

        public async Task<int> UpdateWeatherModelAsync(List<WeatherModel> model)
        {
            return await _context.UpdateWeatherModelAsync(model);
        }

        public async Task<int> UpsertWeatherModelAsync(List<WeatherModel> models)
        {
            return await _context.UpsertWeatherModelAsync(models);
        }

       
        public async Task<int> SaveGeolocationModelAsync(GeolocationModel model)
        {
            return await _context.SaveGeolocationModelAsync(model);
        }


        public async Task ClearDatabase(CancellationToken cancellationToken)
        {
            await _context.ClearWeatherModelAsync(cancellationToken);
            await _context.ClearHealthStateModelAsync(cancellationToken);
            await _context.ClearGeolocationModelAsync(cancellationToken);

        }

       
    }
}
