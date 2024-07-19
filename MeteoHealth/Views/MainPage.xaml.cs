using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SQLite_Database_service.Interfaces;
using SQLite_Database_service;
using Xamarin.CommunityToolkit.Extensions;
using OpenWeatherMap_Api_Service.Models;
//using OxyPlot.Xamarin.Forms;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using MeteoHealth.Models;
using MeteoHealth.Services;
using OxyPlot.Xamarin.Forms;
using GeoLocation_Service;
using Xamarin.Essentials;
using Microsoft.Extensions.DependencyInjection;
using MeteoHealth.ViewModels;

namespace MeteoHealth.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private readonly IMeteoHealthRepository repo;

        //private PlotModel temperaturePlotmodel;
        //private PlotModel healthForTemperaturePlotModel;

        //private PlotModel pressurePlotModel;
        //private PlotModel healthForPressurePlotModel;

        //private PlotModel humidityPlotModel;
        //private PlotModel healthForHumidityPlotModel;

        //private bool isTempHealthSync;
        //private bool isPressHealthSync;
        //private bool isHumHealthSync;
        //private readonly IMeteoHealthRepository meteoHealthRepository;

        //public MainPage() :this(App.ServiceProvider.GetRequiredService<IMeteoHealthRepository>())
        //{

        //}
        private MainPageViewModel viewModel;
        //public MainPage(MainPageViewModel mainPageViewModel)
        //{
        //    InitializeComponent();
        //    viewModel = mainPageViewModel;
        //    BindingContext = viewModel;
        //}
        
        public MainPage(IMeteoHealthRepository repo, IChartMaker chartMaker)
        {
            InitializeComponent();

            var oxyThicknessForWeatherCharts = new OxyThickness(40, 0, 10, 0);
            var oxyThicknessForHealth = new OxyThickness(41, 10, 10, 80);
            var defaultOxyThickness = new OxyThickness(0);
            var viewModel = new MainPageViewModel(repo, chartMaker);
            BindingContext = new MainPageViewModel(repo, chartMaker);



            //viewModel.TemperaturePlotView = plotModel;
            TemperaturePlotView.Model = viewModel.TemperaturePlotModel;
            TemperaturePlotView.Model.PlotMargins = oxyThicknessForWeatherCharts;
            TemperaturePlotView.Model.Padding = defaultOxyThickness;

            HealthTemperaturePlotView.Model = viewModel.HealthTemperaturePlotModel;
            HealthTemperaturePlotView.Model.PlotMargins = oxyThicknessForHealth;
            HealthTemperaturePlotView.Model.Padding = defaultOxyThickness;


            //
            PressurePlotView.Model = viewModel.PressurePlotModel;
            PressurePlotView.Model.PlotMargins = oxyThicknessForWeatherCharts;
            PressurePlotView.Model.Padding = defaultOxyThickness;

            PressureHealthPlotView.Model = viewModel.HealthPressurePlotModel;
            PressureHealthPlotView.Model.PlotMargins = oxyThicknessForHealth;
            PressureHealthPlotView.Model.Padding = defaultOxyThickness;

            //
            HumidityPlotView.Model = viewModel.HumidityPlotModel;
            HumidityPlotView.Model.PlotMargins = oxyThicknessForWeatherCharts;
            HumidityPlotView.Model.Padding = defaultOxyThickness;

            HumidityHealthPlotView.Model = viewModel.HealthHumidityPlotModel;
            HumidityHealthPlotView.Model.PlotMargins = oxyThicknessForHealth;
            HumidityHealthPlotView.Model.Padding = defaultOxyThickness;

            //
            WindPlotView.Model = viewModel.WindPlotModel;
            WindPlotView.Model.PlotMargins = oxyThicknessForWeatherCharts;
            WindPlotView.Model.Padding = defaultOxyThickness;

            WindHealthPlotView.Model = viewModel.HealthWindPlotModel;
            WindHealthPlotView.Model.PlotMargins = oxyThicknessForHealth;
            WindHealthPlotView.Model.Padding = defaultOxyThickness;
            //
            PrecipitationProbabilityPlotView.Model = viewModel.PrecipitationProbPlotModel;
            PrecipitationProbabilityPlotView.Model.PlotMargins = oxyThicknessForWeatherCharts;
            PrecipitationProbabilityPlotView.Model.Padding = defaultOxyThickness;

            PrecipitationProbabilityHealthPlotView.Model = viewModel.HealthPrecipitationProbPlotModel;
            PrecipitationProbabilityHealthPlotView.Model.PlotMargins = oxyThicknessForHealth;
            PrecipitationProbabilityHealthPlotView.Model.Padding = defaultOxyThickness;

            //

            PrecipitationVolumePlotView.Model = viewModel.PrecipitationVolPlotModel;
            PrecipitationVolumePlotView.Model.PlotMargins = oxyThicknessForWeatherCharts;
            PrecipitationVolumePlotView.Model.Padding = defaultOxyThickness;

            PrecipitationVolumeHealthPlotView.Model = viewModel.HealthPrecipitationVolPlotModel;
            PrecipitationVolumeHealthPlotView.Model.PlotMargins = oxyThicknessForHealth;
            PrecipitationVolumeHealthPlotView.Model.Padding = defaultOxyThickness;
            this.repo = repo;
            //plotview.Model.PlotMargins = new OxyThickness(41, 10, 10, 80);
            //plotview.Model.Padding = new OxyThickness(0);
            //plotview.Model = plotModel;
            //plotview.Model.PlotMargins = 
            //plotview.Model.Padding = new OxyThickness(0);
            //plotview.Model.Padding = new OxyThickness(0);
            //var TemperaturePlotView = new PlotView();
        }

       
        //public MainPage(IMeteoHealthRepository meteoHealthRepository)
        //{
        //    InitializeComponent();

        //    var weatherData = meteoHealthRepository.GetWeatherModelAsync();
        //    //var chartEntries = GetChartEntries(weatherData.Result);



        //    var res = meteoHealthRepository.GetWeatherModelAsync().Result;
        //    //CreateChart(weatherData.Result);
        //    var weather = CreateMockWeatherModels();
        //    var healthState = CreateHealthModelMock();
        //    //var weather = meteoHealthRepository.GetWeatherModelAsync().Result;
        //    //var healthState = meteoHealthRepository.GetHealthStatesAsync().Result;

        //    //CreateTempHealthChart(weather, healthState);
        //    //CreatePressureHealthChart(weather, healthState);

        //    var chartmaker = new ChartMaker();
        //    var tempPlotModel = chartmaker.CreateWeatherChart(weather, healthState, TemperaturePlotView, "Temperature", "Temperature");
        //    var healthforTempPlotModel = chartmaker.CreateHealthChar(healthState, TemperatureHealthPlotView, "HealtState", "temp");
        //    chartmaker.InitializeCharts(tempPlotModel, healthforTempPlotModel);


        //    var healthPress = chartmaker.CreateHealthChar(healthState, PressureHealthPlotView, "HealtState", "temp");
        //    var pressure = chartmaker.CreateWeatherChart(weather, healthState, PressurePlotView, "Pressure", "Pressure");
        //    chartmaker.InitializeCharts(healthforTempPlotModel, healthPress);

        //    var humidityPlotModel = chartmaker.CreateWeatherChart(weather, healthState, HumidityPlotView, "Humidity", "Humidity");
        //    var healthHumidity = chartmaker.CreateHealthChar(healthState, HumidityHealthPlotView, "HealtState", "temp");
        //    chartmaker.InitializeCharts(humidityPlotModel, healthHumidity);

        //    var windHealth = chartmaker.CreateWeatherChart(weather, healthState, WindPlotView, "Wind", "Wind");
        //    var healthWind = chartmaker.CreateHealthChar(healthState, WindHealthPlotView, "HealtState", "temp");
        //    chartmaker.InitializeCharts(windHealth, healthWind);

        //    var precipitationProbabilityPlotView = chartmaker.CreateWeatherChart(weather, healthState, PrecipitationProbabilityPlotView, "PrecipitationProbability", "PrecipitationProbability");
        //    var precipitationProbabilityHealthPlotView = chartmaker.CreateHealthChar(healthState, PrecipitationProbabilityHealthPlotView, "HealtState", "temp");
        //    chartmaker.InitializeCharts(precipitationProbabilityPlotView, precipitationProbabilityHealthPlotView);

        //    var precipitationVolumePlotModel = chartmaker.CreateWeatherChart(weather, healthState, PrecipitationVolumePlotView, "PrecipitationVolume", "PrecipitationVolume");
        //    var precipitationVolumeHealthPlotModel = chartmaker.CreateHealthChar(healthState, PrecipitationVolumeHealthPlotView, "HealtState", "temp");
        //    chartmaker.InitializeCharts(precipitationVolumePlotModel, precipitationVolumeHealthPlotModel);
        //    //CreateHealthChar(healthState);
        //    //CreateHumidityHealthChart(weather, healthState);
        //    this.meteoHealthRepository = meteoHealthRepository;

        //    var healt = meteoHealthRepository.GetHealthStatesAsync().Result;

        //    var somehealt = healt;
        //    //CreateChart2(CreateMockWeatherModels());
        //    //CreateSingleChart(weatherData.Result);


        //}//
        //protected async override void OnAppearing()
        //{
        //    //not to good approach bc the method calls every time even user just set location 
        //    base.OnAppearing();

        //    var todayHealthState = meteoHealthRepository.GetHealthStatesAsync();

        //    var today = todayHealthState.Result.Where(x=>x.Date == DateTime.Today.ToString()).FirstOrDefault();
        //    var geolocation = meteoHealthRepository.GetGeolocationModelsAsync().Result;
        //    if (today is null)
        //    {
        //        await Navigation.ShowPopupAsync(new HealthStatePopup(meteoHealthRepository));
        //    }
        //    if (geolocation.Count == 0) 
        //    {
        //        await DisplayAlert("Ooops", "Not found geolocation, please set it", "OK");
        //        await Navigation.PushAsync(new LoginPage(meteoHealthRepository));
        //    }



        //}
        //private List<WeatherModel> CreateMockWeatherModels()
        //{

        //    var rand = new Random();
        //    var anotherRand = new Random();
        //    DateTime dateTime1 = new DateTime(2024, 06, 1, 00, 00, 00);

        //    var weatherData = new List<WeatherModel>
        //    {
        //    };
        //    for (int i = 0; i < 100; i++)
        //    {
        //        weatherData.Add(new WeatherModel
        //        {
        //            DateTime = dateTime1.ToString(),
        //            Temperature = rand.Next(15, 35),
        //            Pressure = anotherRand.Next(980, 1080),
        //            Humidity = rand.Next(30, 100)
        //        });
        //        dateTime1 = dateTime1.AddHours(3);
        //    }
        //    return weatherData;
        //}
        //private List<HealthStateModel> CreateHealthModelMock()
        //{
        //    Random rand = new Random();
        //    var datestr = "2024-06-01";
        //    DateTime dateTime = DateTime.Parse(datestr);
        //    var healthState = new List<HealthStateModel>
        //    {

        //    };

        //    for (int i = 0; i < 10; i++)
        //    {
        //        healthState.Add(new HealthStateModel { Date = dateTime.ToString(), HealthLevel = (byte)rand.Next(1, 5) });
        //        dateTime = dateTime.AddDays(1);
        //    }
        //    var datetime2 = dateTime;
        //    return healthState;

        //}
        //private async void Button_Clicked(object sender, EventArgs e)
        //{
        //    var result = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Lowest, TimeSpan.FromMinutes(1)));
        //}

        //private void Button_Clicked_1(object sender, EventArgs e)
        //{
        //    Navigation.PushAsync(new LoginPage(meteoHealthRepository));
        //}
    }
}