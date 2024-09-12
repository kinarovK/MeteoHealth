using MeteoHealth.Services;
using MeteoHealth.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xamarin.Forms;
using System.Reflection;
using System.Linq;
using System.IO;
using OpenWeatherMap_Api_Service;
using OpenWeatherMap_Api_Service.Interfaces;
using SQLite_Database_service;
using SQLite_Database_service.Interfaces;
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
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var db = ServiceProvider.GetRequiredService<IMeteoHealthRepository>();
            var weatherApi = ServiceProvider.GetRequiredService<IOpenWeatherMapApiController>();
            var chart = ServiceProvider.GetRequiredService<IChartMaker>();
            var reportMaker = ServiceProvider.GetRequiredService<IReportMaker>();
            var apiService = ServiceProvider.GetRequiredService<IOpenWeatherMapConverter>();

            Application.Current.MainPage = new MainFlyoutPage(db, chart, reportMaker, weatherApi, apiService)
            {
                Title = "Menu"

            };

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
            services.AddScoped<IOpenWeatherMapConverter, OpenWeatherMapConverter>();
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
                .FirstOrDefault(r => r.EndsWith("secrets.json", StringComparison.OrdinalIgnoreCase));

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
            reminder.ScheduleDailyReminder(18, 00);
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
