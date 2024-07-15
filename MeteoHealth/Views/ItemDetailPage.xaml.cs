using MeteoHealth.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace MeteoHealth.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}