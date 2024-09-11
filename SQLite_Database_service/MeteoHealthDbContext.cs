using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SQLite_Database_service.Models;

namespace SQLite_Database_service
{
    internal class MeteoHealthDbContext
    {
        private readonly SQLite.SQLiteAsyncConnection _connection;

        public MeteoHealthDbContext(string dbPath)
        {
            
            _connection = new SQLite.SQLiteAsyncConnection(dbPath);
            
            _connection.CreateTableAsync<WeatherModel>();
            _connection.CreateTableAsync<HealthStateModel>();
            _connection.CreateTableAsync<GeolocationModel>();
        }

        public Task<List<WeatherModel>> GetWeatherModelsAsync(CancellationToken cancellationToken)
        {
            return _connection.Table<WeatherModel>().ToListAsync();
        }

        public async Task<List<HealthStateModel>> GetHealthStatesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var healthStates = await _connection.Table<HealthStateModel>().ToListAsync();

            // Order in memory by converting string to DateTime
            return healthStates.OrderBy(x =>
            {
                return DateTime.Parse(x.Date);
            }).ToList();

        }
        public Task<int> SaveWeatherModelAsync(List<WeatherModel> model)
        {
            return _connection.InsertAllAsync(model);
        }
        public Task<int> SaveHealtStateModel(HealthStateModel model)
        {
            return _connection.InsertAsync(model);
        }
        public async Task<int> UpsertWeatherModelAsync(List<WeatherModel> models)
        {
            int rowsAffected = 0;
            foreach (var item in models)
            {
                var existingModel = await _connection.Table<WeatherModel>()
                                            .Where(w=> w.DateTime== item.DateTime).FirstOrDefaultAsync();

                if (existingModel != null)
                {
                    item.Id = existingModel.Id;
                    rowsAffected += await _connection.UpdateAsync(item);
                }
                else
                {
                    rowsAffected += await _connection.InsertAsync(item);
                }
            }
            return rowsAffected;
        }
        public Task<int> UpdateWeatherModelAsync(List<WeatherModel> model )
        {
            return _connection.UpdateAllAsync(model);
        }

        public Task<int> SaveGeolocationModelAsync(GeolocationModel model)
        {
            return _connection.InsertAsync(model);
        }

        public  Task<List<GeolocationModel>> GetGeolocationModelsAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return _connection.Table<GeolocationModel>().ToListAsync();
        }

        public async Task<GeolocationModel> GetLastGeolocationModelAsync(CancellationToken cancellationToken)
        {

            cancellationToken.ThrowIfCancellationRequested();
            return await _connection.Table<GeolocationModel>().OrderByDescending(i => i.Id).FirstOrDefaultAsync();
        }

        public async Task<HealthStateModel> GetLastHealthStateModelAsync(CancellationToken cancellationToken)
        {

            cancellationToken.ThrowIfCancellationRequested();
            //return await _connection.Table<HealthStateModel>().OrderByDescending(i => i.Date).FirstOrDefaultAsync();
            var healthStates = await _connection.Table<HealthStateModel>().ToListAsync();

            // Order in memory by converting string to DateTime
            return healthStates.OrderBy(x =>
            {
                return DateTime.Parse(x.Date);
            }).Last();
            //var a = await _connection.Table<HealthStateModel>().OrderByDescending(i => i.Date).FirstOrDefaultAsync();
            //return a;
        }
        public async Task<WeatherModel> GetLastWeatherModelAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return await _connection.Table<WeatherModel>().OrderByDescending(i => i.Id).FirstOrDefaultAsync();
        }

        public Task<int> ClearWeatherModelAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _connection.DeleteAllAsync<WeatherModel>();
        }

        public Task<int> ClearHealthStateModelAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _connection.DeleteAllAsync<HealthStateModel>();
        }

        public Task<int> ClearGeolocationModelAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return _connection.DeleteAllAsync<GeolocationModel>();
        }

    }
}
