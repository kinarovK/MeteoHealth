using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherMap_Api_Service.Interfaces
{
    public interface IWeatherApiHttpClient
    {
        HttpClient HttpClient { get; }
    }
}
