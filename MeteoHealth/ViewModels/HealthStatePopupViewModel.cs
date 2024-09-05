﻿using SQLite_Database_service;
using SQLite_Database_service.Interfaces;
using SQLite_Database_service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MeteoHealth.ViewModels
{
    internal class HealthStatePopupViewModel : BaseViewModel
    {
        private double _healthLevel;
        private readonly DateTime date;
        
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
        public HealthStatePopupViewModel(IMeteoHealthRepository meteoHealthRepository, DateTime date)
        {
            _meteoHealthRepository = meteoHealthRepository;
            this.date = date;
          
            HealthLevel = 3;
            SaveCommand = new Command(async () => await OnSave());
        }
        public ICommand SaveCommand { get; }
        private async Task OnSave()
        {
            var model = new HealthStateModel
            {
                HealthLevel = (byte)HealthLevel,
                Date = date.ToString()
            };

            await _meteoHealthRepository.SaveHealtStateModel(model);
            MessagingCenter.Send(this, "ClosePopup"); //System.Reflection.TargetInvocationException: 'Exception has been thrown by the target of an invocation.'

        }
    }
}
