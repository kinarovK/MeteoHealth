﻿using MeteoHealth.Services;
using MeteoHealth.ViewModels;
using Report_Service.Interfaces;
using SQLite_Database_service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MeteoHealth.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPage : ContentPage
    {
        private readonly IMeteoHealthRepository repo;

        public ReportPage(IMeteoHealthRepository repo, IReportMaker reportMaker, CancellationToken cancellationToken)
        {
            InitializeComponent();
            this.repo = repo;
            BindingContext = new ReportPageViewModel(repo, reportMaker, cancellationToken);
            
        }
    }
}