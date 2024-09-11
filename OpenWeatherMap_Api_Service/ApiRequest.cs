using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Text.Json;
using System.Threading.Tasks;
using System.Net.Http;
using OpenWeatherMap_Api_Service.Interfaces;
using OpenWeatherMap_Api_Service.Models;
using System.Threading;
using System.Net.Sockets;

namespace OpenWeatherMap_Api_Service
{
    internal class ApiRequest : IOpenWeatherMapApiRequest
    {
        private readonly IWeatherApiHttpClient client;
        private readonly string apiUri;
        private readonly string apiKey;
        private  string fullApi;
        public ApiRequest(IOptionsMonitor<ApplicationOptions> options, IHttpClientFactory clientFactory)
        {
            apiUri = options.CurrentValue.ApiUrlBase;
            apiKey = options.CurrentValue.ApiKey;
            client = new OpenWeatherMapHttpClient(clientFactory.CreateClient());
            //https://api.openweathermap.org/data/2.5/forecast?q=Berehove&units=metric&appid=967d6d313cb6f392bc0bcbed0f868597

           
        }
        public async Task<WeatherApiResponse> GetWeatherByCoordinates(string lat, string lon, CancellationToken token)
        {
            fullApi = $"{apiUri}?lat={lat}&lon={lon}&units=metric&appid={apiKey}";
            token.ThrowIfCancellationRequested();
          
            //try
            //{
                using (var response = await client.HttpClient.GetAsync(new Uri(fullApi), token))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var json = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<WeatherApiResponse>(json);
                        
                    }
                    return null;

                }
            //}
            ////TaskCancelledException
            ////Handle exception or make some logger service
            //catch (Exception ex)
            //{

            //    return null;
            //}
        }
  
    }
}
