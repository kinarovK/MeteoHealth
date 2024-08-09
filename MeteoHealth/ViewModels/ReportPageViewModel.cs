using SQLite_Database_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Report_Service.Interfaces;
using Report_Service.Models;
using System.ComponentModel;

namespace MeteoHealth.ViewModels
{
    internal class ReportPageViewModel : INotifyPropertyChanged
    {
        private readonly IMeteoHealthRepository repo;
        private readonly IReportMaker reportMaker;

        public ICommand GetReportCommand { get; }

        public ReportPageViewModel(IMeteoHealthRepository repo, IReportMaker reportMaker)
        {
            this.repo = repo;
            this.reportMaker = reportMaker;
            GetReportCommand = new Command(async () => await GetReportAsync()); 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async Task<ReportModel> GetReportAsync()
        {
            var result = reportMaker.GetReport();
            return result;
        }
    }
}
