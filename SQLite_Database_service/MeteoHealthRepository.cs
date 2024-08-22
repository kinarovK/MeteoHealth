using SQLite_Database_service.Interfaces;
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

        public  Task<List<GeolocationModel>> GetGeolocationModelsAsync()
        {
            // _context.ClearGeolocationModelAsync();
            return  _context.GetGeolocationModelsAsync();
        }
        public async Task<int> SaveGeolocationModelAsync(GeolocationModel model)
        {
            return await _context.SaveGeolocationModelAsync(model);
        }

        public async Task<int> DeleteWeatherModelsAsync()
        {
            return await _context.ClearWeatherModelAsync();
        }
        public async Task<int> DeleteHealthStateModelsAsync()
        {
            return await _context.ClearHealthStateModelAsync();
        }
        public async Task<int> DeleteGeolocationAsync()
        {
            return await _context.ClearGeolocationModelAsync();
        }
    }
}
