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
using System.Buffers;

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
            var start = s.GetRequiredService<IOpenWeatherMapApiController>();

            

            var db = s.GetRequiredService<IMeteoHealthRepository>();
            var weatherApi = s.GetRequiredService<IOpenWeatherMapApiController>();
    
            var chart = s.GetRequiredService<IChartMaker>();
            var reportMaker = s.GetRequiredService<IReportMaker>();
            var apiService = s.GetRequiredService<IWeatherApiService>();
    
            //db.DeleteGeolocationAsync();
            //db.DeleteHealthStateModelsAsync();
            //db.DeleteWeatherModelsAsync();
          
            Device.BeginInvokeOnMainThread(() =>
            {
                //var flyoutpage = new MainFlyoutPage(db, chart, reportMaker, start, apiService)
                //{
                //    Title = "Menu"
                //};
                //Application.Current.MainPage = new FlyoutPage
                //{
                //    Flyout = flyoutpage,
                //    Detail = new NavigationPage(new MainPage(db, chart, start, apiService))
                //};
                var flyout = new MainFlyoutPage(db, chart, reportMaker, start, apiService);
                MainPage = flyout;
                //Application.Current.MainPage = new MainFlyoutPage(db, chart, reportMaker, weatherApi, apiService)
                //{
                //    Title = "Menu"
                //};
                //Application.Current.MainPage = new NavigationPage(new MainFlyoutPage(db, chart, reportMaker, weatherApi, apiService));

            });

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
            services.AddScoped<IWeatherApiService, WeatherApiService>();
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
            var weatherApi = ServiceProvider.GetRequiredService<IOpenWeatherMapApiController>();
            //weatherApi.ExecuteApiRequest();
            var reminder = ServiceProvider.GetRequiredService<IReminderService>();
            //reminder.ScheduleDailyReminder(13, 32);

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    var db = ServiceProvider.GetRequiredService<IMeteoHealthRepository>();
            //    var chart = ServiceProvider.GetRequiredService<IChartMaker>();
            //    var reportMaker = ServiceProvider.GetRequiredService<IReportMaker>();
            //    var apiService = ServiceProvider.GetRequiredService<IWeatherApiService>();
            //    //Application.Current.MainPage = new NavigationPage(new MainFlyoutPage(db, chart, reportMaker, weatherApi, apiService));
            //    var flyoutPage = new MainFlyoutPage(db, chart, reportMaker, weatherApi, apiService)
            //    {
            //        Title = "Menu"
            //    };
            //    Application.Current.MainPage = new FlyoutPage
            //    {
            //        Flyout = flyoutPage,
            //        Detail = new NavigationPage(new MainPage(db, chart, weatherApi, apiService))
            //    };
            //});
        
    }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
