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

namespace OpenWeatherMap_Api_Service
{
    internal class ApiRequest : IApiRequest
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
        public async Task<WeatherApiResponse> GetWeatherByCoordinates(string lat, string lon)
        {
            fullApi = $"{apiUri}?lat={lat}&lon={lon}&units=metric&appid={apiKey}";

            try
            {
                using (var response = client.HttpClient.GetAsync(new Uri(fullApi)))
                {
                    if (response.Result.IsSuccessStatusCode)
                    {

                        var json = await response.Result.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<WeatherApiResponse>(json);


                        var some = res;

                        return res;
                    }
                    return null;

                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public async Task<WeatherApiResponse> GetWeatherByCity(string city)
        {
            fullApi = $"{apiUri}?q={city}&units=metric&appid={apiKey}";

            try
            {
                using (var response =  client.HttpClient.GetAsync(new Uri(fullApi)))
                {
                    if (response.Result.IsSuccessStatusCode)
                    {
                        
                        var json = await response.Result.Content.ReadAsStringAsync();
                        var res = JsonConvert.DeserializeObject<WeatherApiResponse>(json);
                    

                        var some = res;
                   
                        return res;
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
