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

namespace MeteoHealth.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        private PlotModel temperaturePlotmodel;
        private PlotModel healthForTemperaturePlotModel;

        private PlotModel pressurePlotModel;
        private PlotModel healthForPressurePlotModel;

        private PlotModel humidityPlotModel;
        private PlotModel healthForHumidityPlotModel;

        private bool isTempHealthSync;
        private bool isPressHealthSync;
        private bool isHumHealthSync;
        private readonly IMeteoHealthRepository meteoHealthRepository;

        public MainPage() :this(App.ServiceProvider.GetRequiredService<IMeteoHealthRepository>())
        {
            
        }
        public MainPage(IMeteoHealthRepository meteoHealthRepository)
        {
            InitializeComponent();

            var weatherData = meteoHealthRepository.GetWeatherModelAsync();
            //var chartEntries = GetChartEntries(weatherData.Result);



            var res = meteoHealthRepository.GetWeatherModelAsync().Result;
            //CreateChart(weatherData.Result);
            var weather = CreateMockWeatherModels();
            var healthState = CreateHealthModelMock();
            //var weather = meteoHealthRepository.GetWeatherModelAsync().Result;
            //var healthState = meteoHealthRepository.GetHealthStatesAsync().Result;

            //CreateTempHealthChart(weather, healthState);
            //CreatePressureHealthChart(weather, healthState);
            
            var chartmaker = new ChartMaker();
            var tempPlotModel = chartmaker.CreateWeatherChart(weather, healthState, TemperaturePlotView, "Temperature", "Temperature");
            var healthforTempPlotModel = chartmaker.CreateHealthChar(healthState, TemperatureHealthPlotView, "HealtState", "temp");
            chartmaker.InitializeCharts(tempPlotModel, healthforTempPlotModel);
            

            var healthPress = chartmaker.CreateHealthChar(healthState, PressureHealthPlotView, "HealtState", "temp");
            var pressure = chartmaker.CreateWeatherChart(weather, healthState, PressurePlotView, "Pressure", "Pressure");
            chartmaker.InitializeCharts(healthforTempPlotModel, healthPress);

            var humidityPlotModel = chartmaker.CreateWeatherChart(weather, healthState, HumidityPlotView, "Humidity", "Humidity");
            var healthHumidity = chartmaker.CreateHealthChar(healthState, HumidityHealthPlotView, "HealtState", "temp");
            chartmaker.InitializeCharts(humidityPlotModel, healthHumidity);

            var windHealth = chartmaker.CreateWeatherChart(weather, healthState, WindPlotView, "Wind", "Wind");
            var healthWind = chartmaker.CreateHealthChar(healthState, WindHealthPlotView, "HealtState", "temp");
            chartmaker.InitializeCharts(windHealth, healthWind);

            var precipitationProbabilityPlotView = chartmaker.CreateWeatherChart(weather, healthState, PrecipitationProbabilityPlotView, "PrecipitationProbability", "PrecipitationProbability");
            var precipitationProbabilityHealthPlotView = chartmaker.CreateHealthChar(healthState, PrecipitationProbabilityHealthPlotView, "HealtState", "temp");
            chartmaker.InitializeCharts(precipitationProbabilityPlotView, precipitationProbabilityHealthPlotView);

            var precipitationVolumePlotModel = chartmaker.CreateWeatherChart(weather, healthState, PrecipitationVolumePlotView, "PrecipitationVolume", "PrecipitationVolume");
            var precipitationVolumeHealthPlotModel = chartmaker.CreateHealthChar(healthState, PrecipitationVolumeHealthPlotView, "HealtState", "temp");
            chartmaker.InitializeCharts(precipitationVolumePlotModel, precipitationVolumeHealthPlotModel);
            //CreateHealthChar(healthState);
            //CreateHumidityHealthChart(weather, healthState);
            this.meteoHealthRepository = meteoHealthRepository;
             
            var healt = meteoHealthRepository.GetHealthStatesAsync().Result;

            var somehealt = healt;
            //CreateChart2(CreateMockWeatherModels());
            //CreateSingleChart(weatherData.Result);


        }//
        
       
        protected async override void OnAppearing()
        {
            //not to good approach bc the method calls every time even user just set location 
            base.OnAppearing();

            var todayHealthState = meteoHealthRepository.GetHealthStatesAsync();

            var today = todayHealthState.Result.Where(x=>x.Date == DateTime.Today.ToString()).FirstOrDefault();
            var geolocation = meteoHealthRepository.GetGeolocationModelsAsync().Result;
            if (today is null)
            {
                await Navigation.ShowPopupAsync(new HealthStatePopup(meteoHealthRepository));
            }
            if (geolocation.Count == 0) 
            {
                await DisplayAlert("Ooops", "Not found geolocation, please set it", "OK");
                await Navigation.PushAsync(new LoginPage(meteoHealthRepository));
            }

          

        }
        private List<WeatherModel> CreateMockWeatherModels()
        {

            var rand = new Random();
            var anotherRand = new Random();
            DateTime dateTime1 = new DateTime(2024, 06, 1, 00, 00, 00);

            var weatherData = new List<WeatherModel>
            {
            };
            for (int i = 0; i < 100; i++)
            {
                weatherData.Add(new WeatherModel
                {
                    DateTime = dateTime1.ToString(),
                    Temperature = rand.Next(15, 35),
                    Pressure = anotherRand.Next(980, 1080),
                    Humidity = rand.Next(30,100)
                });
                  dateTime1 = dateTime1.AddHours(3);
            }
           return weatherData;
        }

        private List<HealthStateModel> CreateHealthModelMock()
        {
            Random rand = new Random();
            var datestr = "2024-06-01";
            DateTime dateTime = DateTime.Parse(datestr);
            var healthState = new List<HealthStateModel>
            {

            };

            for (int i = 0; i < 10; i++)
            {
                healthState.Add(new HealthStateModel { Date = dateTime.ToString(), HealthLevel = (byte)rand.Next(1, 5) });
                dateTime = dateTime.AddDays(1);
            }
            var datetime2 = dateTime;
            return healthState;

        }
       

        private void CreateTempHealthChart(List<WeatherModel> weatherData, List<HealthStateModel> healthData)
        {

            temperaturePlotmodel = new PlotModel { Title = "Temperature" };

            var temperatureSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Red,
                LineStyle = LineStyle.Solid
            };
            var parsedHealthData = DateTime.TryParse(healthData.LastOrDefault().Date, out DateTime parsedLastHealthdata);
            // Populate temperature data points
            foreach (var item in weatherData)
            {
                if (DateTime.TryParse(item.DateTime, out DateTime parsedDateTime))
                {
                    if (parsedDateTime <= parsedLastHealthdata)
                    temperatureSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.Temperature));
                }
            }
            var newCol = weatherData.Where(x=> DateTime.Parse(x.DateTime) <= parsedLastHealthdata).ToList();

              
            
            // Create the DateTime axis for the X-axis
            var temperatureDateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd", // Adjust the format as needed

                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,

                Angle = 90,
                IsPanEnabled = true, // Enable panning
                //IsZoomEnabled = false, // Enable zooming
                // Set the initial window to the last 10 records
                Minimum = DateTimeAxis.ToDouble(DateTime.Parse(weatherData[newCol.Count - 33].DateTime)),
                Maximum = DateTimeAxis.ToDouble(DateTime.Parse(weatherData[newCol.Count - 1].DateTime)),
                AbsoluteMaximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.Last().Date)),
                AbsoluteMinimum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.First().Date)),

                LabelFormatter = x => string.Empty
            };
            
            temperatureDateTimeAxis.AxisChanged += OnTemperatureAxisChanged;
            
            temperaturePlotmodel.Axes.Add(temperatureDateTimeAxis);

            // Create the Linear axis for the Y-axis (temperature)
            var temperatureAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Temperature (°C)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IsPanEnabled = true, // Enable panning
                //IsZoomEnabled = true // Enable zooming
            };

            temperaturePlotmodel.Axes.Add(temperatureAxis);

            // Add the series to the plot model
            temperaturePlotmodel.Series.Add(temperatureSeries);

            // Set the model to plot view
            TemperaturePlotView.Model = temperaturePlotmodel;
            TemperaturePlotView.Model.PlotMargins = new OxyThickness(40, 0, 10, 0);
            TemperaturePlotView.Model.Padding = new OxyThickness(0);
            //the second plotmodel
            healthForTemperaturePlotModel = new PlotModel { };
            var healthForTemperatureSeries = new LineSeries
            {
                //Title = "Humidity",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Yellow,
                LineStyle = LineStyle.Solid,
                Smooth = true,
                Color = OxyColors.Red,

            };
            foreach (var item in healthData)
            {
                if (DateTime.TryParse(item.Date, out DateTime parsedDateTime))
                {
                    healthForTemperatureSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.HealthLevel));
                }
            }

            var healthForTemperatureDateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd",
                //Title = "Date"
                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Angle = 90,
                IsPanEnabled = true,
                //IsZoomEnabled = false,
                Minimum = DateTimeAxis.ToDouble(DateTime.Parse(healthData[healthData.Count - 5].Date)),
                Maximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData[healthData.Count - 1].Date)),
                AbsoluteMaximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.Last().Date)),
                AbsoluteMinimum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.First().Date)),

            };
            healthForTemperatureDateTimeAxis.AxisChanged += OnTemperatureHealthAxisChanged;

            //Dynamically refres

            var healthFortempAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Health state",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IsPanEnabled = true,
                //IsZoomEnabled = true

            };
            healthForTemperaturePlotModel.Axes.Add(healthForTemperatureDateTimeAxis);
            healthForTemperaturePlotModel.Axes.Add(healthFortempAxis);
            healthForTemperaturePlotModel.Series.Add(healthForTemperatureSeries);
            TemperatureHealthPlotView.Model = healthForTemperaturePlotModel;
            TemperatureHealthPlotView.Model.PlotMargins = new OxyThickness(41, 10, 10, 80);
            TemperatureHealthPlotView.Model.Padding = new OxyThickness(0);
        }

        private void CreatePressureHealthChart(List<WeatherModel> weatherData, List<HealthStateModel> healthData)
        {
            pressurePlotModel = new PlotModel { Title = "Pressure" };

            var pressureSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Red,
                LineStyle = LineStyle.Solid
            };
            var parsedHealthData = DateTime.TryParse(healthData.LastOrDefault().Date, out DateTime parsedLastHealthdata);

            // Populate temperature data points
            foreach (var item in weatherData)
            {
                if (DateTime.TryParse(item.DateTime, out DateTime parsedDateTime))
                {
                    pressureSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.Pressure));
                }
            }
            var newCol = weatherData.Where(x => DateTime.Parse(x.DateTime) <= parsedLastHealthdata).ToList();
            var minimumDateTemp = (weatherData.Count > 33 ? weatherData[newCol.Count - 33] : weatherData[0]).DateTime;
            // Create the DateTime axis for the X-axis
            var dateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd", // Adjust the format as needed

                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,

                Angle = 90,
                IsPanEnabled = true, // Enable panning
                //IsZoomEnabled = true, // Enable zooming
                // Set the initial window to the last 10 records
                Minimum = DateTimeAxis.ToDouble(DateTime.Parse(minimumDateTemp)),
                Maximum = DateTimeAxis.ToDouble(DateTime.Parse(weatherData[weatherData.Count - 1].DateTime)),
                AbsoluteMaximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.Last().Date)),
                AbsoluteMinimum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.First().Date)),
                LabelFormatter = x => string.Empty
            };
            dateTimeAxis.AxisChanged += OnPressureAxisChanged;
            pressurePlotModel.Axes.Add(dateTimeAxis);

            // Create the Linear axis for the Y-axis (temperature)
            var pressureAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Pressure",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IsPanEnabled = true, // Enable panning
                //IsZoomEnabled = true // Enable zooming
            };

            pressurePlotModel.Axes.Add(pressureAxis);

            // Add the series to the plot model
            pressurePlotModel.Series.Add(pressureSeries);

            // Set the model to plot view
            PressurePlotView.Model = pressurePlotModel;
            PressurePlotView.Model.PlotMargins = new OxyThickness(40, 0, 10, 0);
            PressurePlotView.Model.Padding = new OxyThickness(0);
            #region
            //healthForPressurePlotModel = new PlotModel { };
            //var healthForPressureSeries = new LineSeries
            //{
            //    //Title = "Humidity",
            //    MarkerType = MarkerType.Circle,
            //    MarkerSize = 4,
            //    MarkerStroke = OxyColors.Yellow,
            //    LineStyle = LineStyle.Solid,
            //    Smooth = true,
            //    Color = OxyColors.Red,

            //};
            //foreach (var item in healthData)
            //{
            //    if (DateTime.TryParse(item.Date, out DateTime parsedDateTime))
            //    {
            //        healthForPressureSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.HealthLevel));
            //    }
            //}
            //var minimumDate = (healthData.Count > 5 ? healthData[healthData.Count - 5] : healthData[0]).Date;
            //var healthForPressureDateAxis = new DateTimeAxis
            //{
            //    Position = AxisPosition.Bottom,
            //    StringFormat = "yyyy-MM-dd",
            //    //Title = "Date"
            //    IntervalType = DateTimeIntervalType.Days,
            //    MajorStep = 1,
            //    MajorGridlineStyle = LineStyle.Solid,
            //    MinorGridlineStyle = LineStyle.Dot,
            //    Angle = 90,
            //    IsPanEnabled = true,
            //    //IsZoomEnabled = true,

            //    Minimum = (DateTimeAxis.ToDouble(DateTime.Parse(minimumDate))),
            //    Maximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData[healthData.Count - 1].Date)),
            //    AbsoluteMaximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.Last().Date)),
            //    AbsoluteMinimum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.First().Date)),
            //};
            //healthForPressureDateAxis.AxisChanged += OnPressureHealthAxisChanged;

            //var healthForPressureAxis = new LinearAxis
            //{
            //    Position = AxisPosition.Left,
            //    Title = "Health state",
            //    MajorGridlineStyle = LineStyle.Solid,
            //    MinorGridlineStyle = LineStyle.Dot,
            //    IsPanEnabled = true,
            //    //IsZoomEnabled = true

            //};
            //healthForPressurePlotModel.Axes.Add(healthForPressureDateAxis);
            //healthForPressurePlotModel.Axes.Add(healthForPressureAxis);
            //healthForPressurePlotModel.Series.Add(healthForPressureSeries);
            //PressureHealthPlotView.Model = healthForPressurePlotModel;
            //PressureHealthPlotView.Model.PlotMargins = new OxyThickness(41, 10, 10, 80);
            //PressureHealthPlotView.Model.Padding = new OxyThickness(0);
            #endregion n
        }
        private void CreateHealthChar(List<HealthStateModel> healthData)
        {
            healthForPressurePlotModel = new PlotModel { };
            var healthForPressureSeries = new LineSeries
            {
                //Title = "Humidity",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Yellow,
                LineStyle = LineStyle.Solid,
                Smooth = true,
                Color = OxyColors.Red,

            };
            foreach (var item in healthData)
            {
                if (DateTime.TryParse(item.Date, out DateTime parsedDateTime))
                {
                    healthForPressureSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.HealthLevel));
                }
            }
            var minimumDate = (healthData.Count > 5 ? healthData[healthData.Count - 5] : healthData[0]).Date;
            var healthForPressureDateAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd",
                //Title = "Date"
                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Angle = 90,
                IsPanEnabled = true,
                //IsZoomEnabled = true,

                Minimum = (DateTimeAxis.ToDouble(DateTime.Parse(minimumDate))),
                Maximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData[healthData.Count - 1].Date)),
                AbsoluteMaximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.Last().Date)),
                AbsoluteMinimum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.First().Date)),
            };
            healthForPressureDateAxis.AxisChanged += OnPressureHealthAxisChanged;

            var healthForPressureAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Health state",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IsPanEnabled = true,
                //IsZoomEnabled = true

            };
            healthForPressurePlotModel.Axes.Add(healthForPressureDateAxis);
            healthForPressurePlotModel.Axes.Add(healthForPressureAxis);
            healthForPressurePlotModel.Series.Add(healthForPressureSeries);
            PressureHealthPlotView.Model = healthForPressurePlotModel;
            PressureHealthPlotView.Model.PlotMargins = new OxyThickness(41, 10, 10, 80);
            PressureHealthPlotView.Model.Padding = new OxyThickness(0);
        }
        
        private void CreateHumidityHealthChart(List<WeatherModel> weatherData, List<HealthStateModel> healthData)
        {

            humidityPlotModel = new PlotModel { Title = "Humidity" };

            var humiditySeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Red,
                LineStyle = LineStyle.Solid
            };
            var parsedHealthData = DateTime.TryParse(healthData.LastOrDefault().Date, out DateTime parsedLastHealthdata);
            // Populate temperature data points
            foreach (var item in weatherData)
            {
                if (DateTime.TryParse(item.DateTime, out DateTime parsedDateTime))
                {
                    if (parsedDateTime <= parsedLastHealthdata)
                        humiditySeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.Humidity));
                }
            }
            var newCol = weatherData.Where(x => DateTime.Parse(x.DateTime) <= parsedLastHealthdata).ToList();



            // Create the DateTime axis for the X-axis
            var humidityDateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd", // Adjust the format as needed

                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,

                Angle = 90,
                IsPanEnabled = true, // Enable panning
                //IsZoomEnabled = false, // Enable zooming
                // Set the initial window to the last 10 records
                Minimum = DateTimeAxis.ToDouble(DateTime.Parse(weatherData[newCol.Count - 33].DateTime)),
                Maximum = DateTimeAxis.ToDouble(DateTime.Parse(weatherData[newCol.Count - 1].DateTime)),
                AbsoluteMaximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.Last().Date)),
                AbsoluteMinimum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.First().Date)),

                LabelFormatter = x => string.Empty
            };

            humidityDateTimeAxis.AxisChanged += OnHumidityAxisChanged;

            humidityPlotModel.Axes.Add(humidityDateTimeAxis);

            // Create the Linear axis for the Y-axis (temperature)
            var humidityAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Humidity (%)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IsPanEnabled = true, // Enable panning
                //IsZoomEnabled = true // Enable zooming
            };

            humidityPlotModel.Axes.Add(humidityAxis);

            // Add the series to the plot model
            humidityPlotModel.Series.Add(humiditySeries);

            // Set the model to plot view
            HumidityPlotView.Model = humidityPlotModel;
            HumidityPlotView.Model.PlotMargins = new OxyThickness(40, 0, 10, 0);
            HumidityPlotView.Model.Padding = new OxyThickness(0);
            //the second plotmodel
            healthForHumidityPlotModel = new PlotModel { };
            var healthForHumiditySeries = new LineSeries
            {
                //Title = "Humidity",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Yellow,
                LineStyle = LineStyle.Solid,
                Smooth = true,
                Color = OxyColors.Red,

            };
            foreach (var item in healthData)
            {
                if (DateTime.TryParse(item.Date, out DateTime parsedDateTime))
                {
                    healthForHumiditySeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.HealthLevel));
                }
            }

            var healthForHumidityDateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd",
                //Title = "Date"
                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Angle = 90,
                IsPanEnabled = true,
                //IsZoomEnabled = false,
                Minimum = DateTimeAxis.ToDouble(DateTime.Parse(healthData[healthData.Count - 5].Date)),
                Maximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData[healthData.Count - 1].Date)),
                AbsoluteMaximum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.Last().Date)),
                AbsoluteMinimum = DateTimeAxis.ToDouble(DateTime.Parse(healthData.First().Date)),

            };
            healthForHumidityDateTimeAxis.AxisChanged += OnHumidityHealthAxisChanged;




            var healthFortempAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Health state",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IsPanEnabled = true,
                //IsZoomEnabled = true

            };
            healthForHumidityPlotModel.Axes.Add(healthForHumidityDateTimeAxis);
            healthForHumidityPlotModel.Axes.Add(healthFortempAxis);
            healthForHumidityPlotModel.Series.Add(healthForHumiditySeries);

            HumidityHealthPlotView.Model = healthForHumidityPlotModel;
            HumidityHealthPlotView.Model.PlotMargins = new OxyThickness(41, 10, 10, 80);
            HumidityHealthPlotView.Model.Padding = new OxyThickness(0);
        }


        
        private void CreateChart2(List<WeatherModel> weatherData)
        {

            temperaturePlotmodel = new PlotModel { Title = "Temprature" };

            var temperatureSeries = new LineSeries
            {
                //Title = "Temperature",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Red,
                LineStyle = LineStyle.Solid
            };
            
            // Populate temperature data points
            foreach (var item in weatherData)
            {
                if (DateTime.TryParse(item.DateTime, out DateTime parsedDateTime))
                {
                    temperatureSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.Temperature));
                }
            }

            // Create the DateTime axis for the X-axis
            var dateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd", // Adjust the format as needed
              
                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,

                Angle = 90,
                IsPanEnabled = true, // Enable panning
                //IsZoomEnabled = true, // Enable zooming
                // Set the initial window to the last 10 records

                Minimum = DateTimeAxis.ToDouble(DateTime.Parse(weatherData[weatherData.Count - 10].DateTime)),
                Maximum = DateTimeAxis.ToDouble(DateTime.Parse(weatherData[weatherData.Count - 1].DateTime)),

                LabelFormatter = x => string.Empty
            };
            dateTimeAxis.AxisChanged += OnTemperatureAxisChanged;
            temperaturePlotmodel.Axes.Add(dateTimeAxis);

            // Create the Linear axis for the Y-axis (temperature)
            var temperatureAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Temperature (°C)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IsPanEnabled = true, // Enable panning
                //IsZoomEnabled = true // Enable zooming
            };
         
            temperaturePlotmodel.Axes.Add(temperatureAxis);

            // Add the series to the plot model
            temperaturePlotmodel.Series.Add(temperatureSeries);

            // Set the model to your plot view
            TemperaturePlotView.Model = temperaturePlotmodel;
            TemperaturePlotView.Model.PlotMargins = new OxyThickness(40, 0, 10, 0);
            TemperaturePlotView.Model.Padding = new OxyThickness(0);
            //the second plotmodel
            healthForTemperaturePlotModel = new PlotModel {  };
            var humiditySeries = new LineSeries
            {
                //Title = "Humidity",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Yellow,
                LineStyle = LineStyle.Solid, 
                Smooth = true,
                Color = OxyColors.Red,
                
            };
            foreach (var item in weatherData)
            {
                if (DateTime.TryParse(item.DateTime, out DateTime parsedDateTime))
                {
                    humiditySeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.Humidity));
                }
            }

            var humidityDateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd",
                //Title = "Date"
                IntervalType = DateTimeIntervalType.Days,
                MajorStep = 1,
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Angle = 90,
                IsPanEnabled = true,
                //IsZoomEnabled = true,
                Minimum = DateTimeAxis.ToDouble(DateTime.Parse(weatherData[weatherData.Count - 10].DateTime)),
                Maximum = DateTimeAxis.ToDouble(DateTime.Parse(weatherData[weatherData.Count - 1].DateTime))
            };
            humidityDateTimeAxis.AxisChanged += OnTemperatureHealthAxisChanged;

            var humidityAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Humidity (%)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IsPanEnabled = true,
                //IsZoomEnabled = true
                
            };

            healthForTemperaturePlotModel.Axes.Add(humidityDateTimeAxis);
            healthForTemperaturePlotModel.Axes.Add(humidityAxis);
            healthForTemperaturePlotModel.Series.Add(humiditySeries);

            TemperaturePlotView.Model = healthForTemperaturePlotModel;
            TemperaturePlotView.Model.PlotMargins = new OxyThickness(41,10,10,80);
            TemperaturePlotView.Model.Padding = new OxyThickness(0);
        }
        private void OnTemperatureAxisChanged(object sender, AxisChangedEventArgs e)
        {
            if (isTempHealthSync) return;
            isTempHealthSync = true;

            var temperatureAxis = sender as DateTimeAxis;
            var healthforTempdateAxis = healthForTemperaturePlotModel.Axes[0] as DateTimeAxis; // Assuming the DateTimeAxis is the first axis in the humidity plot
            if (temperatureAxis != null && healthforTempdateAxis != null)
            {
                // Detach event handler to prevent loop
                healthforTempdateAxis.AxisChanged -= OnTemperatureHealthAxisChanged;

                // Sync the humidity axis
                healthforTempdateAxis.Zoom(temperatureAxis.ActualMinimum, temperatureAxis.ActualMaximum);

                // Re-attach event handler
                healthforTempdateAxis.AxisChanged += OnTemperatureHealthAxisChanged;

                // Refresh the humidity plot
                healthForTemperaturePlotModel.InvalidatePlot(false);
            }

            isTempHealthSync = false;
        }

        private void OnTemperatureHealthAxisChanged(object sender, AxisChangedEventArgs e)
        {
            if (isTempHealthSync) return;
            isTempHealthSync = true;
            //
            var healthAxis = sender as DateTimeAxis;
            var temperatureAxis = temperaturePlotmodel.Axes[0] as DateTimeAxis; // Assuming the DateTimeAxis is the first axis in the temperature plot
            if (healthAxis != null && temperatureAxis != null)
            {
                // Detach event handler to prevent loop
                temperatureAxis.AxisChanged -= OnTemperatureAxisChanged;

                // Sync the temperature axis
                temperatureAxis.Zoom(healthAxis.ActualMinimum, healthAxis.ActualMaximum);

                // Re-attach event handler
                temperatureAxis.AxisChanged += OnTemperatureAxisChanged;

                // Refresh the temperature plot
                temperaturePlotmodel.InvalidatePlot(false);
            }

            isTempHealthSync = false;
        }
        //
        private void OnPressureAxisChanged(object sender, AxisChangedEventArgs e)
        {
            if (isPressHealthSync) return;
            isPressHealthSync = true;

            var pressureAxis = sender as DateTimeAxis;
            var pressHealthAxis = healthForPressurePlotModel.Axes[0] as DateTimeAxis; // Assuming the DateTimeAxis is the first axis in the humidity plot
            if (pressureAxis != null && pressHealthAxis != null)
            {
                // Detach event handler to prevent loop
                pressHealthAxis.AxisChanged -= OnPressureHealthAxisChanged;

                // Sync the humidity axis
                pressHealthAxis.Zoom(pressureAxis.ActualMinimum, pressureAxis.ActualMaximum);

                // Re-attach event handler
                pressHealthAxis.AxisChanged += OnPressureHealthAxisChanged;

                // Refresh the humidity plot
                healthForPressurePlotModel.InvalidatePlot(false);
            }

            isPressHealthSync = false;
        }

        private void OnPressureHealthAxisChanged(object sender, AxisChangedEventArgs e)
        {
            if (isPressHealthSync) return;
            isPressHealthSync = true;

            var pressHealthAxis = sender as DateTimeAxis;
            var pressAxis = pressurePlotModel.Axes[0] as DateTimeAxis; // Assuming the DateTimeAxis is the first axis in the temperature plot
            if (pressHealthAxis != null && pressAxis != null)
            {
                // Detach event handler to prevent loop
                pressAxis.AxisChanged -= OnPressureAxisChanged;

                // Sync the temperature axis
                pressAxis.Zoom(pressHealthAxis.ActualMinimum, pressHealthAxis.ActualMaximum);

                // Re-attach event handler
                pressAxis.AxisChanged += OnPressureAxisChanged;

                // Refresh the temperature plot
                pressurePlotModel.InvalidatePlot(false);
            }

            isPressHealthSync = false;
        }


        //
        private void OnHumidityAxisChanged(object sender, AxisChangedEventArgs e)
        {
            if (isHumHealthSync) return;
            isHumHealthSync = true;

            var humidityAxis = sender as DateTimeAxis;
            var humHealthAxis = healthForHumidityPlotModel.Axes[0] as DateTimeAxis; // Assuming the DateTimeAxis is the first axis in the humidity plot
            if (humidityAxis != null && humHealthAxis != null)
            {
                // Detach event handler to prevent loop
                humHealthAxis.AxisChanged -= OnHumidityHealthAxisChanged;

                // Sync the humidity axis
                humHealthAxis.Zoom(humidityAxis.ActualMinimum, humidityAxis.ActualMaximum);

                // Re-attach event handler
                humHealthAxis.AxisChanged += OnHumidityHealthAxisChanged;

                // Refresh the humidity plot
                healthForHumidityPlotModel.InvalidatePlot(false);
            }
            isHumHealthSync = false;
        }
        private void OnHumidityHealthAxisChanged(object sender, AxisChangedEventArgs e)
        {
            if (isHumHealthSync) return;
            isHumHealthSync = true;

            var humHealthAxis = sender as DateTimeAxis;
            var humAxis = humidityPlotModel.Axes[0] as DateTimeAxis; // Assuming the DateTimeAxis is the first axis in the temperature plot
            if (humHealthAxis != null && humAxis != null)
            {
                // Detach event handler to prevent loop
                humAxis.AxisChanged -= OnHumidityAxisChanged;

                // Sync the temperature axis
                humAxis.Zoom(humHealthAxis.ActualMinimum, humHealthAxis.ActualMaximum);

                // Re-attach event handler
                humAxis.AxisChanged += OnHumidityAxisChanged;

                // Refresh the temperature plot
                humidityPlotModel.InvalidatePlot(false);
            }

            isHumHealthSync = false;
        }
















        private void CreateChart(List<WeatherModel> weather)
        {
            var plotModel = new PlotModel { Title = "Temperature Data" };

            var temperatireSeries = new LineSeries
            {
                Title = "Temp",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White,
                YAxisKey = "TemperatureAxis"
            };
            foreach (var item in weather)
            {
                if (DateTime.TryParse(item.DateTime, out DateTime parsedDateTime))
                    temperatireSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.Temperature));
            }

            var humiditySeries = new LineSeries
            {
                Title = "Humidity",
                MarkerType = MarkerType.Square,
                MarkerSize = 4,
                MarkerStroke = OxyColors.Blue,
                LineStyle = LineStyle.Solid, 
                BrokenLineColor = OxyColors.White,
                
            };
            // Example data


            DateTime dateTime = DateTime.Now.AddDays(-1);
            DateTime dateTime2 = DateTime.Now.AddDays(2);
            foreach (var item in weather)
            {
                if (DateTime.TryParse(item.DateTime, out DateTime parsedDateTime))
                {
                    if (parsedDateTime.Date == dateTime.Date || parsedDateTime.Date == dateTime2.Date)
                    {
                        continue;
                    }
                    if (parsedDateTime.Hour == 00)
                    {
                        humiditySeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.Humidity));

                    }
                }
            }
            var windSeries = new BarSeries
            {
                Title = "Wind",
                YAxisKey = "WindAxis",
                FillColor = OxyColors.Blue,
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1
            };
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left, // Typically for a BarSeries, it's on the left
                Key = "WindAxis",
                Title = "Date"
            };
            for (int i = 0; i < weather.Count; i++)
            {
                if (DateTime.TryParse(weather[i].DateTime, out DateTime parsedDateTime))
                {
                    windSeries.Items.Add(new BarItem { Value = weather[i].WindSpeed, CategoryIndex = i });
                    categoryAxis.Labels.Add(parsedDateTime.ToString("yyyy-MM-dd"));
                }
            }
            //GPG 

            plotModel.Series.Add(temperatireSeries);

            var dateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd", // Adjust the format as needed
                Title = "Date",
                IntervalType = DateTimeIntervalType.Days, // Set the interval type

                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MinimumMajorStep = 0,
                MajorStep = 1, MinorStep = 1,
                MinorIntervalType = DateTimeIntervalType.Hours,
                
                Angle = 90
            };
            plotModel.Axes.Add(dateTimeAxis);
            plotModel.Series.Add(humiditySeries);
            var termeratureAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Temperature (°C)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Key = "TemperatureAxis", // Key to associate with the temperature series
                StartPosition = 0.2, // Shift to the right to avoid overlap
                EndPosition = 1
            };
            //var windAxis = new LinearAxis
            //{
            //    Position = AxisPosition.Right,
            //    Title = "Winds (%)",
            //    MajorGridlineStyle = LineStyle.Solid,
            //    MinorGridlineStyle = LineStyle.Dot,
            //    Key = "WindAxis", // Key to associate with the humidity series
            //    IsPanEnabled = false,
            //    IsZoomEnabled = false
            //};
            //plotModel.Axes.Add(windAxis);
            //plotModel.Series.Add(windSeries);



            plotModel.Axes.Add(termeratureAxis);
            TemperaturePlotView.Model = plotModel;
        }
        private void CreateSingleChart(List<WeatherModel> weather)
        {
            var plotModel = new PlotModel { Title = "Temperature Data" };

            var temperatireSeries = new LineSeries
            {
                Title = "Temp",
                MarkerType = MarkerType.Circle,
                MarkerSize = 4,
                MarkerStroke = OxyColors.White,
                LineStyle = LineStyle.Solid,
                YAxisKey = "TemperatureAxis"
            };

            
            foreach (var item in weather)
            {
                if (DateTime.TryParse(item.DateTime, out DateTime parsedDateTime))
                    temperatireSeries.Points.Add(new DataPoint(DateTimeAxis.ToDouble(parsedDateTime), item.Temperature));
            }
      

            // Create the bar series for humidity
            var humiditySeries = new BarSeries
            {
                Title = "Humidity",
                YAxisKey = "HumidityAxis", // Link to the humidity axis
                FillColor = OxyColors.Blue,
                StrokeColor = OxyColors.Black,
                StrokeThickness = 1
            };
            //
            // Create a category axis for the Y-axis
            var categoryAxis = new CategoryAxis
            {
                Position = AxisPosition.Left, // Typically for a BarSeries, it's on the left
                Key = "HumidityAxis",
                Title = "Date"
            };
            //
        
            // Populate humidity data points and categories
            for (int i = 0; i < weather.Count; i++)
            {
                if (DateTime.TryParse(weather[i].DateTime, out DateTime parsedDateTime))
                {
                    humiditySeries.Items.Add(new BarItem { Value = weather[i].Humidity, CategoryIndex = i });
                    categoryAxis.Labels.Add(parsedDateTime.ToString("yyyy-MM-dd"));
                }
            }
            //
            var dateTimeAxis = new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "yyyy-MM-dd", // Adjust the format as needed
                Title = "Date",
                IntervalType = DateTimeIntervalType.Days, // Set the interval type
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                MinimumMajorStep = 0,
                MajorStep = 1,
                MinorStep = 1,
                MinorIntervalType = DateTimeIntervalType.Hours,

                Angle = 90
            };
            plotModel.Axes.Add(dateTimeAxis);
            //
            ////plotModel.Series.Add(humiditySeries);
            var termeratureAxis = new LinearAxis
            {
                Position = AxisPosition.Left,
                Title = "Temperature (°C)",
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                Key = "TemperatureAxis", // Key to associate with the temperature series
                StartPosition = 0.2, // Shift to the right to avoid overlap
                EndPosition = 1
            };
            plotModel.Axes.Add(termeratureAxis);


            plotModel.Axes.Add(categoryAxis);

            // Add both series to the plot model
            plotModel.Series.Add(temperatireSeries);
            plotModel.Series.Add(humiditySeries);

            // Set the model to your plot view
            TemperaturePlotView.Model = plotModel;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            var result = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Lowest, TimeSpan.FromMinutes(1)));
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LoginPage(meteoHealthRepository));
        }
    }
}