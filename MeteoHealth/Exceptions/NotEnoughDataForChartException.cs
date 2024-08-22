using System;
using System.Collections.Generic;
using System.Text;

namespace MeteoHealth.Exceptions
{
    public class NotEnoughDataForChartException : BaseCustomException
    {
        public NotEnoughDataForChartException() : base("Not enought data to display the charts")
        {
        }
    }
}
