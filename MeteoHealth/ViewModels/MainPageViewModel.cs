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
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace MeteoHealth.ViewModels
{
    public class MainPageViewModel : BaseViewModel //INotifyPropertyChanged
    {
        private readonly IMeteoHealthRepository _meteoHealthRepository;
        private readonly IChartMaker chartmaker;
        private readonly IOpenWeatherMapApiController apiController;
        private readonly IOpenWeatherMapConverter apiService;
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
        public MainPageViewModel(IMeteoHealthRepository meteoHealthRepository, IChartMaker chartMaker, IOpenWeatherMapApiController apiController, IOpenWeatherMapConverter apiService, CancellationToken cancellationToken)
        {
            _meteoHealthRepository = meteoHealthRepository;
            this.chartmaker = chartMaker;
            this.apiController = apiController;
            this.apiService = apiService;
            this.cancellationToken = cancellationToken;
        }
        private CancellationToken cancellationToken;

        public async Task OnApperering()
        {

            IsLoading = true;
            try
            {

                await CheckHealthState(cancellationToken);
                await CheckGeolocation(cancellationToken);
                await CheckWeather(cancellationToken);
                await InitializeChartsAsync(cancellationToken);
            }
        
            catch (HttpRequestException)
            {
                bool answer = await Application.Current.MainPage.DisplayAlert("Ooops", "Your internet connection is weak or missing. Please check your connection or try again later.", "Try again", "Cancel");
                if (answer)
                {
                    await CheckWeather(cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                //Operration in current page cancelled, going to next page

                //await Application.Current.MainPage.DisplayAlert("Cancelled", "Operation cancelled", "Ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Ooops", "Something went wrong. Please try again later.", "OK");

            }
            finally
            {
                IsLoading = false;
            }


        }
        public async Task InitializeChartsAsync(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var weatherData = await _meteoHealthRepository.GetWeatherModelAsync(cancellationToken); 
            var healthState = await _meteoHealthRepository.GetHealthStatesAsync(cancellationToken);

            //var weatherData = CreateMockWeatherModels();
            //var healthState = CreateHealthModelMock();


            if (weatherData.Count == 0 || healthState.Count < 2)
            {
                NotEnoughDataLabel = "The charts will appear here once there is enough data";
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
       
        internal async Task CheckGeolocation(CancellationToken token)
        {
            var geolocation = await _meteoHealthRepository.GetGeolocationModelsAsync(cancellationToken);
            await Task.Delay(5000);

            if (geolocation.Count == 0)
            {
                token.ThrowIfCancellationRequested();

                await Application.Current.MainPage.DisplayAlert("Ooops", "Geolocation not found, please set it", "OK");

                var tcs = new TaskCompletionSource<bool>();

                var geolocationPage = new GeolocationPage(_meteoHealthRepository);

                geolocationPage.Disappearing += (sender, e) => tcs.SetResult(true);

                
                await Application.Current.MainPage.Navigation.PushModalAsync(geolocationPage);
                await tcs.Task;

            }
        }
        internal async Task CheckWeather(CancellationToken token)
        {
          

            token.ThrowIfCancellationRequested();
            var weathers = await _meteoHealthRepository.GetLastWeatherModelAsync(cancellationToken); //or maybe get last 
            if (weathers == null || DateTime.Parse(weathers.RequestDate) != DateTime.Today.AddDays(30))//exception here
            {
                var geolocation = await _meteoHealthRepository.GetLastGeolocationModelAsync(cancellationToken); //get just last
                WeatherApiResponse apiResult =await apiController.ExecuteApiRequest(geolocation.Latitude.ToString(), geolocation.Longitude.ToString(), token);
                if (apiResult != null)
                {
                    await _meteoHealthRepository.UpsertWeatherModelAsync(apiService.ConvertToModel(apiResult));

                }
                else
                {
                    throw new Exception();
                }
            }
            IsLoading = false;
        }
      
        private async Task ShowHealthPopupAsync(string message, DateTime healthStateDate)
        { 
            await Application.Current.MainPage.Navigation.ShowPopupAsync(new HealthStatePopup(_meteoHealthRepository, message, healthStateDate));

        }



        internal async Task CheckHealthState(CancellationToken token)// 
        {

            var healthStates =  await _meteoHealthRepository.GetHealthStatesAsync(cancellationToken);

            foreach (var item in healthStates)
            {
                Debug.WriteLine(item.Date);
            }
            var healthStateDate = DateTime.Today;
            if (healthStates.Count == 0)
            {
                await ShowHealthPopupAsync("How are you feeling today?", healthStateDate);
                return;
            }
            var today = healthStates.FirstOrDefault(x => x.Date == healthStateDate.ToString());
            DateTime.TryParse(healthStates.FirstOrDefault().Date, out var firstDateInDb);

            if (healthStates.Count == 0 || today is null)
            {
                await ShowHealthPopupAsync("How are you feeling today?", healthStateDate);

            }
      
            bool isRecordExists = false;
            while (!isRecordExists)
            {
                healthStateDate = healthStateDate.AddDays(-1);
                if (healthStates.FirstOrDefault(x => x.Date == healthStateDate.ToString()) is null && healthStateDate > firstDateInDb)
                {
                    var date = healthStateDate.ToString("MM.dd");
                    await ShowHealthPopupAsync($"Oops, it looks like you didn't check the state at " +
                        $"{healthStateDate.ToString("MM.dd")}.  How were you feeling on this day?", healthStateDate);

                }
                else
                {
                    isRecordExists = true;
                }
            }
        }
      
    }
}
