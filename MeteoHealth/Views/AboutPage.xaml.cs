using MeteoHealth.ViewModels;
using Xamarin.Forms;

namespace MeteoHealth.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            BindingContext = new AboutViewModel();
        }
    }
}