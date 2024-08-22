using MeteoHealth.Services;
using OpenWeatherMap_Api_Service.Interfaces;
using Report_Service.Interfaces;
using SQLite_Database_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private readonly IApiController apiController;
        private readonly IWeatherApiService apiService;
        private MainPage mainPage;
        public MainFlyoutPage (IMeteoHealthRepository repo, IChartMaker chMaker, IReportMaker reportMaker, IApiController apiController, IWeatherApiService apiService)
		{
			InitializeComponent ();
			meteoHealthRepository = repo;
			chartMaker = chMaker;
            this.reportMaker = reportMaker;
            this.apiController = apiController;
            this.apiService = apiService;
            mainPage = CreateMainPage();
            Detail = new NavigationPage(mainPage);
		}
		private async void OnHomeClicked(object sender, EventArgs e)
		{
            //Detail = new NavigationPage(new MainPage(meteoHealthRepository, chartMaker));
            //IsPresented = true;
            if (Detail is NavigationPage navigationPage && navigationPage.CurrentPage == mainPage)
            {
                // No need to create a new page; just show it
                IsPresented = false;
            }
            else
            {
                Detail = new NavigationPage(mainPage);
                IsPresented = false;
            }
        }
		private async void OnGeolocationClicked(object sender, EventArgs e)
		{
			Detail = new NavigationPage(new GeolocationPage(meteoHealthRepository));
			IsPresented = false;
		}
        private async void OnAboutClicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new AboutPage());
            IsPresented = false; 
        }
        private MainPage CreateMainPage()
        {
            
            return new MainPage(meteoHealthRepository, chartMaker, apiController, apiService);
        }



        private void OnReportClicked(object sender, EventArgs e)
        {
			Detail = new NavigationPage(new ReportPage(meteoHealthRepository, reportMaker));
			IsPresented = false;
        }
    }
}