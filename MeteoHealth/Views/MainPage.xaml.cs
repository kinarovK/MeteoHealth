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

using Xamarin.Essentials;
using Microsoft.Extensions.DependencyInjection;
using MeteoHealth.ViewModels;
using OpenWeatherMap_Api_Service.Interfaces;

namespace MeteoHealth.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        //private readonly IMeteoHealthRepository repo;

        private MainPageViewModel viewModel;
        public MainPage(IMeteoHealthRepository repo, IChartMaker chartMaker, IOpenWeatherMapApiController apiController, IWeatherApiService apiService)
        {
            InitializeComponent();

        
            viewModel = new MainPageViewModel(repo, chartMaker, apiController, apiService);
            BindingContext = viewModel;
         
        }

        
        protected override async void OnAppearing()
        {
            base.OnAppearing();
         
            var oxyThicknessForWeatherCharts = new OxyThickness(50, 10, 10, 0);
            var oxyThicknessForHealth = new OxyThickness(50, 10, 10, 80);
            var defaultOxyThickness = new OxyThickness(0);
            await viewModel.OnApperering();

            try
            {
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
            }
            catch (NullReferenceException ex)
            {
                //Throws when not enought data 
                return;
            }
            catch (Exception ex)
            {

                
                await Application.Current.MainPage.DisplayAlert("Error", $"An unexpected error occurred, try letter", "OK");
                //OnAppearing();
                return;
            }
           
        }
    }
}