using MeteoHealth.ViewModels;
using SQLite_Database_service;
using SQLite_Database_service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeteoHealth.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HealthStatePopup : Popup, INotifyPropertyChanged
    {

        public HealthStatePopup(IMeteoHealthRepository repo, string message, DateTime date)
        {
            InitializeComponent();
            BindingContext = new HealthStatePopupViewModel(repo, date);
            title.Text = message;
            
            MessagingCenter.Subscribe<HealthStatePopupViewModel>(this, "ClosePopup",  (sender) =>
            {
                  
                  Dismiss(null);
            });
        
        }
        private const double StepValue = 1.0;
      
        private void StateSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue);
            StateSlider.Value = newStep * StepValue;
        }
    }
}