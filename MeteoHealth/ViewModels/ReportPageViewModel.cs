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
using System.Reflection;
using System.Collections.ObjectModel;
using MeteoHealth.Views;
using System.Threading;
using MeteoHealth.ViewModels.Models;
using Report_Service.Exceptions;
using Xamarin.CommunityToolkit.Extensions;

namespace MeteoHealth.ViewModels
{
   
    internal class ReportPageViewModel : INotifyPropertyChanged
    {
        private readonly IMeteoHealthRepository repo;
        private readonly IReportMaker reportMaker;

        public ICommand GetReportCommand { get; }
        public ICommand GetReportDetailsCommand { get; }
        public ICommand DeleteAllDataCommand { get; }
        public ICommand OpenAboutReportPageCommand { get; }
        public ReportPageViewModel(IMeteoHealthRepository repo, IReportMaker reportMaker, CancellationToken cancellationToken)
        {
            this.repo = repo;
            this.reportMaker = reportMaker;
            GetReportCommand = new Command(async() =>await GetReportAsync(cancellationToken));
            GetReportDetailsCommand = new Command(() => GetReportDetails());
            OpenAboutReportPageCommand = new Command(async () => await OpenAboutReportPageAsync());
            DeleteAllDataCommand = new Command(async () => await DeleteAllDataAsync(cancellationToken));
            listOfPotentionalRelations = new List<ResultModel>();
            listOfPossibleRelations = new List<ResultModel>();
            

            PotentialRelationsList = new ObservableCollection<ResultModel>();
            PossibleRelationsList = new ObservableCollection<ResultModel>();
        }

        private async Task OpenAboutReportPageAsync( )
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new AboutReportPage());
        }

        private string reportPeriod;
        public string ReportPeriod
        {
            get { return reportPeriod; }
            set
            {
                if (reportPeriod != value)
                {
                    reportPeriod = value;
                    OnPropertyChanged(nameof(ReportPeriod));
                }
            }
        }
        private bool pontetionalRelationshipLabelIsVisible;

        public bool IsRelationshipListVisible
        {
            get { return pontetionalRelationshipLabelIsVisible; }
            set
            {
                if (pontetionalRelationshipLabelIsVisible != value)
                {
                    pontetionalRelationshipLabelIsVisible = value;
                    OnPropertyChanged(nameof(IsRelationshipListVisible));
                }
            }
        }

        private bool possibleRelationshipLabelIsVisible;

        public bool PossibleRelationshipLabelIsVisible
        {
            get { return possibleRelationshipLabelIsVisible; }
            set
            {
                if (possibleRelationshipLabelIsVisible != value)
                {
                    possibleRelationshipLabelIsVisible = value;
                    OnPropertyChanged(nameof(PossibleRelationshipLabelIsVisible));
                }
            }
        }

        private List<ResultModel> listOfPotentionalRelations;
        private List<ResultModel> listOfPossibleRelations;
        private bool isGetDetailReportButtonVisible;

        public bool IsGetDetailReportButtonEnabled
        {
            get { return isGetDetailReportButtonVisible; }
            set
            {
                if (isGetDetailReportButtonVisible != value)
                {
                    isGetDetailReportButtonVisible = value;
                    OnPropertyChanged(nameof(IsGetDetailReportButtonEnabled));
                }
            }
        }
        private ObservableCollection<ResultModel> detailedReportList;
        public ObservableCollection<ResultModel> DetailedReportList
        {
            get { return detailedReportList; }
            set
            {
                detailedReportList = value;
                OnPropertyChanged(nameof(DetailedReportList));
            }
        }

        private ObservableCollection<ResultModel> possibleRelationsList;
        public ObservableCollection<ResultModel> PossibleRelationsList
        {
            get => possibleRelationsList;
            set
            {
                possibleRelationsList = value;
                OnPropertyChanged(nameof(PossibleRelationsList));
            }
        }
        private ObservableCollection<ResultModel> potentialRelationsList;
        public ObservableCollection<ResultModel> PotentialRelationsList
        {

            get => potentialRelationsList;
            set
            {
                potentialRelationsList = value;
                OnPropertyChanged(nameof(PotentialRelationsList));
            }
        }
        ReportModel reportModel;
        public async Task GetReportAsync(CancellationToken cancellationToken)
        {
            PotentialRelationsList.Clear();
            listOfPotentionalRelations.Clear();

            PossibleRelationsList.Clear();
            listOfPossibleRelations.Clear();

            try
            {
                await FillMissingDates(cancellationToken);
                reportModel = await reportMaker.GetReportAsync(cancellationToken);

            }
            catch (OperationCanceledException)
            {
                //Task cancelled
            }
            catch (NotEnoughDataToReportException)
            {
                await Application.Current.MainPage.DisplayAlert("Ooops", "Not enough data to summarize. Please continue to check your health state.", "OK");
                return;
                //PossibleRelationshipLabelIsVisible = false;
            }
            catch(Exception ex )
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"Something went wrong. Please try again later. {ex.Message}", "OK");
                return;
            }
            /*var result*/

            //if (/*result*/reportModel == null)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Ooops", "Not enough data to summarize. Please continue to check your health state.", "OK");
            //    return;
            //}
            //PossibleRelationshipLabelIsVisible = true;
            //reportModel = await reportMaker.GetReport(cancellationToken);
            //reportModel = new ReportModel
            //{
            //    TemperatureRelation = 0.15,
            //    HumidityRelation = 0.50,
            //    PressureRelation = 0.87,
            //    PrecVolRelation = 0.45,
            //    PrecProbabilityRelation = 0.8,
            //    WindRelation = 0.01,
            //    FullRelation = 0.005,
            //    FirstDate = "2000.05.15",
            //    LastDate = "2010.04.15"
            //};

            IsBiggerThanThresholdValue(reportModel);
            ChangeNaming(ref listOfPotentionalRelations);
            ChangeNaming(ref listOfPossibleRelations);

            foreach (var item in listOfPotentionalRelations)
            {
                PotentialRelationsList.Add(new ResultModel { Name = item.Name, Value = item.Value });
            }
            foreach (var item in listOfPossibleRelations)
            {
                PossibleRelationsList.Add(new ResultModel { Name = item.Name, Value = item.Value });
            }
            if (listOfPotentionalRelations.Count == 0 && listOfPossibleRelations.Count == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Hmm", "No significant relationship was found between your health states and the weather parameters. Please continue to monitor your health state.", "OK");
                IsRelationshipListVisible = false;
                IsGetDetailReportButtonEnabled = true;
                return;


            }
            IsGetDetailReportButtonEnabled = true;
            IsRelationshipListVisible = true;

        }
        private List<ResultModel> tempResults;

        public async Task FillMissingDates(CancellationToken cancellationToken)
        {
            var dates =await reportMaker.CheckAbsentDates(cancellationToken);
            if (dates.Count == 0)
            {
                return;
            }
            else
            {
                foreach (var date in dates)
                {
                    await Application.Current.MainPage.Navigation.ShowPopupAsync(new HealthStatePopup(repo, $"Oops, it looks like you didn't check the state at " +
                        $"{date.ToString("MM.dd")}.  How were you feeling on this day?", date)); 
                }
            }
        }
        public void IsBiggerThanThresholdValue(ReportModel reportModel)
        {
            tempResults = new List<ResultModel>();

            foreach (var item in typeof(ReportModel).GetProperties())
            {
                Double.TryParse(item.GetValue(reportModel).ToString(), out double value);

                if (value >= 0.75)
                {
                    listOfPotentionalRelations.Add(new ResultModel { Name = item.Name, Value = value });
                }
                else if (value >= 0.5)
                {
                    listOfPossibleRelations.Add(new ResultModel { Name = item.Name, Value = value});
                }
                if (item.Name.Contains("Date"))
                {
                    continue;
                }
                tempResults.Add(new ResultModel { Name = item.Name, Value = value });
            }
        }

        public void ChangeNaming(ref List<ResultModel> targetList)
        {
            for (int i = 0; i < targetList.Count; i++)
            {
                switch(targetList[i].Name)
                {
                    case "TemperatureRelation":
                        targetList[i].Name = "Temperature (C°)";
                        break;
                    case "HumidityRelation":
                        targetList[i].Name = "Humidity (%)";
                        break;
                    case "PressureRelation":
                        targetList[i].Name = "Pressure (mhp)";
                        break;
                    case "PrecVolRelation":
                        targetList[i].Name = "Precipitation volume (cm)";
                        break;
                    case "PrecProbabilityRelation":
                        targetList[i].Name = "Precipitation probability (%)";
                        break;
                    case "WindRelation":
                        targetList[i].Name = "Wind (km/h)";
                        break;
                    case "FullRelation":
                        targetList[i].Name = "All weather parameters";
                        break;
                }
                    
            }
        }
        private string daysInReport;

        public string DaysInReport
        {
            get { return daysInReport; }
            set
            {
                daysInReport = value;
                OnPropertyChanged(nameof(DaysInReport));
            }
        }
        public void GetReportDetails()
        {

            //days in report 
            //table
            DateTime.TryParse(reportModel.FirstDate, out var firstDate);
            DateTime.TryParse(reportModel.LastDate, out var lastDate);

            if (firstDate != null && lastDate != null )
            {
                DaysInReport = (lastDate - firstDate).TotalDays.ToString();
            }
            DetailedReportList = new ObservableCollection<ResultModel>();
            ChangeNaming(ref tempResults);
            foreach (var item in tempResults)
            {
                 DetailedReportList.Add(new ResultModel { Name = item.Name, Value= item.Value });

            }
            ReportPeriod = $"Report from {firstDate.Date.ToShortDateString()}-{lastDate.Date.ToShortDateString()}";


        }

        public async Task DeleteAllDataAsync(CancellationToken cancellationToken)
        {
            bool answer = await Application.Current.MainPage.DisplayAlert("Confirm Delete", "Are you sure you want to delete all data?", "Yes", "Cancel");

            if (answer)
            {
                await repo.ClearDatabase(cancellationToken);
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName )
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
