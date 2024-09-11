using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenWeatherMap_ApiHandler_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_ApiHandler_Service
{
    public static class DependencyInjection
    {
        public static void DependencyRegistrationForApi(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IApiController, ApiController>();
            services.AddSingleton<IApiRequest, ApiRequest>();
        }
    }
}
