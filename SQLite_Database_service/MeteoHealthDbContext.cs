using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
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

        public Task<List<WeatherModel>> GetWeatherModelsAsync()
        {
            return _connection.Table<WeatherModel>().ToListAsync();
        }

        public async Task<List<HealthStateModel>> GetHealthStatesAsync()
        {
            return await _connection.Table<HealthStateModel>().ToListAsync();
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
                var existingModel =  _connection.Table<WeatherModel>()
                                            .Where(w=> w.DateTime== item.DateTime).FirstOrDefaultAsync().Result;

                if (existingModel != null)
                {
                    item.Id = existingModel.Id;
                    rowsAffected +=  _connection.UpdateAsync(item).Result;
                }
                else
                {
                    rowsAffected +=  _connection.InsertAsync(item).Result;
                }
            }
            return rowsAffected;
        }
        public Task<int> UpdateWeatherModelAsync(List<WeatherModel> model)
        {
            return _connection.UpdateAllAsync(model);
        }

        public Task<int> SaveGeolocationModelAsync(GeolocationModel model)
        {
            return _connection.InsertAsync(model);
        }

        public  Task<List<GeolocationModel>> GetGeolocationModelsAsync()
        {
            return  _connection.Table<GeolocationModel>().ToListAsync();
        }

        public async Task<GeolocationModel> GetLastGeolocationModelAsync()
        {
            return await _connection.Table<GeolocationModel>().OrderByDescending(i => i.Id).FirstOrDefaultAsync();
        }

        public async Task<HealthStateModel> GetLastHealthStateModelAsync()
        {
            return await _connection.Table<HealthStateModel>().OrderByDescending(i => i.Id).FirstOrDefaultAsync();
        }
        public async Task<WeatherModel> GetLastWeatherModelAsync()
        {
            return await _connection.Table<WeatherModel>().OrderByDescending(i => i.Id).FirstOrDefaultAsync();
        }

        public Task<int> ClearWeatherModelAsync()
        {
            return _connection.DeleteAllAsync<WeatherModel>();
        }

        public Task<int> ClearHealthStateModelAsync()
        {
            return _connection.DeleteAllAsync<HealthStateModel>();
        }

        public Task<int> ClearGeolocationModelAsync()
        {
            return _connection.DeleteAllAsync<GeolocationModel>();
        }

    }
}
