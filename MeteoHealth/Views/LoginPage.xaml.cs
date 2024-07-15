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
    public partial class LoginPage : ContentPage
    {
        public LoginPage(IMeteoHealthRepository meteoHealthRepository)
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
            this.meteoHealthRepository = meteoHealthRepository;
            _geocoder = new Geocoder();
        }
        private readonly Geocoder _geocoder;
        private readonly IMeteoHealthRepository meteoHealthRepository;
        public async Task<bool> ShowConfirmationDialog(Position pos)
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Alert", 
                $"Are u sure that want the {pos.Latitude} - {pos.Longitude} as position? ", "Ok", "Cancel");
            return answer;
        }
        private async void Map_MapClicked(object sender, Xamarin.Forms.Maps.MapClickedEventArgs e)
        {
            

            if (!await ShowConfirmationDialog(e.Position))
            {
                return;
            }
            try
            {
                await DisplayAlert("Coordinates", $"Lat: {e.Position.Latitude}, Long: {e.Position.Longitude}", "OK");
                //show popup - save this location ? 
                //messagebox the xy location successfully saved

            }
            catch (FeatureNotEnabledException)
            {
                await DisplayAlert("Location Services Disabled", "Please enable location services to use this feature.", "OK");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Permission Denied", "Location permission is denied. Please grant permission to use this feature.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }

        }
        //Optimize exception handling in current class level 
        private async void Button_Clicked(object sender, EventArgs e)
        {
           
            try
            {
                var result = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Lowest, TimeSpan.FromSeconds(10)));

                if (result != null)
                {
                    var addresses = await _geocoder.GetAddressesForPositionAsync(new Position(result.Latitude, result.Longitude));
                   // await ShowConfirmationDialog(result);
                    //await DisplayAlert("Address", addresses.FirstOrDefault()?.ToString(), "OK");
                    //add some ConfirmationDialog like in another method 
                    geoLabel.Text = ($"{addresses.FirstOrDefault()?.ToString()}");
                    await meteoHealthRepository.SaveGeolocationModelAsync(new GeolocationModel
                    {
                        DateTime = DateTime.Now.ToString(),
                        Latitude = result.Latitude,
                        Longitude = result.Longitude
                    });

                    //messagebox the xy location successfully saved
                }
            }
            catch (FeatureNotEnabledException)
            {
                await DisplayAlert("Location Services Disabled", "Please enable location services to use this feature.", "OK");
            }
            catch (PermissionException)
            {
                await DisplayAlert("Permission Denied", "Location permission is denied. Please grant permission to use this feature.", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }

        }
    }
}