using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_ApiHandler_Service.Interfaces
{
    public interface IWeatherApiHttpClient
    {
        HttpClient HttpClient { get; }
    }
}
