using MeteoHealth.Services;
using Microsoft.Extensions.Primitives;
using OpenWeatherMap_Api_Service.Interfaces;
using Report_Service.Interfaces;
using SQLite_Database_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeteoHealth.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainFlyoutPage : FlyoutPage
	{
		private readonly IMeteoHealthRepository meteoHealthRepository;
		private readonly IChartMaker chartMaker;
        private readonly IReportMaker reportMaker;
        private readonly IOpenWeatherMapApiController apiController;
        private readonly IWeatherApiService apiService;
        private MainPage mainPage;
        private NavigationPage navigationPage;
        private CancellationTokenSource cancellationTokenSource;
        public MainFlyoutPage (IMeteoHealthRepository repo, IChartMaker chMaker, IReportMaker reportMaker, IOpenWeatherMapApiController apiController, IWeatherApiService apiService)
		{

            cancellationTokenSource = new CancellationTokenSource();
            InitializeComponent ();
			meteoHealthRepository = repo;
			chartMaker = chMaker;
            this.reportMaker = reportMaker;
            this.apiController = apiController;
            this.apiService = apiService;
            mainPage = new MainPage(meteoHealthRepository, chartMaker, apiController, apiService, cancellationTokenSource.Token);
            navigationPage = new NavigationPage(mainPage);
            Detail = navigationPage;

		}
        internal void OnHomeClicked(object sender, EventArgs e)

        {
            cancellationTokenSource?.Cancel();

            if (cancellationTokenSource.IsCancellationRequested)
            {
                //cancellationTokenSource.Dispose();
                cancellationTokenSource = new CancellationTokenSource();
            }
            var mainPage = new MainPage(meteoHealthRepository, chartMaker, apiController, apiService, cancellationTokenSource.Token);
            navigationPage = new NavigationPage(mainPage);
            Detail = navigationPage;
            IsPresented = false;
        }
        private void OnGeolocationClicked(object sender, EventArgs e)
		{
            cancellationTokenSource?.Cancel();
            if (cancellationTokenSource.IsCancellationRequested)
            {
                //cancellationTokenSource.Dispose();
                cancellationTokenSource = new CancellationTokenSource();
            }

            Detail = new NavigationPage(new GeolocationPage(meteoHealthRepository, cancellationTokenSource.Token ));
			IsPresented = false;
		}
        private void OnAboutClicked(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();

            if (cancellationTokenSource.IsCancellationRequested)
            {
                //cancellationTokenSource.Dispose();
                cancellationTokenSource = new CancellationTokenSource();
            }

            Detail = new NavigationPage(new AboutPage());
            IsPresented = false;
         
        }
        private void OnReportClicked(object sender, EventArgs e)
        {
            cancellationTokenSource?.Cancel();

            if (cancellationTokenSource.IsCancellationRequested)
            {
                //cancellationTokenSource.Dispose();
                cancellationTokenSource = new CancellationTokenSource();
            }
            Detail = new NavigationPage(new ReportPage(meteoHealthRepository, reportMaker, cancellationTokenSource.Token));
            IsPresented = false;
           
        }
    }
}