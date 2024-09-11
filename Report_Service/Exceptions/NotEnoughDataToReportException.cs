using System;
using System.Collections.Generic;
using System.Text;

namespace Report_Service.Exceptions
{
    public class NotEnoughDataToReportException : BaseCustomException
    {
        public NotEnoughDataToReportException() : base("Not enough data to report")
        {
        }
    }
}
