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
using MeteoHealth.ViewModels;
using Report_Service;
using Report_Service.Interfaces;

namespace MeteoHealth
{
    public partial class App : Application
    {
        public IConfiguration Configuration { get; }
        public static ServiceProvider ServiceProvider { get; set; }
        public App()
        {
            InitializeComponent();
           
            var serviceCollection = new ServiceCollection();
         
            ConfigureServices(serviceCollection);
       
            var s = serviceCollection.BuildServiceProvider();
            ServiceProvider = serviceCollection.BuildServiceProvider();
            var start = s.GetRequiredService<IApiController>();

            

            var db = s.GetRequiredService<IMeteoHealthRepository>();
            //var result = start.ExecuteApiRequest("48.124", "22.15566");
            var chart = s.GetRequiredService<IChartMaker>();
            var reportMaker = s.GetRequiredService<IReportMaker>();
            //var result2 = result.Result;
            //db.UpsertWeatherModelAsync(ConvertToModel(result.Result));
                //db.SaveWeatherModelAsync(ConvertToModel(result.Result));
                //db.UpdateWeatherModelAsync(ConvertToModel(result.Result));
            //var lists = db.GetWeatherModelAsync().Result;

            //var f = lists.FirstOrDefault();
            //
            //DependencyService.Register<MockDataStore>();

            //MainPage = new MainPage();


            //MainPage = new NavigationPage(ServiceProvider.GetRequiredService<MainPage>());

            MainPage = new NavigationPage(new MainFlyoutPage(db, chart, reportMaker));
        }
        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = BuildConfiguration();
            services.Configure<ApplicationOptions>(configuration);
            services.AddSingleton<IConfiguration>(configuration);
            services.DependencyRegistrationForApi(configuration);
            services.DependencyRegistrationForDB();
            services.AddScoped<IChartMaker, ChartMaker>();
            services.AddScoped<IReminderService, ReminderService>();
            services.DependencyRegistrationForReport();
            services.AddTransient<MainPageViewModel>();
            services.AddTransient<MainPage>();
            services.AddTransient<GeolocationPage>();
            services.AddTransient<HealthStatePopupViewModel>();
            services.AddTransient<ReportPageViewModel>();
            services.AddTransient<ReportPage>();
            services.AddSingleton<INotificationService>(provider =>
                {
                return DependencyService.Get<INotificationService>();
                });
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
            var reminder = ServiceProvider.GetRequiredService<IReminderService>();
            reminder.ScheduleDailyReminder(13, 32);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
