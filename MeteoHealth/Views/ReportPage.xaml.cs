using MeteoHealth.ViewModels;
using Report_Service.Interfaces;
using SQLite_Database_service.Interfaces;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeteoHealth.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPage : ContentPage
    {
        public ReportPage(IMeteoHealthRepository repo, IReportMaker reportMaker, CancellationToken cancellationToken)
        {
            InitializeComponent();
            BindingContext = new ReportPageViewModel(repo, reportMaker, cancellationToken);
            
        }
    }
}