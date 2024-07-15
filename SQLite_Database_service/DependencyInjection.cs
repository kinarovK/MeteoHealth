using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SQLite_Database_service.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SQLite_Database_service
{
    public static class DependencyInjection
    {
        public static void DependencyRegistrationForDB(this IServiceCollection services)
        {

            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MeteoHealth.db3");
            services.AddSingleton<MeteoHealthDbContext>(provider => new MeteoHealthDbContext(dbPath));
            services.AddScoped<IMeteoHealthRepository, MeteoHealthRepository>();
        }
    }
}
