using MeteoHealth.Services;
using MeteoHealth.Views;
using SQLite_Database_service;
using SQLite_Database_service.Interfaces;
using SQLite_Database_service.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MeteoHealth.ViewModels
{
    internal class GeolocationPageViewModel : BaseViewModel//INotifyPropertyChanged
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
           
        }
        private async Task HandleMapClickedAsync(Position position )
        {
            if (!await ShowConfirmationDialog(position.Latitude.ToString(), position.Longitude.ToString()))
            {
                return;
            }

            try
            {

              
                await meteoHealthRepository.SaveGeolocationModelAsync(new GeolocationModel
                {
                    DateTime = DateTime.Now.ToString(),
                    Latitude = position.Latitude,
                    Longitude = position.Longitude
                });

              
                GeoLabel = $"Lat: {position.Latitude}, Long: {position.Longitude}";
            }
       
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong. Please try again later. {ex.Message}", "OK");
            }
          
        }

        
        public async Task<bool> ShowConfirmationDialog(string latitude, string longitude)
        {
            return await Application.Current.MainPage.DisplayAlert("Alert",
                $"Are you sure you want to use {latitude} - {longitude} as your position?", "Ok", "Cancel");
        }
        private async Task MapClickedAsync(Position position)
        {
            if (!await ShowConfirmationDialog(position.Latitude.ToString(), position.Longitude.ToString()))
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
                await Application.Current.MainPage.DisplayAlert("Location Services are Disabled", "Please enable location services to use this feature.", "OK");
            }
            catch (PermissionException)
            {
                await Application.Current.MainPage.DisplayAlert("Permission Denied", "Location permission is denied. Please grant permission to use this feature.", "OK");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong. Please try again later. {ex.Message}", "OK");
            }
        }
        private async Task GetLocationAsync()
        {
            try
            {
                var result = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Lowest, TimeSpan.FromSeconds(10)));

                if (!await ShowConfirmationDialog(result.Latitude.ToString(), result.Latitude.ToString()))
                {
                    return;
                }
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

                    await Application.Current.MainPage.DisplayAlert("Success", "Location has been successfully saved.", "OK");

                }
            }
            catch (FeatureNotEnabledException)
            {
                await Application.Current.MainPage.DisplayAlert("Location Services are Disabled", "Please enable location services to use this feature.", "OK");
            }
            catch (PermissionException)
            {
                await Application.Current.MainPage.DisplayAlert("Permission Denied", "Location permission is denied. Please grant permission to use this feature.", "OK");
            }
         
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong. Please try again later. {ex.Message}" , "OK");
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
    }
}
