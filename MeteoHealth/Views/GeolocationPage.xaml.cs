using MeteoHealth.ViewModels;
using SQLite_Database_service.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MeteoHealth.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeolocationPage : ContentPage
    {
        public GeolocationPage(IMeteoHealthRepository meteoHealthRepository )
        {
            InitializeComponent();
            BindingContext = new GeolocationPageViewModel(meteoHealthRepository);

        }
        //Google Map api not have direct implementation to work with VM
        private void MapClicked(object sender, MapClickedEventArgs e)
        {
            var viewModel = BindingContext as GeolocationPageViewModel;
            viewModel.HandleMapClickedCommand.Execute(e.Position);
        }


    }
}
   