using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenWeatherMap_Api_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace OpenWeatherMap_Api_Service
{
    public static class DependencyInjection
    {
        public static void DependencyRegistrationForApi(this IServiceCollection services, IConfiguration config)
        {

            services.AddHttpClient();

            services.Configure<ApplicationOptions>(config.GetSection(ApplicationOptions.AppSettings));
            //var apiKey = config["AppSettings:apiKey"];
            //var apiUrlBase = config["AppSettings:apiUrlBase"];
            services.AddSingleton<IApiController, ApiController>();
            services.AddSingleton<IApiRequest, ApiRequest>();
            services.AddSingleton<IWeatherApiHttpClient, OpenWeatherMapHttpClient>();
        }
    }
}
