using MeteoHealth.Services;
using MeteoHealth.Views;
using OpenWeatherMap_Api_Service.Interfaces;
using OpenWeatherMap_Api_Service.Models;
using OxyPlot;
//using OxyPlot.Xamarin.Forms;
using SQLite_Database_service.Interfaces;
using SQLite_Database_service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private readonly IMeteoHealthRepository _meteoHealthRepository;
        private readonly IChartMaker chartmaker;
        private readonly IOpenWeatherMapApiController apiController;
        private readonly IWeatherApiService apiService;
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
       
        private string notEnoughDataLabel;
        public string NotEnoughDataLabel
        {
            get { return notEnoughDataLabel;}
            set
            {
                if (notEnoughDataLabel != value)
                {
                    notEnoughDataLabel= value;
                    OnPropertyChanged(nameof(NotEnoughDataLabel));
                }
 
            }
        }
        private bool isLoading = false;
        public bool IsLoading
        {
            get { return isLoading; }
            set 
            {
                    isLoading= value;
                    OnPropertyChanged(nameof(IsLoading));
            }
        }
        //CTOR
        public MainPageViewModel(IMeteoHealthRepository meteoHealthRepository, IChartMaker chartMaker, IOpenWeatherMapApiController apiController, IWeatherApiService apiService)
        {
            _meteoHealthRepository = meteoHealthRepository;
            this.chartmaker = chartMaker;
            this.apiController = apiController;
            this.apiService = apiService;
            //ShowhealthPopupCommand = new Command(async () => await ShowHealthPopupAsync());
            //ShowAboutPageCommand = new Command(async () => await ShowAboutPage());
            //ShowGeolocationCommand = new Command(async () => await ShowGeoLocationPage());
        }
        //internal async Task InitializeAsync()
        //{
        //    await OnApperering();
        //    //await InitializeChartsAsync();
        //}
        public async Task InitializeChartsAsync()
        {
            var weatherData = await _meteoHealthRepository.GetWeatherModelAsync(); 
            var healthState = await _meteoHealthRepository.GetHealthStatesAsync();


            //var weatherData = CreateMockWeatherModels();
            //var healthState = CreateHealthModelMock();


            if (weatherData.Count == 0 || healthState.Count < 2)
            {
                NotEnoughDataLabel = "Here will be appears the charts when be enought data";
                return;
            }

            TemperaturePlotModel = chartmaker.CreateWeatherChart(weatherData, healthState, "Temperature (C°)", "Temperature");
            HealthTemperaturePlotModel = chartmaker.CreateHealthChar(healthState, "Health State", "temp");
            chartmaker.InitializeCharts(TemperaturePlotModel, HealthTemperaturePlotModel); 

            PressurePlotModel = chartmaker.CreateWeatherChart(weatherData, healthState, "Pressure (hPa)", "Pressure");
            HealthPressurePlotModel = chartmaker.CreateHealthChar(healthState, "Health State", "press");
            chartmaker.InitializeCharts(PressurePlotModel, HealthPressurePlotModel);

            HumidityPlotModel = chartmaker.CreateWeatherChart(weatherData, healthState, "Humidity (%)", "Humidity");
            HealthHumidityPlotModel = chartmaker.CreateHealthChar(healthState, "Health State", "hum");
            chartmaker.InitializeCharts(HumidityPlotModel, HealthHumidityPlotModel);

            WindPlotModel = chartmaker.CreateWeatherChart(weatherData, healthState, "Wind (m/s)", "Wind");
            HealthWindPlotModel = chartmaker.CreateHealthChar(healthState, "Health State", "Wind");
            chartmaker.InitializeCharts(WindPlotModel, HealthWindPlotModel);

            PrecipitationProbPlotModel = chartmaker.CreateWeatherChart(weatherData, healthState, "Precipitation Probability (%)", "Precipitation Probability");
            HealthPrecipitationProbPlotModel = chartmaker.CreateHealthChar(healthState, "Health State", "PrecProb");
            chartmaker.InitializeCharts(PrecipitationProbPlotModel, HealthPrecipitationProbPlotModel);

            PrecipitationVolPlotModel = chartmaker.CreateWeatherChart(weatherData, healthState, "Precipitation Volume (mm)", "Precipitation Volume");
            HealthPrecipitationVolPlotModel = chartmaker.CreateHealthChar(healthState, "Health State", "PrecVol");
            chartmaker.InitializeCharts(PrecipitationVolPlotModel, HealthPrecipitationVolPlotModel);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Mock data creation for testing 
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
        #endregion
        public async Task OnApperering()
        {
            IsLoading = true;
            //remove try
            try
            {
                await CheckHealthState();
                await CheckGeolocation();
                await CheckWeather();
                await InitializeChartsAsync();
            }
            catch (Exception ex)
            {
                var em = ex.Message;
                await Application.Current.MainPage.DisplayAlert("Ooops", "Something went wrong try again letter", "OK");

            }
            finally
            {
                IsLoading = false;
            }


        }
        internal async Task CheckGeolocation()
        {
            var geolocation = await _meteoHealthRepository.GetGeolocationModelsAsync();

            if (geolocation.Count == 0)
            {

                await Application.Current.MainPage.DisplayAlert("Ooops", "Not found geolocation, please set it", "OK");

                var tcs = new TaskCompletionSource<bool>();

                var geolocationPage = new GeolocationPage(_meteoHealthRepository);

                geolocationPage.Disappearing += (sender, e) => tcs.SetResult(true);

                
                await Application.Current.MainPage.Navigation.PushModalAsync(geolocationPage);
                await tcs.Task;

                //ShowHealthPopup();
                //await Application.Current.MainPage.Navigation.PushModalAsync(new GeolocationPage(_meteoHealthRepository));
                
            }
        }
        internal async Task CheckWeather()
        {

            var weathers = await _meteoHealthRepository.GetLastWeatherModelAsync(); //or maybe get last 
            if (weathers == null || DateTime.Parse(weathers.RequestDate) != DateTime.Today)//exception here
            {
                var geolocation = await _meteoHealthRepository.GetLastGeolocationModelAsync(); //get just last
                WeatherApiResponse apiResult =await apiController.ExecuteApiRequest(geolocation.Latitude.ToString(), geolocation.Longitude.ToString());
                if (apiResult != null)
                {
                    await _meteoHealthRepository.UpsertWeatherModelAsync(apiService.ConvertToModel(apiResult));

                }
                else
                {
                    bool answer = await Application.Current.MainPage.DisplayAlert("Ooops", "Your internet connection blabla, check your internet connection or try letter", "Try again", "Cancel");
                    if (answer)
                    {
                        await CheckWeather();
                    }
                }
            }
            IsLoading = false;
        }
      
        private async Task ShowHealthPopupAsync(string message, DateTime healthStateDate)
        {
            await Application.Current.MainPage.Navigation.ShowPopupAsync(new HealthStatePopup(_meteoHealthRepository, message, healthStateDate));

        }



        internal async Task CheckHealthState()// 
        {
            
            var healthStates =  await _meteoHealthRepository.GetHealthStatesAsync();
            var healthStateDate = DateTime.Today;
            if (healthStates.Count == 0)
            {
                await ShowHealthPopupAsync("How do you feel today?", healthStateDate);
                //await Application.Current.MainPage.Navigation.ShowPopupAsync(new HealthStatePopup(_meteoHealthRepository, "How do u feel today?", healthStateDate));
                return;
            }
            var today = healthStates.FirstOrDefault(x => x.Date == healthStateDate.ToString());
            DateTime.TryParse(healthStates.FirstOrDefault().Date, out var firstDateInDb);

            if (healthStates.Count == 0 || today is null)
            {
                //call instead ShowHealthPopup
                await ShowHealthPopupAsync("How do you feel today?", healthStateDate);

                //await Application.Current.MainPage.Navigation.ShowPopupAsync(new HealthStatePopup(_meteoHealthRepository, "How do u feel today?", healthStateDate));
                //return;
            }
      
            //if (today is null)
            //{
            //    await Application.Current.MainPage.Navigation.ShowPopupAsync(new HealthStatePopup(_meteoHealthRepository, "How do u feel today?", healthStateDate));
            //}
            bool isRecordExists = false;
            while (!isRecordExists)
            {
                healthStateDate = healthStateDate.AddDays(-1);
                if (healthStates.FirstOrDefault(x => x.Date == healthStateDate.ToString()) is null && healthStateDate > firstDateInDb)
                {
                    var date = healthStateDate.ToString("MM.dd");
                    await ShowHealthPopupAsync($"Oops its look like in " +
                        $"{healthStateDate.ToString("MM.dd")} you not checked stete,  How was your state in this day?", healthStateDate);

                    //await Application.Current.MainPage.Navigation.ShowPopupAsync(new HealthStatePopup(_meteoHealthRepository, $"Oops its look like in " +
                    //    $"{healthStateDate.ToString("MM.dd")} you not checked stete,  How was your state in this day?", healthStateDate));
                }
                else
                {
                    isRecordExists = true;
                }
            }
        }
      
    }
}
