using MeteoHealth.Services;
using MeteoHealth.Views;
using SQLite_Database_service;
using SQLite_Database_service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MeteoHealth.ViewModels
{
    internal class GeolocationPageViewModel : INotifyPropertyChanged
    {
        private readonly IMeteoHealthRepository meteoHealthRepository;
        private readonly Geocoder geocoder;
        public ICommand HandleMapClickedCommand { get; }
        public ICommand GetCurrentLocationCommand { get; }
        public GeolocationPageViewModel(IMeteoHealthRepository meteoHealthRepository)
        {
            this.meteoHealthRepository = meteoHealthRepository;
            geocoder = new Geocoder();
            MapClickedCommand = new Command<MapClickedEventArgs>(async e => await MapClickedAsync(e.Position));
            GetLocationCommand = new Command(async () => await GetLocationAsync());

            HandleMapClickedCommand = new Command<Position>(async pos => await HandleMapClickedAsync(pos));
            GetCurrentLocationCommand = new Command(async () => await GetCurrentLocationAsync());
        }
        private async Task HandleMapClickedAsync(Position position)
        {
            if (!await ShowConfirmationDialog(position))
            {
                return;
            }

            try
            {
                // Save the geolocation
                await meteoHealthRepository.SaveGeolocationModelAsync(new GeolocationModel
                {
                    DateTime = DateTime.Now.ToString(),
                    Latitude = position.Latitude,
                    Longitude = position.Longitude
                });

                // Optionally update the label or other UI elements here
                GeoLabel = $"Lat: {position.Latitude}, Long: {position.Longitude}";
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }

        private async Task GetCurrentLocationAsync()
        {
            try
            {
                var result = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Low, TimeSpan.FromSeconds(10)));

                if (result != null)
                {
                    var addresses = await geocoder.GetAddressesForPositionAsync(new Position(result.Latitude, result.Longitude));
                    GeoLabel = addresses.FirstOrDefault()?.ToString();

                    await meteoHealthRepository.SaveGeolocationModelAsync(new GeolocationModel
                    {
                        DateTime = DateTime.Now.ToString(),
                        Latitude = result.Latitude,
                        Longitude = result.Longitude
                    });
                }

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }

        //private async Task<bool> ShowConfirmationDialog(Position pos)
        //{
        //    return await Application.Current.MainPage.DisplayAlert("Alert",
        //        $"Are you sure that you want {pos.Latitude} - {pos.Longitude} as position?", "Ok", "Cancel");
        //}
        public async Task<bool> ShowConfirmationDialog(Position pos)
        {
            return await Application.Current.MainPage.DisplayAlert("Alert",
                $"Are u sure that want the {pos.Latitude} - {pos.Longitude} as position? ", "Ok", "Cancel");
        }
        private async Task MapClickedAsync(Position position)
        {
            if (!await ShowConfirmationDialog(position))
            {
                return;
            }

            try
            {
                GeoLabel = $"Lat: {position.Latitude}, Long: {position.Longitude}";
                await Application.Current.MainPage.DisplayAlert("Coordinates", GeoLabel, "OK");


            }
            catch (FeatureNotEnabledException)
            {
                await Application.Current.MainPage.DisplayAlert("Location Services Disabled", "Please enable location services to use this feature.", "OK");
            }
            catch (PermissionException)
            {
                await Application.Current.MainPage.DisplayAlert("Permission Denied", "Location permission is denied. Please grant permission to use this feature.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }
        private async Task GetLocationAsync()
        {
            try
            {
                var result = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Lowest, TimeSpan.FromSeconds(10)));

                if (result != null)
                {
                    var addresses = await geocoder.GetAddressesForPositionAsync(new Position(result.Latitude, result.Longitude));
                    GeoLabel = addresses.FirstOrDefault().ToString();

                    await meteoHealthRepository.SaveGeolocationModelAsync(new GeolocationModel
                    {
                        DateTime = DateTime.Now.ToString(),
                        Latitude = result.Latitude,
                        Longitude = result.Longitude
                    });

                    await Application.Current.MainPage.DisplayAlert("Success", "Location successfully saved.", "OK");

                }
            }
            catch (FeatureNotEnabledException)
            {
                await Application.Current.MainPage.DisplayAlert("Location Services Disabled", "Please enable location services to use this feature.", "OK");
            }
            catch (PermissionException)
            {
                await Application.Current.MainPage.DisplayAlert("Permission Denied", "Location permission is denied. Please grant permission to use this feature.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An unexpected error occurred: {ex.Message}", "OK");
            }
        }
        public ICommand MapClickedCommand { get; }
        public ICommand GetLocationCommand { get; }

        private string geoLabel;
        public string GeoLabel
        {
            get { return geoLabel; } set
            {
                geoLabel = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
