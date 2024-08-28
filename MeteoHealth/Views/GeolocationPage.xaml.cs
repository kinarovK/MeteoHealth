using MeteoHealth.ViewModels;
using SQLite_Database_service;
using SQLite_Database_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace MeteoHealth.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeolocationPage : ContentPage
    {
        public GeolocationPage(IMeteoHealthRepository meteoHealthRepository)
        {
            InitializeComponent();
            BindingContext = new GeolocationPageViewModel(meteoHealthRepository);

        }

        private void MapClicked(object sender, MapClickedEventArgs e)
        {
            var viewModel = BindingContext as GeolocationPageViewModel;
            viewModel.HandleMapClickedCommand.Execute(e.Position);
        }

        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    var viewModel = BindingContext as GeolocationPageViewModel;
        //    viewModel.GetCurrentLocationCommand.Execute(null);
        //}
    }
}
   