using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqLiteDatabaseService.Interfaces
{
    public interface IMeteoHealthRepository
    {
        public Task<List<WeatherModel>> GetWeatherModelAsync();
        public Task<List<HealthStateModel>> GetHealthStatesAsync();
        public Task<int> SaveWeatherModelAsync(List<WeatherModel> model);
        public Task<int> SaveHealtStateModel(List<HealthStateModel> model);
 
    }
}
