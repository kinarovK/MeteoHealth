﻿using SQLite_Database_service.Interfaces;
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

namespace MeteoHealth.ViewModels
{
    public class ResultModel
    {
        public string Name { get; set; }
        public double Value { get; set; }
    }
    internal class ReportPageViewModel : INotifyPropertyChanged
    {
        private readonly IMeteoHealthRepository repo;
        private readonly IReportMaker reportMaker;
        private readonly CancellationToken cancellationToken;

        public ICommand GetReportCommand { get; }
        public ICommand GetReportDetailsCommand { get; }
        public ICommand DeleteAllDataCommand { get; }
        public ICommand OpenAboutReportPageCommand { get; }
        public ReportPageViewModel(IMeteoHealthRepository repo, IReportMaker reportMaker, CancellationToken cancellationToken)
        {
            this.repo = repo;
            this.reportMaker = reportMaker;
            this.cancellationToken = cancellationToken;
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
            //await Application.Current.MainPage.Navigation.PushAsync(new AboutReportPage());
            await Application.Current.MainPage.Navigation.PushModalAsync(new AboutReportPage());

            //Application.Current.MainPage = new NavigationPage(new AboutReportPage());
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

        public bool PotentionalRelationshipLabelIsVisible
        {
            get { return pontetionalRelationshipLabelIsVisible; }
            set
            {
                if (pontetionalRelationshipLabelIsVisible != value)
                {
                    pontetionalRelationshipLabelIsVisible = value;
                    OnPropertyChanged(nameof(PotentionalRelationshipLabelIsVisible));
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

        public bool IsGetDetailReportButtonVisible
        {
            get { return isGetDetailReportButtonVisible; }
            set
            {
                if (isGetDetailReportButtonVisible != value)
                {
                    isGetDetailReportButtonVisible = value;
                    OnPropertyChanged(nameof(IsGetDetailReportButtonVisible));
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
            var result = await reportMaker.GetReport(cancellationToken );

            if (result == null)
            {
                await Application.Current.MainPage.DisplayAlert("Ooops", "Not enought data to summary, keep going to check health state", "OK");
                return;
            }
            PotentionalRelationshipLabelIsVisible = true;
            PossibleRelationshipLabelIsVisible = true;
            reportModel = await reportMaker.GetReport(cancellationToken);
            IsBiggerThanThresholdValue(reportModel);
            ChangeNaming(ref listOfPotentionalRelations);
            ChangeNaming(ref listOfPossibleRelations);

            foreach (var item in listOfPotentionalRelations)
            {
                PotentialRelationsList.Add(new ResultModel { Name = item.Name, Value = item.Value});
            }
            foreach (var item in listOfPossibleRelations)
            {
                PossibleRelationsList.Add(new ResultModel { Name = item.Name, Value=item.Value});
            }
            IsGetDetailReportButtonVisible = true;
        }
        private List<ResultModel> tempResults;


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
            ReportPeriod = $"Report from {reportModel.FirstDate}-{reportModel.LastDate}";


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
