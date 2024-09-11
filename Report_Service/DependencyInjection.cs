using Microsoft.Extensions.DependencyInjection;
using Report_Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Report_Service
{
    public static class DependencyInjection
    {
        public static void DependencyRegistrationForReport(this IServiceCollection services)
        {
            services.AddScoped<IReportMaker, ReportMaker>();    
            services.AddScoped<IDataPreparator, DataPreparator>();
            services.AddScoped<IRegressionCalculator, RegressionCalculator>();

        }
    }
}
