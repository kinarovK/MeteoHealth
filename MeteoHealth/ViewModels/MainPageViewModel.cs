using MeteoHealth.Services;
using MeteoHealth.Views;
using OxyPlot;
//using OxyPlot.Xamarin.Forms;
using SQLite_Database_service;
using SQLite_Database_service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace MeteoHealth.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        private readonly IMeteoHealthRepository _meteoHealthRepository;
        private readonly IChartMaker chartmaker;

        //Temperature
        private PlotModel _temperaturePlotModel;
        public PlotModel TemperaturePlotModel
        {
            get => _temperaturePlotModel;
            set
            {
                _temperaturePlotModel = value;
                OnPropertyChanged(nameof(TemperaturePlotModel));
            }
        }
        private PlotModel _healthTemperaturePlotModel;
        public PlotModel HealthTemperaturePlotModel
        {
            get => _healthTemperaturePlotModel;
            set
            {
                _healthTemperaturePlotModel = value;
                OnPropertyChanged(nameof(HealthTemperaturePlotModel));
            }
        }
        //Pressure
        private PlotModel _pressurePlotModel;
        public PlotModel PressurePlotModel
        {
            get => _pressurePlotModel;
            set
            {
                _pressurePlotModel = value;
                OnPropertyChanged(nameof(PressurePlotModel));
            }
        }
        private PlotModel _healthPressurePlotModel;
        public PlotModel HealthPressurePlotModel
        {
            get => _healthPressurePlotModel;
            set
            {
                _healthPressurePlotModel = value;
                OnPropertyChanged(nameof(HealthPressurePlotModel));
            }
        }
        //humidity
        private PlotModel _humidityPlotModel;
        public PlotModel HumidityPlotModel
        {
            get => _humidityPlotModel;
            set
            {
                _humidityPlotModel = value;
                OnPropertyChanged(nameof(HumidityPlotModel));
            }
        }
        private PlotModel _healthHumidityPlotModel;
        public PlotModel HealthHumidityPlotModel
        {
            get => _healthHumidityPlotModel;
            set
            {
                _healthHumidityPlotModel = value;
                OnPropertyChanged(nameof(HealthHumidityPlotModel));
            }
        }
        //Wind
        private PlotModel _windPlotModel;
        public PlotModel WindPlotModel
        {
            get => _windPlotModel;
            set
            {
                _windPlotModel = value;
                OnPropertyChanged(nameof(WindPlotModel));
            }
        }
        private PlotModel _windHealthPlotModel;
        public PlotModel HealthWindPlotModel
        {
            get => _windHealthPlotModel;
            set
            {
                _windHealthPlotModel = value;
                OnPropertyChanged(nameof(HealthWindPlotModel));
            }
        }
        //PrecipitationProb
        private PlotModel _precipitationProbPlotModel;
        public PlotModel PrecipitationProbPlotModel
        {
            get => _precipitationProbPlotModel;
            set
            {
                _precipitationProbPlotModel = value;
                OnPropertyChanged(nameof(PrecipitationProbPlotModel));
            }
        }
        private PlotModel _healthPrecipitationProbPlotModel;
        public PlotModel HealthPrecipitationProbPlotModel
        {
            get => _healthPrecipitationProbPlotModel;
            set
            {
                _healthPrecipitationProbPlotModel = value;
                OnPropertyChanged(nameof(HealthPrecipitationProbPlotModel));
            }
        }
        //PrecipitationVol
        private PlotModel _precipitationVolPlotModel;
        public PlotModel PrecipitationVolPlotModel
        {
            get => _precipitationVolPlotModel;
            set
            {
                _precipitationVolPlotModel = value;
                OnPropertyChanged(nameof(PrecipitationVolPlotModel));
            }
        }
        private PlotModel _healthPrecipitationVolPlotModel;
        public PlotModel HealthPrecipitationVolPlotModel
        {
            get => _healthPrecipitationVolPlotModel;
            set
            {
                _healthPrecipitationVolPlotModel = value;
                OnPropertyChanged(nameof(HealthPrecipitationVolPlotModel));
            }
        }
        //CTOR
        public MainPageViewModel(IMeteoHealthRepository meteoHealthRepository, IChartMaker chartMaker)
        {
            _meteoHealthRepository = meteoHealthRepository;
            this.chartmaker = chartMaker;
            ShowhealthPopupCommand = new Command(ShowHealthPopup);
            ShowAboutPageCommand = new Command(ShowAboutPage);
            ShowGeolocationCommand = new Command(ShowGeoLocationPage);
            OnApperering();
            InitializeChartsAsync();
        }

        public void InitializeChartsAsync()
        {
            //var weatherData = await _meteoHealthRepository.GetWeatherModelAsync();
            //var healthState = await _meteoHealthRepository.GetHealthStatesAsync();

            var weatherData = CreateMockWeatherModels();
            var healthState = CreateHealthModelMock();

            TemperaturePlotModel = chartmaker.CreateWeatherChart(weatherData, healthState, "Temperature", "Temperature");
            HealthTemperaturePlotModel = chartmaker.CreateHealthChar(healthState, "HealthState", "temp");
            chartmaker.InitializeCharts(TemperaturePlotModel, HealthTemperaturePlotModel);

            PressurePlotModel = chartmaker.CreateWeatherChart(weatherData, healthState, "Pressure", "Pressure");
            HealthPressurePlotModel = chartmaker.CreateHealthChar(healthState, "HealthState", "press");
            chartmaker.InitializeCharts(PressurePlotModel, HealthPressurePlotModel);

            HumidityPlotModel = chartmaker.CreateWeatherChart(weatherData, healthState, "Humidity", "Humidity");
            HealthHumidityPlotModel = chartmaker.CreateHealthChar(healthState, "HealthState", "hum");
            chartmaker.InitializeCharts(HumidityPlotModel, HealthHumidityPlotModel);

            WindPlotModel = chartmaker.CreateWeatherChart(weatherData, healthState, "Wind", "Wind");
            HealthWindPlotModel = chartmaker.CreateHealthChar(healthState, "HealthState", "Wind");
            chartmaker.InitializeCharts(WindPlotModel, HealthWindPlotModel);

            PrecipitationProbPlotModel = chartmaker.CreateWeatherChart(weatherData, healthState, "PrecipitationProbability", "PrecipitationProbability");
            HealthPrecipitationProbPlotModel = chartmaker.CreateHealthChar(healthState, "HealthState", "PrecProb");
            chartmaker.InitializeCharts(PrecipitationProbPlotModel, HealthPrecipitationProbPlotModel);

            PrecipitationVolPlotModel = chartmaker.CreateWeatherChart(weatherData, healthState, "PrecipitationVolume", "PrecipitationVolume");
            HealthPrecipitationVolPlotModel = chartmaker.CreateHealthChar(healthState, "HealthState", "PrecVol");
            chartmaker.InitializeCharts(PrecipitationVolPlotModel, HealthPrecipitationVolPlotModel);




        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
                    Humidity = rand.Next(30, 100)
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
        private async void OnApperering()
        {
            CheckHealthState();
            var geolocation = _meteoHealthRepository.GetGeolocationModelsAsync().Result;

            if (geolocation.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Ooops", "Not found geolocation, please set it", "OK");
                await Application.Current.MainPage.Navigation.PushAsync(new GeolocationPage(_meteoHealthRepository));
            }
            var weather = _meteoHealthRepository.GetWeatherModelAsync().Result;
            var some = weather;

        }
        public ICommand ShowhealthPopupCommand { get; }
        private void ShowHealthPopup()
        {
            var popup = new HealthStatePopup(_meteoHealthRepository, "Hi");
            Application.Current.MainPage.Navigation.ShowPopup(popup);
        }

        public ICommand ShowAboutPageCommand { get; }
        
        internal async void CheckHealthState()
        {
            
           var healthStates = await _meteoHealthRepository.GetHealthStatesAsync();
            if (healthStates.Count == 0)
            {
                await Application.Current.MainPage.Navigation.ShowPopupAsync(new HealthStatePopup(_meteoHealthRepository, "How do u feel today?"));
                return;
            }
            var todayDate = DateTime.Today;
            var today = healthStates.FirstOrDefault(x => x.Date == todayDate.ToString());
            DateTime.TryParse(healthStates.FirstOrDefault().Date, out var firstDateInDb);
            if (today is null)
            {
                await Application.Current.MainPage.Navigation.ShowPopupAsync(new HealthStatePopup(_meteoHealthRepository, "How do u feel today?"));
            }
            bool isRecordExists = false;
            while (!isRecordExists)
            {
                todayDate =  todayDate.AddDays(-1);
                if (healthStates.FirstOrDefault(x=> x.Date == todayDate.ToString()) is null || todayDate > firstDateInDb)
                {
                    var date = todayDate.ToString("MM.dd");
                    await Application.Current.MainPage.Navigation.ShowPopupAsync(new HealthStatePopup(_meteoHealthRepository, $"Oops its look like in {todayDate.ToString("MM.dd")} you not checked stete,  How was your state in this day?"));
                }
                else
                {
                    isRecordExists = true;
                }
                
            }
        }
        private async void ShowAboutPage()
        {

            await Application.Current.MainPage.Navigation.PushAsync(new AboutPage());
        }
        public ICommand ShowGeolocationCommand { get; }
        private async void ShowGeoLocationPage()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new GeolocationPage(_meteoHealthRepository));
        }
        
        

    }
}
