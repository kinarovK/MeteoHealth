using SqLiteDatabaseService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqLiteDatabaseService
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
        
        public Task<List<HealthStateModel>> GetHealthStatesAsync()
        {
            return _context.GetHealthStatesAsync();
        }
        public Task<int> SaveWeatherModelAsync(List<WeatherModel> model)
        {

            return _context.SaveWeatherModelAsync(model);
        }
        public Task<int> SaveHealtStateModel(List<HealthStateModel> model)
        {

            return _context.SaveHealtStateModel(model);
        }
    }   
}
