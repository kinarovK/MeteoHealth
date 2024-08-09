using SQLite_Database_service;
using SQLite_Database_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MeteoHealth.ViewModels
{
    internal class HealthStatePopupViewModel : BaseViewModel
    {
        private double _healthLevel;

        private readonly IMeteoHealthRepository _meteoHealthRepository;

        public double HealthLevel
        { 
            get 
            { 
                return _healthLevel; 
            }
        
            set 
            {
                _healthLevel = value; 
                OnPropertyChanged();
            } 
        }
        public HealthStatePopupViewModel(IMeteoHealthRepository meteoHealthRepository)
        {
            _meteoHealthRepository = meteoHealthRepository;
            HealthLevel = 3;
            SaveCommand = new Command(OnSave);
        }
        public ICommand SaveCommand { get; }
        private void OnSave()
        {
            var model = new HealthStateModel
            {
                HealthLevel = (byte)HealthLevel,
                Date = DateTime.Today.ToString()
            };

            _meteoHealthRepository.SaveHealtStateModel(model);
            MessagingCenter.Send(this, "ClosePopup");
            
        }
    }
}
