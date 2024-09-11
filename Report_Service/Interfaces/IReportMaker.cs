using Report_Service.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Report_Service.Interfaces
{
    public interface IReportMaker
    {

        public Task<ReportModel> GetReportAsync(CancellationToken cancellationToken);
    }
}