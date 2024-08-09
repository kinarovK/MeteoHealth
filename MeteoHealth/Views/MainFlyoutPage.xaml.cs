using MeteoHealth.Services;
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

        public MainFlyoutPage (IMeteoHealthRepository repo, IChartMaker chMaker, IReportMaker reportMaker)
		{
			InitializeComponent ();
			meteoHealthRepository = repo;
			chartMaker = chMaker;
            this.reportMaker = reportMaker;
            Detail = new NavigationPage(CreateMainPage());
		}
		private async void OnHomeClicked(object sender, EventArgs e)
		{
			Detail = new NavigationPage(new MainPage(meteoHealthRepository, chartMaker));
			IsPresented = true;
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
            return new MainPage(meteoHealthRepository, chartMaker);
        }



        private void OnReportClicked(object sender, EventArgs e)
        {
			Detail = new NavigationPage(new ReportPage(meteoHealthRepository, reportMaker));
			IsPresented = false;
        }
    }
}