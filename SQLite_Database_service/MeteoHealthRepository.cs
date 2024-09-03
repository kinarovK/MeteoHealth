using SQLite_Database_service.Interfaces;
using SQLite_Database_service.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
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

        public Task<List<WeatherModel>> GetWeatherModelAsync()
        {
            return _context.GetWeatherModelsAsync();
        }

        public async Task<List<HealthStateModel>> GetHealthStatesAsync()
        {
            return await _context.GetHealthStatesAsync();
            
        }
        public Task<List<GeolocationModel>> GetGeolocationModelsAsync()
        {
            return _context.GetGeolocationModelsAsync();
        }
        public async Task<GeolocationModel> GetLastGeolocationModelAsync()
        {
            return await _context.GetLastGeolocationModelAsync();
        }

        public async Task<WeatherModel> GetLastWeatherModelAsync()
        {
            return await _context.GetLastWeatherModelAsync();
        }
        public async Task<HealthStateModel> GetLastHealthStateModelAsync()
        {
            return await _context.GetLastHealthStateModelAsync();
        }
        //
        public Task<int> SaveWeatherModelAsync(List<WeatherModel> model)
        {
            return _context.SaveWeatherModelAsync(model);
        }
        public Task<int> SaveHealtStateModel(HealthStateModel model)
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


        public async Task ClearDatabase()
        {
            await _context.ClearWeatherModelAsync();
            await _context.ClearHealthStateModelAsync();
            await _context.ClearGeolocationModelAsync();

        }

       
    }
}
