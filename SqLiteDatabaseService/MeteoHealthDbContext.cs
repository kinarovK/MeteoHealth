using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqLiteDatabaseService
{
    internal class MeteoHealthDbContext 
    {
        private readonly SQLite.SQLiteAsyncConnection _connection;

        public MeteoHealthDbContext(string dbPath)
        {
            _connection = new SQLite.SQLiteAsyncConnection(dbPath);
            _connection.CreateTableAsync<WeatherModel>();
            _connection.CreateTableAsync<HealthStateModel>();
        }

        public Task<List<WeatherModel>> GetWeatherModelsAsync()
        {
            return _connection.Table<WeatherModel>().ToListAsync();
        }

        public Task<List<HealthStateModel>> GetHealthStatesAsync()
        {
            return _connection.Table<HealthStateModel>().ToListAsync();
        }
        public Task<int> SaveWeatherModelAsync(List<WeatherModel> model)
        {
            return _connection.InsertAllAsync(model);
             
        }
        public Task<int> SaveHealtStateModel(List<HealthStateModel> model)
        {
            return _connection.InsertAllAsync(model);     
           
        }

        //Delete records 

        //update records 
    }
}
