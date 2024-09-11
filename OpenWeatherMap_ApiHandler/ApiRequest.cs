using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenWeatherMap_ApiHandler_Service.Interfaces;
using OpenWeatherMap_ApiHandler_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
namespace OpenWeatherMap_ApiHandler_Service
{
    internal class ApiRequest : IApiRequest
    {
        private readonly IWeatherApiHttpClient client;
        private readonly string apiUri;
        public ApiRequest(IOptionsMonitor<ApplicationOptions> options, IHttpClientFactory clientFactory)
        {
            apiUri = options.CurrentValue.ApiUrl;
            this.client = new OpenWeatherMapHttpClient(clientFactory.CreateClient());
        }

        public async Task<WeatherApiResponse> GetWeatherByCity(string city)
        {
            try
            {
                using (var response = await client.HttpClient.GetAsync(new Uri(apiUri + city)))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<WeatherApiResponse>(json);
                    }
                    return null;
              
                }
            }
            catch (Exception ex)
            {
             
                return null;
            }
        }
    }
}
