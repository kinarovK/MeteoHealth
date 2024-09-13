using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenWeatherMap_Api_Service.Interfaces;

namespace OpenWeatherMap_Api_Service
{
    public static class DependencyInjection
    {
        public static void DependencyRegistrationForApi(this IServiceCollection services, IConfiguration config)
        {

            services.AddHttpClient();

            services.Configure<ApplicationOptions>(config.GetSection(ApplicationOptions.AppSettings));
            services.AddSingleton<IOpenWeatherMapApiController, OpenWeatherMapApiController>();
            services.AddSingleton<IOpenWeatherMapApiRequest, OpenWeatherMapApiRequest>();
            services.AddSingleton<IWeatherApiHttpClient, OpenWeatherMapHttpClient>();
        }
    }
}
