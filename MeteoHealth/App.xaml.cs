using MeteoHealth.Services;
using MeteoHealth.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.Extensions.Configuration.Json;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using OpenWeatherMap_Api_Service;
using OpenWeatherMap_Api_Service.Interfaces;
using SQLite_Database_service;
using SQLite_Database_service.Interfaces;
using OpenWeatherMap_Api_Service.Models;
using System.Collections.Generic;

namespace MeteoHealth
{
    public partial class App : Application
    {
        public IConfiguration Configuration { get; }
        public static ServiceProvider ServiceProvider { get; set; }
        public App()
        {
            InitializeComponent();
            //GetSettings
            //var assembly = Assembly.GetExecutingAssembly();
            //var resName = assembly.GetManifestResourceNames()?.FirstOrDefault(r => r.EndsWith("settings.json", StringComparison.OrdinalIgnoreCase));
            //var file = assembly.GetManifestResourceStream(resName);
            ////dispose
            //var sr = new StreamReader(file);
            //var json = sr.ReadToEnd();
            //var j = JObject.Parse(json);
            //var apiUrlBase = j.Value<string>("apiUrlBase");
            //var apiKey = j.Value<string>("apiKey");

            //var som = apiUrlBase + apiKey;
           
           
            var serviceCollection = new ServiceCollection();
            var configuration = BuildConfiguration();
            serviceCollection.Configure<ApplicationOptions>(configuration);

            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.DependencyRegistrationForApi(configuration);
            serviceCollection.DependencyRegistrationForDB();

            serviceCollection.AddTransient<MainPage>();
            serviceCollection.AddTransient<LoginPage>();
            //serviceCollection.AddTransient<MeteoHealthFlyoutFlyoutViewModel>();

            var s = serviceCollection.BuildServiceProvider();
            ServiceProvider = serviceCollection.BuildServiceProvider();
            var start = s.GetRequiredService<IApiController>();

            

            var db = s.GetRequiredService<IMeteoHealthRepository>();
            var result = start.ExecuteApiRequest("48.124", "22.15566");

            var result2 = result.Result;
            db.UpsertWeatherModelAsync(ConvertToModel(result.Result));
            //db.SaveWeatherModelAsync(ConvertToModel(result.Result));
            //db.UpdateWeatherModelAsync(ConvertToModel(result.Result));
            var lists = db.GetWeatherModelAsync().Result;

            var f = lists.FirstOrDefault();
            //
            DependencyService.Register<MockDataStore>();

            //MainPage = new MainPage();


            MainPage = new NavigationPage(new MainPage(db));


        }

        private List<WeatherModel> ConvertToModel(WeatherApiResponse response)
        {
            var result = new List<WeatherModel>();

            foreach (var item in response.list)
            {
                result.Add(new WeatherModel
                {
                    DateTime = item.dt_txt,
                    Humidity = item.main.humidity,
                    PrecipitationProbability = (int)item.pop *100,
                    PrecipitationVolume = (item.rain?.ThreeHours ?? 0) + (item.snow?.ThreeHours ?? 0),
                    Pressure = item.main.pressure,
                    Temperature = item.main.temp,
                    WindSpeed = item.wind.speed
                });
            }
            return result;
        }
        private IConfiguration BuildConfiguration()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = assembly.GetManifestResourceNames()
                .FirstOrDefault(r => r.EndsWith("appsettings.json", StringComparison.OrdinalIgnoreCase));

            if (resourceName == null)
            {
                throw new FileNotFoundException("appsettings.json not found as an embedded resource.");
            }

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();

                return configuration;
            }
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
