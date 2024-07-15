using SQLite_Database_service;
using SQLite_Database_service.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeteoHealth.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HealthStatePopup : Popup
    { 
        
        public HealthStatePopup(IMeteoHealthRepository repo)
        {
            InitializeComponent();
            this.repo = repo;
        }

     
        private void SetButton_Clicked(object sender, EventArgs e)
        {
            var sliderValie = (byte)StateSlider.Value;
            HealthStateModel model = new HealthStateModel();

            model.HealthLevel = sliderValie;
            model.Date = DateTime.Today.ToString();


            repo.SaveHealtStateModel(model);
            //Some mock method 
            Dismiss(null);
        }
        private const double StepValue = 1.0;
        private readonly IMeteoHealthRepository repo;

        private void StateSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue);
            StateSlider.Value = newStep * StepValue;
            SliderValue.Text = StateSlider.Value.ToString();
        }
    }
}