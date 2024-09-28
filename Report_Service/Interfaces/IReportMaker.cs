using Report_Service.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Report_Service.Interfaces
{
    public interface IReportMaker
    {
        public Task<List<DateTime>> CheckAbsentDates(CancellationToken cancellationToken);
        public Task<ReportModel> GetReportAsync(CancellationToken cancellationToken);
    }
}